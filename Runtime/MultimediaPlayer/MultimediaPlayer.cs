using System;
using System.IO;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [DisallowMultipleComponent, RequireComponent(typeof(AudioOutput), typeof(MediaPlayer))]
    public abstract class MultimediaPlayer : MonoBehaviour, IMultimediaPlayer
    {
       [Tooltip("缩放模式：0:拉伸填充 1:保持宽高比填充 2:保持宽高比缩放填充")] [SerializeField]
        private ScaleMode scaleMode = ScaleMode.ScaleToFit;

        [Tooltip("进度修改量")] public float progressModifyDelta = 5f;
        [Tooltip("音量修改量")] public float volumeModifyDelta = 0.2f;

        private MediaPlayer _mediaPlayer;
        private DisplayUGUI _displayUGUI;
        private AudioOutput _audioOutput;
        private MediaReferenceInfo _mediaReferenceInfo;
        private MediaReference _mediaReference;
        protected MediaPlayer mediaPlayer => _mediaPlayer ??= GetComponentInChildren<MediaPlayer>();
        protected DisplayUGUI displayUGUI => _displayUGUI ??= GetComponentInChildren<DisplayUGUI>();
        protected AudioOutput audioOutput => _audioOutput ??= GetComponentInChildren<AudioOutput>();

        protected virtual void Start()
        {
            _mediaPlayer = GetComponentInChildren<MediaPlayer>();
            _displayUGUI = GetComponentInChildren<DisplayUGUI>();
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

        public virtual void OpenMedia(MediaReferenceInfo referenceInfo, bool autoPlay)
        {
            this._mediaReferenceInfo = referenceInfo;
            mediaPlayer.OpenMedia(_mediaReferenceInfo.mediaReference, autoPlay);
            UpdateMediaSections();
        }

        public virtual void OpenMedia(MediaReference reference, bool autoPlay)
        {
            this._mediaReference = reference;
            mediaPlayer.OpenMedia(_mediaReference, autoPlay);
            UpdateMediaSections();
        }

        public virtual void OpenMedia(FileSystemInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            _mediaReference = ScriptableObject.CreateInstance<MediaReference>();
            _mediaReference.MediaPath = mediaPath;
            mediaPlayer.OpenMedia(_mediaReference, autoPlay);
            UpdateMediaSections();
        }

        public virtual void OpenMedia(FileInfo mediaInfo, bool autoPlay)
        {
            var mediaPath = new MediaPath(mediaInfo.FullName, MediaPathType.AbsolutePathOrURL);
            _mediaReference = ScriptableObject.CreateInstance<MediaReference>();
            _mediaReference.MediaPath = mediaPath;
            mediaPlayer.OpenMedia(_mediaReference, autoPlay);
            UpdateMediaSections();
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
            mediaPlayer.AudioVolume += volumeModifyDelta;
        }

        public virtual void AudioVolumeDown()
        {
            mediaPlayer.AudioVolume -= volumeModifyDelta;
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
            var targetProgress = mediaPlayer.Control.GetCurrentTime() + progressModifyDelta;
            if (mediaPlayer.IsValidProgress(targetProgress)) SetProgress(targetProgress);
        }

        public virtual void JumpBackwards()
        {
            var targetProgress = mediaPlayer.Control.GetCurrentTime() - progressModifyDelta;
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

        public Texture2D ExtractFrame()
        {
            return mediaPlayer.ExtractFrame(null);
        }

        public void ExtractFrameAsync(MediaPlayer.ProcessExtractedFrame callBack)
        {
            mediaPlayer.ExtractFrameAsync(null, callBack);
        }

        public bool TryGetMediaReferenceInfo(out MediaReferenceInfo referenceInfo)
        {
            referenceInfo = _mediaReferenceInfo;
            return referenceInfo != null;
        }

        public bool TryGetMediaReference(out MediaReference reference)
        {
            reference = _mediaReference;
            return _mediaReference != null;
        }

        public bool PlayMediaSection(int sectionIndex)
        {
            if (_mediaReferenceInfo == null) return false;
            if (!_mediaReferenceInfo.TryGetMediaSection(sectionIndex, out var mediaSection)) return false;
            if (mediaPlayer.IsValidProgress(mediaSection.SectionLocation))
            {
                SetProgress(mediaSection.SectionLocation);
                return true;
            }

            Debug.LogWarning($"播放的指定媒体章节{mediaSection.SectionName}的位置{mediaSection.SectionLocation}无效！");
            return false;
        }

        /// <summary>
        /// 更新媒体章节选择器
        /// </summary>
        protected abstract void UpdateMediaSections();
    }
}