using System.Collections.Generic;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public interface IMediaListPlayerUI 
    {
        /// <summary>
        /// 禁用所有UI元素
        /// </summary>
        public void DisableUI();

        /// <summary>
        /// 最大化播放窗口
        /// </summary>
        public void Maximize();

        /// <summary>
        /// 还原播放窗口
        /// </summary>
        /// <param name="continuePlay">是否继续播放</param>
        public void Normal(bool continuePlay);

        /// <summary>
        /// 最小化播放窗口
        /// </summary>
        /// <param name="pause">最小化时是否暂停播放</param>
        public void Minimize(bool pause);

        /// <summary>
        /// 关闭播放器：使用Fade动画淡出后关闭播放器对象，若要立即关闭播放器请使用CloseImmediately()
        /// </summary>
        public void Close();

        /// <summary>
        /// 立即关闭播放器：立即禁用播放器对象
        /// </summary>
        public void CloseImmediately();

        /// <summary>
        /// 隐藏控制栏
        /// </summary>
        public void HideControlBar();

        /// <summary>
        /// 显示控制栏
        /// </summary>
        public void ShowControlBar();

        /// <summary>
        /// 启用控制栏
        /// </summary>
        public void EnableControlBar();

        /// <summary>
        /// 禁用控制栏
        /// </summary>
        public void DisableControlBar();

        /// <summary>
        /// 切换字幕显示
        /// </summary>
        public void ToggleSubtitlesDisplay();

        /// <summary>
        /// 启用字幕
        /// </summary>
        public void EnableSubtitles();

        /// <summary>
        /// 禁用字幕
        /// </summary>
        public void DisableSubtitles();

        /// <summary>
        /// 切换控制栏显示
        /// </summary>
        public void ToggleControlBarDisplay();

        /// <summary>
        /// 切换提示图形显示
        /// </summary>
        public void ToggleOverlayDisplay();

        /// <summary>
        /// 启用提示图标
        /// </summary>
        public void EnableOverlay();

        /// <summary>
        /// 禁用提示图标
        /// </summary>
        public void DisableOverlay();

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="text"></param>
        public void SendNotification(string text);

        /// <summary>
        /// 更新媒体分段
        /// </summary>
        /// <param name="mediaSegments">媒体分段列表</param>
        /// <param name="player">播放器</param>
        public void UpdateMediaSegments(List<MediaSegment> mediaSegments, IMediaListPlayer player);
    }
}