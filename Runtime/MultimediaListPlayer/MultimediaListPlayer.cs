using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [RequireComponent(typeof(MultimediaListManager))]
    public class MultimediaListPlayer : MultimediaStandardPlayer, IMultimediaListPlayer
    {
        [Tooltip("运行时自动打开媒体(如有)")] public bool autoOpenMedia = true;
        [Tooltip("运行时自动开始播放媒体")] public bool autoPlayMedia = true;
        public int currentListIndex;

        [Tooltip("多媒体循环模式：0：单曲播放 1：单曲循环 2：列表循环 3：跨列表循环 4：列表随机播放 5：跨列表随机播放")]
        public MultimediaPlayingMode playingMode = MultimediaPlayingMode.ListLoop;

        [Tooltip("播放完成识别误差")] public float completedDeviation = 0.2F;
        public UnityEvent onCompleted = new UnityEvent();

        [SerializeField] private List<MultimediaListManager> multimediaLists;
        private MultimediaListManager currentMultimediaList => multimediaLists[currentListIndex];

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            multimediaLists = GetComponents<MultimediaListManager>().ToList();
            yield return new WaitUntil(() => currentMultimediaList.loadingCompleted);
            if (currentMultimediaList.IsEmpty || !autoOpenMedia) yield break;
            OpenMedia(currentMultimediaList.mediaReferenceInfos[0], autoPlayMedia);
        }

        private bool _invokeOnFinished;

        protected virtual void Update()
        {
            mediaPlayer.Loop = playingMode == MultimediaPlayingMode.Loop;
            OnCompletedUpdate();
        }

        #region private methods

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnCompletedUpdate()
        {
            var playbackCompleted = DetectedPlaybackCompleted();
            if (playbackCompleted)
            {
                onCompleted?.Invoke();
                switch (playingMode)
                {
                    case MultimediaPlayingMode.Once:
                        break;
                    case MultimediaPlayingMode.Loop:
                        RePlayback();
                        break;
                    case MultimediaPlayingMode.ListLoop:
                        PlayNextMedia();
                        break;
                    case MultimediaPlayingMode.CrossListLoop:
                        PlayNextMedia(false);
                        break;
                    case MultimediaPlayingMode.ListRandom:
                        PlayIndex(Random.Range(0, currentMultimediaList.mediaReferenceInfos.Count));
                        break;
                    case MultimediaPlayingMode.CrossListRandom:
                        currentListIndex = Random.Range(0, multimediaLists.Count);
                        var mediaIndex = Random.Range(0, currentMultimediaList.mediaReferenceInfos.Count);
                        PlayIndex(mediaIndex);
                        break;
                    default:
                        return;
                }

                _invokeOnFinished = true;
            }

            if (_invokeOnFinished && !playbackCompleted) _invokeOnFinished = false;
        }

        private bool DetectedPlaybackCompleted()
        {
            var duration = mediaPlayer.Info.GetDuration();
            var currentTime = mediaPlayer.Control.GetCurrentTime();
            return duration > 0 && currentTime > 0 && (currentTime > duration - completedDeviation);
        }

        #endregion

        #region API

        //TODO 当playerUI使用音频衰减时暂停视频后播放不会播放，禁用音频衰减可以解决此问题

        public void PlayMediaSegment(MediaSection segment)
        {
            if (segment.mediaSectionIsValid) SetProgress(segment.SectionLocation);
        }

        public void PlayMediaSegment(int segmentIndex)
        {
            var info = GetCurrentMediaReferenceInfo();
            if (info == null || segmentIndex < 0 || segmentIndex >= info.sections.Count) return;
            var segment = info.sections[segmentIndex];
            if (segment.mediaSectionIsValid) SetProgress(segment.SectionLocation);
        }

        public MediaReferenceInfo GetCurrentMediaReferenceInfo()
        {
            return currentMultimediaList.mediaReferenceInfos.FirstOrDefault(info =>
                info.mediaReference == mediaPlayer.MediaReference);
        }

        public void PlayIndex(int index)
        {
            if (currentMultimediaList.mediaReferenceInfos.IsValidIndex(index))
                OpenMedia(currentMultimediaList.mediaReferenceInfos[index].mediaReference, true);
        }

        public void PlayListIndex(int listIndex)
        {
            throw new NotImplementedException();
        }

        public void PlayListIndexMedia(int listIndex, int index)
        {
            if (multimediaLists.IsInvalidIndex(listIndex)) return;
            var targetList = multimediaLists[listIndex];
            if (targetList.mediaReferenceInfos.IsInvalidIndex(index)) return;
            currentListIndex = listIndex;
            PlayIndex(index);
        }


        public void PlayNextMedia(bool onlyCurrentList = true)
        {
            if (currentMultimediaList.mediaReferenceInfos.Count <= 1)
            {
                RePlayback();
                return;
            }

            var currentIndex = GetCurrentMediaIndex();

            if (!onlyCurrentList && NextMultimediaList())
            {
                PlayIndex(0);
            }
            else
            {
                var nextIndex = (currentIndex + 1 + currentMultimediaList.mediaReferenceInfos.Count) %
                                currentMultimediaList.mediaReferenceInfos.Count;
                PlayIndex(nextIndex);
            }
        }

        public void PlayPreviousMedia(bool onlyCurrentList = true)
        {
            if (currentMultimediaList.mediaReferenceInfos.Count <= 1)
            {
                RePlayback();
                return;
            }

            var currentIndex = GetCurrentMediaIndex();

            if (!onlyCurrentList && PreviousMultimediaList())
            {
                PlayIndex(currentMultimediaList.mediaReferenceInfos.Count - 1);
            }
            else
            {
                var previousIndex = (currentIndex - 1 + currentMultimediaList.mediaReferenceInfos.Count) %
                                    currentMultimediaList.mediaReferenceInfos.Count;
                PlayIndex(previousIndex);
            }
        }

        public int GetCurrentMediaIndex()
        {
            return currentMultimediaList.mediaReferenceInfos.FindIndex(
                media => media.mediaReference.Equals(mediaPlayer.MediaReference));
        }


        public void PlayNextList(int mediaIndex = 0)
        {
            if (NextMultimediaList()) PlayIndex(mediaIndex);
        }

        public void PlayPreviousList(int mediaIndex = 0)
        {
            if (PreviousMultimediaList()) PlayIndex(mediaIndex);
        }

        public void SetMultimediaList(MultimediaListManager multimediaList)
        {
            if (multimediaLists.Contains(multimediaList))
            {
                currentListIndex = multimediaLists.IndexOf(multimediaList);
            }
            else
            {
                multimediaLists.Add(multimediaList);
                currentListIndex = multimediaLists.Count - 1;
            }
        }

        public void SetMultimediaListAndPlay(MultimediaListManager multimediaList, bool autoPlay, int index)
        {
            SetMultimediaList(multimediaList);
            if (autoPlay) PlayIndex(index);
        }

        public void TogglePlayMode()
        {
            var playModes = (MultimediaPlayingMode[])Enum.GetValues(typeof(MultimediaPlayingMode));
            var currentIndex = Array.IndexOf(playModes, playingMode);
            var nextIndex = currentIndex + 1 > playModes.Length - 1 ? 0 : currentIndex + 1;
            playingMode = playModes[nextIndex];
        }

        public void SetPlayMode(MultimediaPlayingMode mode)
        {
            playingMode = mode;
        }

        public void SetPlayMode(int playingModeIndex)
        {
            playingMode = (MultimediaPlayingMode)playingModeIndex;
        }

        public bool NextMultimediaList()
        {
            if (multimediaLists.Count <= 1) return false;
            currentListIndex = currentListIndex + 1 > multimediaLists.Count - 1 ? 0 : currentListIndex + 1;
            return true;
        }

        public bool PreviousMultimediaList()
        {
            if (multimediaLists.Count <= 1) return false;
            currentListIndex = currentListIndex - 1 < 0 ? multimediaLists.Count - 1 : currentListIndex - 1;
            return true;
        }

        #endregion
    }
}