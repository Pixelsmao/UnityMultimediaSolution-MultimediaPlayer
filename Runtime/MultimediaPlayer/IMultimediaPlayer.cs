using System.IO;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public interface IMultimediaPlayer
    {
        /// <summary>
        /// 打开媒体
        /// </summary>
        /// <param name="mediaPath">媒体路径</param>
        /// <param name="autoPlay">打开时自动播放</param>
        public void OpenMedia(string mediaPath, bool autoPlay);

        /// <summary>
        /// 打开媒体
        /// </summary>
        /// <param name="mediaReferenceInfo">媒体引用信息</param>
        /// <param name="autoPlay">打开时自动播放</param>
        public void OpenMedia(MediaReferenceInfo mediaReferenceInfo, bool autoPlay);

        /// <summary>
        /// 打开媒体
        /// </summary>
        /// <param name="mediaReference">媒体引用</param>
        /// <param name="autoPlay">打开时自动播放</param>
        public void OpenMedia(MediaReference mediaReference, bool autoPlay);

        /// <summary>
        /// 打开媒体
        /// </summary>
        /// <param name="mediaInfo">媒体文件系统信息</param>
        /// <param name="autoPlay">打开时自动播放</param>
        public void OpenMedia(FileSystemInfo mediaInfo, bool autoPlay);

        /// <summary>
        /// 打开媒体
        /// </summary>
        /// <param name="mediaInfo">媒体文件信息</param>
        /// <param name="autoPlay">打开时自动播放</param>
        public void OpenMedia(FileInfo mediaInfo, bool autoPlay);

        /// <summary>
        /// 继续播放
        /// </summary>
        public void Play();

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause();

        /// <summary>
        /// 切换播放暂停
        /// </summary>
        public void TogglePlayPause();

        /// <summary>
        /// 停止播放：将视频在指定进度停止
        /// </summary>
        /// <param name="progress">停止的进度</param>
        public void Stop(double progress = 0);

        /// <summary>
        /// 完全停止：从播放器中关闭媒体
        /// </summary>
        public void StopCompletely();

        /// <summary>
        /// 重新开始播放
        /// </summary>
        public void RePlayback();

        /// <summary>
        /// 音量加
        /// </summary>
        /// <param name="delta">增量</param>
        public void AudioVolumeUp(float delta);

        /// <summary>
        /// 音量减
        /// </summary>
        /// <param name="delta">增量</param>
        public void AudioVolumeDown(float delta);

        /// <summary>
        /// 音量加
        /// </summary>
        public void AudioVolumeUp();

        /// <summary>
        /// 音量减
        /// </summary>
        public void AudioVolumeDown();

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="volume">音量</param>
        public void SetAudioVolume(float volume);

        /// <summary>
        /// 切换静音
        /// </summary>
        public void ToggleMute();

        /// <summary>
        /// 设置为静音
        /// </summary>
        public void SetMute();

        /// <summary>
        /// 取消静音
        /// </summary>
        public void CancelMute();

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度：单位(秒)</param>
        public void SetProgress(double value);

        /// <summary>
        /// 设置播放速率：范围[-4,4]
        /// </summary>
        /// <param name="rate">播放速率</param>
        public void SetPlayingRate(float rate);

        /// <summary>
        /// 恢复播放速率
        /// </summary>
        public void RestorePlayingRate();

        /// <summary>
        /// 媒体前进
        /// </summary>
        /// <param name="jumpDeltaTime">前进时间</param>
        public void JumpForwards(double jumpDeltaTime);

        /// <summary>
        /// 媒体后退
        /// </summary>
        /// <param name="jumpDeltaTime">后退时间</param>
        public void JumpBackwards(double jumpDeltaTime);

        /// <summary>
        /// 媒体前进
        /// </summary>
        public void JumpForwards();

        /// <summary>
        /// 媒体后退
        /// </summary>
        public void JumpBackwards();

        /// <summary>
        /// 设置缩放模式
        /// </summary>
        /// <param name="scaleModeIndex">缩放模式索引</param>
        public void SetScaleMode(int scaleModeIndex);

        /// <summary>
        /// 设置缩放模式
        /// </summary>
        /// <param name="newScaleMode">缩放模式</param>
        public void SetScaleMode(ScaleMode newScaleMode);

        /// <summary>
        /// 切换缩放模式
        /// </summary>
        public void ToggleScaleMode();

        /// <summary>
        /// 提取帧
        /// </summary>
        public Texture2D ExtractFrame();

        /// <summary>
        /// 异步提取帧
        /// </summary>
        public void ExtractFrameAsync(MediaPlayer.ProcessExtractedFrame callBack);

        /// <summary>
        /// 尝试获取当前媒体引用信息：目标媒体是通过MediaReferenceInfo包装进行播放时返回True，否则返回False
        /// </summary>
        public bool TryGetMediaReferenceInfo(out MediaReferenceInfo mediaReferenceInfo);

        /// <summary>
        /// 尝试获取当前媒体引用：目标媒体是通过MediaReference、MediaPath或者绝对路径播放时返回True，否则返回False
        /// </summary>
        public bool TryGetMediaReference(out MediaReference mediaReferenceInfo);

        /// <summary>
        /// 播放媒体章节：仅当媒体通过MediaReferenceInfo包装进行播放时有效
        /// </summary>
        /// <param name="sectionIndex">媒体章节索引</param>
        public bool PlayMediaSection(int sectionIndex);
    }
}