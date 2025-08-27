using System.Reflection;
using System.Text;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    /// <summary>
    /// 解析方案：先根据是否有冒号判断是否携带参数，带两个参数的使用连字符分割，然后根据EndWith判断命令类型
    /// </summary>
    public static class RemoteControlCommand
    {
        #region MultimediaPlayerAPI

        /// <summary>
        /// 切换播放暂停
        /// </summary>
        [Tooltip("切换播放暂停")] public const string TogglePlayPause = "TOGGLE_PLAY_PAUSE";

        /// <summary>
        /// 继续播放
        /// </summary>
        [Tooltip("继续播放")] public const string Play = "PLAY";

        /// <summary>
        /// 暂停
        /// </summary>
        [Tooltip("暂停播放")] public const string Pause = "PAUSE";

        /// <summary>
        /// 停止播放：将视频停止在媒体开始时
        /// </summary>
        [Tooltip("停止播放：将视频停止在媒体开始时")] public const string Stop = "STOP";

        /// <summary>
        /// 带参数的停止播放：将视频在指定进度停止
        /// </summary>
        [Tooltip("带参数的停止播放：将视频在指定进度停止")] public const string StopWithParameter = "STOP_WITH_PARAMETER";

        /// <summary>
        /// 完全停止：从播放器中关闭媒体
        /// </summary>
        [Tooltip("完全停止：从播放器中关闭媒体")] public const string StopCompletely = "STOP_COMPLETELY";

        /// <summary>
        /// 重新开始播放
        /// </summary>
        [Tooltip("重新开始播放")] public const string RePlayback = "REPLAYBACK";

        /// <summary>
        /// 增大音量
        /// </summary>
        [Tooltip("增大音量")] public const string AudioVolumeUp = "AUDIO_VOLUME_UP";

        /// <summary>
        /// 减小音量
        /// </summary>
        [Tooltip("减小音量")] public const string AudioVolumeDown = "AUDIO_VOLUME_DOWN";

        /// <summary>
        /// 设置音量
        /// </summary>
        [Tooltip("设置音量")] public const string SetAudioVolume = "SET_AUDIO_VOLUME";

        /// <summary>
        /// 切换静音
        /// </summary>
        [Tooltip("切换静音")] public const string ToggleMute = "TOGGLE_MUTE";

        /// <summary>
        /// 设置静音
        /// </summary>
        [Tooltip("设置静音")] public const string SetMute = "SET_MUTE";

        /// <summary>
        /// 取消静音
        /// </summary>
        [Tooltip("取消静音")] public const string CancelMute = "CANCEL_MUTE";


        /// <summary>
        /// 设置播放进度：单位(秒)
        /// </summary>
        [Tooltip("设置播放进度：单位(秒)")] public const string SetProgress = "SET_PROGRESS";

        /// <summary>
        /// 设置播放速率：范围[-4,4]
        /// </summary>
        [Tooltip("设置播放速率：范围[-4,4]")] public const string SetPlayingRate = "SET_PLAYING_RATE";

        /// <summary>
        /// 恢复播放速率
        /// </summary>
        [Tooltip("恢复播放速率")] public const string RestorePlayingRate = "RESTORE_PLAYING_RATE";

        /// <summary>
        /// 视频前进
        /// </summary>
        [Tooltip("视频前进")] public const string JumpForwards = "JUMP_FORWARDS";

        /// <summary>
        /// 视频后退
        /// </summary>
        [Tooltip("视频后退")] public const string JumpBackwards = "JUMP_BACKWARDS";

        /// <summary>
        /// 带参数的视频前进
        /// </summary>
        [Tooltip("带参数的视频前进")] public const string JumpForwardsWithParameter = "JUMP_FORWARDS_WITH_PARAMETER";

        /// <summary>
        /// 带参数的视频后退
        /// </summary>
        [Tooltip("带参数的视频后退")] public const string JumpBackwardsWithParameter = "JUMP_BACKWARDS_WITH_PARAMETER";

        #endregion


        #region MultimediaListPlayerAPI

        /// <summary>
        /// 播放索引列表中的索引媒体
        /// </summary>
        [Tooltip("播放索引列表中的索引媒体")] public const string PlayIndex = "PLAY_INDEX";

        /// <summary>
        /// 播放指定索引媒体列表
        /// </summary>
        [Tooltip("播放指定索引媒体列表")] public const string PlayListIndex = "PLAY_LIST_INDEX";

        /// <summary>
        /// 播放索引列表中的索引媒体
        /// </summary>
        [Tooltip("播放索引列表中的索引媒体")] public const string PlayListIndexMedia = "PLAY_LIST_INDEX_MEDIA";

        /// <summary>
        /// 播放下一条媒体
        /// </summary>
        [Tooltip("播放下一条媒体")] public const string PlayNextMedia = "PLAY_NEXT_MEDIA";

        /// <summary>
        /// 播放上一条媒体
        /// </summary>
        [Tooltip("播放上一条媒体")] public const string PlayPreviousMedia = "PLAY_PREVIOUS_MEDIA";


        /// <summary>
        /// 播放下一个媒体列表
        /// </summary>
        [Tooltip("播放下一个媒体列表")] public const string PlayNextList = "PLAY_NEXT_LIST";

        /// <summary>
        /// 播放上一个媒体列表
        /// </summary>
        [Tooltip("播放上一个媒体列表")] public const string PlayPreviousList = "PLAY_PREVIOUS_LIST";

        /// <summary>
        /// 切换播放模式
        /// </summary>
        [Tooltip("切换播放模式")] public const string TogglePlayMode = "TOGGLE_PLAY_MODE";

        /// <summary>
        /// 设置播放模式
        /// </summary>
        [Tooltip("设置播放模式")] public const string SetPlayMode = "SET_PLAY_MODE";

        #endregion

        #region MultimediaListPlayerUI

        /// <summary>
        /// 全屏
        /// </summary>
        [Tooltip("全屏")] public const string FullScreen = "FULL_SCREEN";

        /// <summary>
        /// 退出全屏
        /// </summary>
        [Tooltip("退出全屏")] public const string ExitFullScreen = "EXIT_FULL_SCREEN";

        /// <summary>
        /// 切换字幕显示
        /// </summary>
        [Tooltip("切换字幕显示")] public const string ToggleSubtitlesDisplay = "TOGGLE_SUBTITLES_DISPLAY";

        /// <summary>
        /// 开启字幕
        /// </summary>
        [Tooltip("开启字幕")] public const string EnableSubtitles = "ENABLE_SUBTITLES";

        /// <summary>
        /// 禁用字幕
        /// </summary>
        [Tooltip("禁用字幕")] public const string DisableSubtitles = "DISABLE_SUBTITLES";

        /// <summary>
        /// 切换控制栏显示
        /// </summary>
        [Tooltip("切换字幕显示")] public const string ToggleControlBarDisplay = "TOGGLE_CONTROL_BAR_DISPLAY";

        /// <summary>
        /// 启用控制栏
        /// </summary>
        [Tooltip("启用控制栏")] public const string EnableControlBar = "ENABLE_CONTROL_BAR";

        /// <summary>
        /// 禁用控制栏
        /// </summary>
        [Tooltip("禁用控制栏")] public const string DisableControlBar = "DISABLE_CONTROL_BAR";

        /// <summary>
        /// 切换提示图标显示
        /// </summary>
        [Tooltip("切换提示图标显示")] public const string ToggleOverlayDisplay = "TOGGLE_OVERLAY_DISPLAY";

        /// <summary>
        /// 启用提示图标
        /// </summary>
        [Tooltip("启用提示图标")] public const string EnableOverlay = "ENABLE_OVERLAY";

        /// <summary>
        /// 禁用提示图标
        /// </summary>
        [Tooltip("禁用提示图标")] public const string DisableOverlay = "DISABLE_OVERLAY";

        #endregion

        #region Advanced

        /// <summary>
        /// 2分屏播放
        /// </summary>
        [Tooltip("2分屏播放")] public const string TwoSplitScreen = "TWO_SPLIT_SCREEN";

        /// <summary>
        /// 4分屏播放
        /// </summary>
        [Tooltip("4分屏播放")] public const string FourSplitScreen = "TWO_SPLIT_SCREEN";

        #endregion

        /// <summary>
        /// 将所有命令连同描述转换为字符串以供保存
        /// </summary>
        /// <returns></returns>
        public new static string ToString()
        {
            var builder = new StringBuilder();
            var fields = typeof(RemoteControlCommand).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                builder.AppendLine($"{GetFieldTooltip(field)}：{field.GetValue(field)}");
            }

            return builder.ToString();
        }

        private static string GetFieldTooltip(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true);
            var tooltip = attributes.Length > 0 ? ((TooltipAttribute)attributes[0]).tooltip : string.Empty;
            return string.IsNullOrEmpty(tooltip) ? string.Empty : $"<{tooltip}>";
        }
    }
}