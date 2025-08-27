using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [RequireComponent(typeof(MediaList))]
    public class MediaListPlayer : MonoBehaviour, IMediaListPlayer
    {
        [Tooltip("运行时自动打开媒体(如有)")] public bool autoOpenMedia = true;
        [Tooltip("运行时自动开始播放媒体")] public bool autoPlayMedia = true;
        public int currentListIndex;

        [Tooltip("多媒体循环模式：0：单曲播放 1：单曲循环 2：列表循环 3：跨列表循环 4：列表随机播放 5：跨列表随机播放")]
        public MediaListPlayingMode playingMode = MediaListPlayingMode.ListLoop;

        [Tooltip("播放完成识别误差")] public float completedDeviation = 0.2F;
        public UnityEvent OnCompleted = new UnityEvent();

        [SerializeField] private List<MediaList> multimediaLists;
        private MediaList currentMultimediaList => multimediaLists[currentListIndex];
        private MediaListPlayerUI mediaListPlayerUI;
        private MediaPlayer mediaPlayer => mediaListPlayerUI.mediaPlayer;
        private MediaPlayerUI mediaPlayerUI => mediaListPlayerUI.mediaPlayerUI;

        protected virtual IEnumerator Start()
        {
            multimediaLists = GetComponents<MediaList>().ToList();
            mediaListPlayerUI = GetComponentInChildren<MediaListPlayerUI>();
            yield return new WaitUntil(() => currentMultimediaList.loadingCompleted);
            if (currentMultimediaList.IsEmpty || !autoOpenMedia) yield break;
            OpenMedia(currentMultimediaList.mediaReferenceInfos[0], autoPlayMedia);
        }

        private bool invokeOnFinished;

        protected virtual void Update()
        {
            mediaPlayer.Loop = playingMode == MediaListPlayingMode.Loop;
            OnCompletedUpdate();
        }

        #region private methods

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnCompletedUpdate()
        {
            var playbackCompleted = DetectedPlaybackCompleted();
            if (playbackCompleted)
            {
                OnCompleted?.Invoke();
                switch (playingMode)
                {
                    case MediaListPlayingMode.Once:
                        break;
                    case MediaListPlayingMode.Loop:
                        RePlayback();
                        break;
                    case MediaListPlayingMode.ListLoop:
                        PlayNextMedia();
                        break;
                    case MediaListPlayingMode.CrossListLoop:
                        PlayNextMedia(false);
                        break;
                    case MediaListPlayingMode.ListRandom:
                        PlayIndex(Random.Range(0, currentMultimediaList.mediaReferenceInfos.Count));
                        break;
                    case MediaListPlayingMode.CrossListRandom:
                        currentListIndex = Random.Range(0, multimediaLists.Count);
                        var mediaIndex = Random.Range(0, currentMultimediaList.mediaReferenceInfos.Count);
                        PlayIndex(mediaIndex);
                        break;
                    default:
                        return;
                }

                invokeOnFinished = true;
            }

            if (invokeOnFinished && !playbackCompleted) invokeOnFinished = false;
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
        public void Play()
        {
            if (!mediaPlayer.Control.IsPlaying()) mediaPlayerUI.TogglePlayPause();
        }

        public void OpenMedia(string mediaPath, bool autoPlay)
        {
            var mediaFileInfo = new FileInfo(mediaPath);
            if (mediaFileInfo.Exists)
            {
                Debug.LogWarning($"指定的媒体文件【{mediaPath}】不存在！");
                return;
            }

            OpenMedia(mediaFileInfo, autoPlay);
        }

        public void OpenMedia(MediaReferenceInfo mediaReferenceInfo, bool autoPlay)
        {
            mediaListPlayerUI.mediaPlayer.OpenMedia(mediaReferenceInfo.mediaReference, autoPlay);
        }

        public void OpenMedia(MediaReference mediaReference, bool autoPlay)
        {
            mediaPlayer.OpenMedia(mediaReference, autoPlay);
        }

        public void OpenMedia(FileSystemInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            mediaPlayer.OpenMedia(mediaPath, autoPlay);
        }

        public void OpenMedia(FileInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            mediaPlayer.OpenMedia(mediaPath, autoPlay);
        }

        public void PlayMediaSegment(MediaSegment segment)
        {
            if (segment.mediaSegmentIsValid) SetProgress(segment.segmentLocation);
        }

        public void PlayMediaSegment(int segmentIndex)
        {
            var info = GetCurrentMediaReferenceInfo();
            if (info == null || segmentIndex < 0 || segmentIndex >= info.segments.Count) return;
            var segment = info.segments[segmentIndex];
            if (segment.mediaSegmentIsValid) SetProgress(segment.segmentLocation);
        }

        public MediaReferenceInfo GetCurrentMediaReferenceInfo()
        {
            return currentMultimediaList.mediaReferenceInfos.FirstOrDefault(info =>
                info.mediaReference == mediaPlayer.MediaReference);
        }

        public void TogglePlayPause()
        {
            mediaPlayerUI.TogglePlayPause();
        }

        public void Pause()
        {
            if (mediaPlayer.Control.IsPlaying()) mediaPlayerUI.TogglePlayPause();
        }

        public void Stop(double progress)
        {
            SetProgress(progress);
            Pause();
        }

        public void StopCompletely()
        {
            mediaPlayer.CloseMedia();
        }


        public void RePlayback()
        {
            SetProgress(0);
            Play();
        }

        public void AudioVolumeUp(float delta)
        {
            throw new NotImplementedException();
        }

        public void AudioVolumeDown(float delta)
        {
            throw new NotImplementedException();
        }

        public void AudioVolumeUp()
        {
            mediaPlayerUI.ChangeAudioVolume(mediaPlayerUI._keyVolumeDelta);
        }

        public void AudioVolumeDown()
        {
            mediaPlayerUI.ChangeAudioVolume(-mediaPlayerUI._keyVolumeDelta);
        }

        public void SetAudioVolume(float volume)
        {
            mediaPlayerUI._audioVolume = volume < 0 ? 0 : volume;
            mediaPlayerUI.ApplyAudioVolume();
            mediaPlayerUI.UpdateVolumeSlider();
        }

        public void ToggleMute()
        {
            mediaPlayerUI.ToggleMute();
        }

        public void SetMute()
        {
            if (!mediaPlayer.AudioMuted) mediaPlayerUI.ToggleMute();
        }

        public void CancelMute()
        {
            if (mediaPlayer.AudioMuted) mediaPlayerUI.ToggleMute();
        }

        public void SetProgress(double value)
        {
            if (mediaPlayer.IsValidProgress(value)) mediaPlayer.Control.Seek(value);
        }

        public void SetPlayingRate(float rate)
        {
            if (mediaPlayer.IsValidPlayRate(rate)) mediaPlayer.PlaybackRate = rate;
        }

        public void RestorePlayingRate()
        {
            mediaPlayer.PlaybackRate = 1;
        }

        //TODO 使用循环模式，取消多个API
        public void SetPlayingRate125()
        {
            mediaPlayer.PlaybackRate = 1.25F;
        }

        public void SetPlayingRate150()
        {
            mediaPlayer.PlaybackRate = 1.5F;
        }

        public void SetPlayingRate175()
        {
            mediaPlayer.PlaybackRate = 1.75F;
        }

        public void SetPlayingRate200()
        {
            mediaPlayer.PlaybackRate = 2;
        }

        public void JumpForwards()
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() + mediaPlayerUI._jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public void JumpBackwards()
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() - mediaPlayerUI._jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public void SetScaleMode(int scaleModeIndex)
        {
            throw new NotImplementedException();
        }

        public void SetScaleMode(ScaleMode newScaleMode)
        {
            throw new NotImplementedException();
        }

        public void ToggleScaleMode()
        {
            throw new NotImplementedException();
        }

        public void JumpForwards(double jumpDeltaTime)
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() + jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public void JumpBackwards(double jumpDeltaTime)
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() - jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public Texture2D ExtractFrame()
        {
            return mediaPlayer.ExtractFrame(null);
        }

        public void ExtractFrameAsync(MediaPlayer.ProcessExtractedFrame callBack)
        {
            mediaPlayer.ExtractFrameAsync(null, callBack);
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

        public void SetMultimediaList(MediaList multimediaList)
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

        public void SetMultimediaListAndPlay(MediaList multimediaList, bool autoPlay, int index)
        {
            SetMultimediaList(multimediaList);
            if (autoPlay) PlayIndex(index);
        }

        public void TogglePlayMode()
        {
            var playModes = (MediaListPlayingMode[])Enum.GetValues(typeof(MediaListPlayingMode));
            var currentIndex = Array.IndexOf(playModes, playingMode);
            var nextIndex = currentIndex + 1 > playModes.Length - 1 ? 0 : currentIndex + 1;
            playingMode = playModes[nextIndex];
        }

        public void SetPlayMode(MediaListPlayingMode mode)
        {
            playingMode = mode;
        }

        public void SetPlayMode(int playingModeIndex)
        {
            playingMode = (MediaListPlayingMode)playingModeIndex;
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