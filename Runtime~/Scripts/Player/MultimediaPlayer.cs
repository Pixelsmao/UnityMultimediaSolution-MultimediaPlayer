using System;
using System.IO;
using Pixelsmao.UnityCommonSolution.Extensions;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [DisallowMultipleComponent]
    public abstract class MultimediaPlayer : MonoBehaviour, IMediaPlayer
    {
        [Tooltip("进度修改量")] public float progressAmendDelta = 5f;
        [Tooltip("音量修改量")] public float volumeAmendDelta = 0.15f;
        [Tooltip("缩放模式：0:拉伸填充 1:保持宽高比填充 2:保持宽高比缩放填充")]
        public ScaleMode scaleMode = ScaleMode.ScaleToFit;

        protected MediaPlayer mediaPlayer;
        protected DisplayUGUI displayUGUI;

        private void Reset()
        {
            InstantiateMultimediaPlayer(transform);
        }

        public static GameObject InstantiateMultimediaPlayer(Transform parent)
        {
            var multimediaPlayerGameObject = new GameObject("Multimedia Player");
            var rectTransform = multimediaPlayerGameObject.GetOrAddComponent<RectTransform>();
            var iMultimediaPlayer = multimediaPlayerGameObject.AddComponent<MultimediaPlayer>();
            multimediaPlayerGameObject.transform.SetParent(parent);
            var audioOutput = multimediaPlayerGameObject.GetOrAddComponent<AudioOutput>();
            var tempMediaPlayer = multimediaPlayerGameObject.GetOrAddComponent<MediaPlayer>();
            audioOutput.SetAudioSource(multimediaPlayerGameObject.GetComponentInChildren<AudioSource>());
            audioOutput.Player = tempMediaPlayer;
            var tempDisplayUGUI = multimediaPlayerGameObject.GetComponentInChildren<DisplayUGUI>();
            if (tempDisplayUGUI == null)
            {
                var rendererObj = new GameObject("Renderer");
                var rendererObjRectTransform = rendererObj.AddComponent<RectTransform>();
                rendererObjRectTransform.SetParent(multimediaPlayerGameObject.transform);
                rendererObjRectTransform.anchorMax = Vector2.one;
                rendererObjRectTransform.anchorMin = Vector2.zero;
                rendererObjRectTransform.pivot = Vector2.one * 0.5f;
                rendererObjRectTransform.offsetMax = Vector2.zero;
                rendererObjRectTransform.offsetMin = Vector2.zero;

                var background = new GameObject("Background");
                var backgroundRectTransform = background.AddComponent<RectTransform>();
                backgroundRectTransform.SetParent(rendererObj.transform);
                backgroundRectTransform.anchorMax = Vector2.one;
                backgroundRectTransform.anchorMin = Vector2.zero;
                backgroundRectTransform.pivot = Vector2.one * 0.5f;
                backgroundRectTransform.offsetMax = Vector2.zero;
                backgroundRectTransform.offsetMin = Vector2.zero;
                var image = background.AddComponent<Image>();
                image.color = new Color(0.1686f, 0.1686f, 0.1686f, 1);

                var tempDisplayUGUIObj = new GameObject("Display UGUI");
                var tempDisplayRectTransform = tempDisplayUGUIObj.AddComponent<RectTransform>();
                tempDisplayRectTransform.SetParent(rendererObj.transform);
                tempDisplayRectTransform.anchorMax = Vector2.one;
                tempDisplayRectTransform.anchorMin = Vector2.zero;
                tempDisplayRectTransform.pivot = Vector2.one * 0.5f;
                tempDisplayRectTransform.offsetMax = Vector2.zero;
                tempDisplayRectTransform.offsetMin = Vector2.zero;
                tempDisplayUGUI = tempDisplayUGUIObj.AddComponent<DisplayUGUI>();
            }

            tempDisplayUGUI.Player = tempMediaPlayer;
            tempDisplayUGUI.ScaleMode = ScaleMode.ScaleToFit;
            return multimediaPlayerGameObject;
        }

        protected virtual void Awake()
        {
            mediaPlayer = GetComponentInChildren<MediaPlayer>();
            displayUGUI = GetComponentInChildren<DisplayUGUI>();
            displayUGUI.ScaleMode = scaleMode;
        }

        public virtual void OpenMedia(string mediaPath, bool autoPlay)
        {
            var mediaFileInfo = new FileInfo(mediaPath);
            if (mediaFileInfo.Exists)
            {
                Debug.LogWarning($"指定的媒体文件【{mediaPath}】不存在！");
                return;
            }

            OpenMedia(mediaFileInfo, autoPlay);
        }

        public virtual void OpenMedia(MediaReferenceInfo mediaReferenceInfo, bool autoPlay)
        {
            mediaPlayer.OpenMedia(mediaReferenceInfo.mediaReference, autoPlay);
        }

        public virtual void OpenMedia(MediaReference mediaReference, bool autoPlay)
        {
            mediaPlayer.OpenMedia(mediaReference, autoPlay);
        }

        public virtual void OpenMedia(FileSystemInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            mediaPlayer.OpenMedia(mediaPath, autoPlay);
        }

        public virtual void OpenMedia(FileInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            mediaPlayer.OpenMedia(mediaPath, autoPlay);
        }

        public virtual void Play()
        {
            mediaPlayer.Play();
        }

        public virtual void Pause()
        {
            mediaPlayer.Pause();
        }

        public virtual void TogglePlayPause()
        {
            if (mediaPlayer.Control.IsPlaying()) mediaPlayer.Pause();
            else mediaPlayer.Play();
        }

        public virtual void Stop(double progress = 0)
        {
            SetProgress(progress);
            Pause();
        }

        public virtual void StopCompletely()
        {
            mediaPlayer.CloseMedia();
        }

        public virtual void RePlayback()
        {
            SetProgress(0);
            Play();
        }

        public virtual void AudioVolumeUp(float delta)
        {
            mediaPlayer.AudioVolume += delta;
        }

        public virtual void AudioVolumeDown(float delta)
        {
            mediaPlayer.AudioVolume -= delta;
        }

        public virtual void AudioVolumeUp()
        {
            mediaPlayer.AudioVolume += volumeAmendDelta;
        }

        public virtual void AudioVolumeDown()
        {
            mediaPlayer.AudioVolume -= volumeAmendDelta;
        }

        public virtual void SetAudioVolume(float volume)
        {
            mediaPlayer.AudioVolume = volume;
        }

        public virtual void ToggleMute()
        {
            mediaPlayer.AudioMuted = !mediaPlayer.AudioMuted;
        }

        public virtual void SetMute()
        {
            mediaPlayer.AudioMuted = true;
        }

        public virtual void CancelMute()
        {
            mediaPlayer.AudioMuted = false;
        }

        public virtual void SetProgress(double value)
        {
            if (mediaPlayer.IsValidProgress(value)) mediaPlayer.Control.Seek(value);
        }

        public virtual void SetPlayingRate(float rate)
        {
            if (mediaPlayer.IsValidPlayRate(rate)) mediaPlayer.PlaybackRate = rate;
        }

        public virtual void RestorePlayingRate()
        {
            mediaPlayer.PlaybackRate = 1;
        }

        public virtual void JumpForwards(double jumpDeltaTime)
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() + jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public virtual void JumpBackwards(double jumpDeltaTime)
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() - jumpDeltaTime;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public virtual void JumpForwards()
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() + progressAmendDelta;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public virtual void JumpBackwards()
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() - progressAmendDelta;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public void SetScaleMode(int scaleModeIndex)
        {
            displayUGUI.ScaleMode = (ScaleMode)scaleModeIndex;
        }

        public void SetScaleMode(ScaleMode newScaleMode)
        {
            displayUGUI.ScaleMode = newScaleMode;
        }

        public void ToggleScaleMode()
        {
            var scaleModes = (ScaleMode[])Enum.GetValues(typeof(ScaleMode));
            var currentIndex = Array.IndexOf(scaleModes, displayUGUI.ScaleMode);
            var nextIndex = currentIndex + 1 > scaleModes.Length - 1 ? 0 : currentIndex + 1;
            displayUGUI.ScaleMode = scaleModes[nextIndex];
        }
    }
}