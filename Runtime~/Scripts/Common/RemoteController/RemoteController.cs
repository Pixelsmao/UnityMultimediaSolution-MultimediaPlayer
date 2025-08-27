using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class RemoteController : MonoBehaviour
    {
        /// <summary>
        /// 为了避免多线程的资源竞争情况，所以使用并发队列。
        /// </summary>
        private readonly ConcurrentQueue<RemoteCommand> commandQueue = new ConcurrentQueue<RemoteCommand>();
        private readonly RemoteCommandExecutor commandExecutor = new RemoteCommandExecutor();
        private IMultimediaPlayer multimediaPlayer;
        private IMediaListPlayerUI mediaListPlayerUI;
        private IMediaListPlayer mediaListPlayer;
        private ISplitScreenPlayer splitScreenPlayer;

        private void Start()
        {
            multimediaPlayer = GetComponent<IMultimediaPlayer>();
            mediaListPlayer = GetComponent<IMediaListPlayer>();
            splitScreenPlayer = GetComponent<ISplitScreenPlayer>();
            InitializeRemoteCommandExecutor();
        }

        private void Update()
        {
            //执行远程控制命令
            if (commandQueue.TryDequeue(out var command)) commandExecutor.Execute(command);
        }


        /// <summary>
        /// 添加到命令执行队列中等候执行。
        /// 由于大多网络框架的接收均是在异步多线程中执行，而Unity中的API无法在多线程环境执行。
        /// 所以您的命令将进入命令队列，由主线程从队列中取出并执行。
        /// 并且一帧仅执行一个命令。
        /// </summary>
        /// <param name="command">控制命令</param>
        public void QueueUpExecuteCommand(string command)
        {
            commandQueue.Enqueue(new RemoteCommand(command));
        }

        private void InitializeRemoteCommandExecutor()
        {
            commandExecutor.AddExecutor(RemoteControlCommand.TogglePlayPause, () => multimediaPlayer.TogglePlayPause());
            commandExecutor.AddExecutor(RemoteControlCommand.Play, () => multimediaPlayer.Play());
            commandExecutor.AddExecutor(RemoteControlCommand.Pause, () => multimediaPlayer.Pause());
            commandExecutor.AddExecutor(RemoteControlCommand.Stop, () => multimediaPlayer.Stop());
            commandExecutor.AddExecutor(RemoteControlCommand.StopWithParameter,
                parameter => multimediaPlayer.Stop((double)parameter));
            commandExecutor.AddExecutor(RemoteControlCommand.StopCompletely, () => multimediaPlayer.StopCompletely());
            commandExecutor.AddExecutor(RemoteControlCommand.RePlayback, () => multimediaPlayer.RePlayback());
            commandExecutor.AddExecutor(RemoteControlCommand.AudioVolumeUp, () => multimediaPlayer.AudioVolumeUp());
            commandExecutor.AddExecutor(RemoteControlCommand.AudioVolumeDown, () => multimediaPlayer.AudioVolumeDown());
            commandExecutor.AddExecutor(RemoteControlCommand.SetAudioVolume,
                volume => multimediaPlayer.SetAudioVolume(Convert.ToSingle(volume)));
            commandExecutor.AddExecutor(RemoteControlCommand.ToggleMute, () => multimediaPlayer.ToggleMute());
            commandExecutor.AddExecutor(RemoteControlCommand.SetMute, () => multimediaPlayer.SetMute());
            commandExecutor.AddExecutor(RemoteControlCommand.CancelMute, () => multimediaPlayer.CancelMute());
            commandExecutor.AddExecutor(RemoteControlCommand.SetProgress,
                progress => multimediaPlayer.SetProgress((double)progress));
            commandExecutor.AddExecutor(RemoteControlCommand.SetPlayingRate, rate => multimediaPlayer.SetPlayingRate(Convert.ToSingle(rate)));
            commandExecutor.AddExecutor(RemoteControlCommand.RestorePlayingRate, () => multimediaPlayer.RestorePlayingRate());
            commandExecutor.AddExecutor(RemoteControlCommand.JumpForwards, () => multimediaPlayer.JumpForwards());
            commandExecutor.AddExecutor(RemoteControlCommand.JumpBackwards, () => multimediaPlayer.JumpBackwards());
            commandExecutor.AddExecutor(RemoteControlCommand.JumpForwardsWithParameter,
                jumpDeltaTime => multimediaPlayer.JumpForwards((double)jumpDeltaTime));
            commandExecutor.AddExecutor(RemoteControlCommand.JumpBackwardsWithParameter,
                jumpDeltaTime => multimediaPlayer.JumpBackwards((double)jumpDeltaTime));
            commandExecutor.AddExecutor(RemoteControlCommand.PlayIndex, index => mediaListPlayer.PlayIndex(Convert.ToInt32(index)));
            commandExecutor.AddExecutor(RemoteControlCommand.PlayListIndex,
                listIndex => mediaListPlayer.PlayListIndex(Convert.ToInt32(listIndex)));
            commandExecutor.AddExecutor(RemoteControlCommand.PlayListIndexMedia,
                (listIndex, index) => mediaListPlayer.PlayListIndexMedia(Convert.ToInt32(listIndex), Convert.ToInt32(index)));
            commandExecutor.AddExecutor(RemoteControlCommand.PlayNextMedia, () => mediaListPlayer.PlayNextMedia());
            commandExecutor.AddExecutor(RemoteControlCommand.PlayPreviousMedia,
                () => mediaListPlayer.PlayPreviousMedia());
            commandExecutor.AddExecutor(RemoteControlCommand.PlayNextList, () => mediaListPlayer.PlayNextList());
            commandExecutor.AddExecutor(RemoteControlCommand.PlayPreviousList,
                () => mediaListPlayer.PlayPreviousList());
            commandExecutor.AddExecutor(RemoteControlCommand.TogglePlayMode, () => mediaListPlayer.TogglePlayMode());
            commandExecutor.AddExecutor(RemoteControlCommand.SetPlayMode,
                modeIndex => mediaListPlayer.SetPlayMode(Convert.ToInt32(modeIndex)));
            commandExecutor.AddExecutor(RemoteControlCommand.FullScreen, () => mediaListPlayerUI.Maximize());
            commandExecutor.AddExecutor(RemoteControlCommand.ExitFullScreen, () => mediaListPlayerUI.Normal(true));
            commandExecutor.AddExecutor(RemoteControlCommand.ToggleSubtitlesDisplay,
                () => mediaListPlayerUI.ToggleSubtitlesDisplay());
            commandExecutor.AddExecutor(RemoteControlCommand.EnableSubtitles,
                () => mediaListPlayerUI.EnableSubtitles());
            commandExecutor.AddExecutor(RemoteControlCommand.DisableSubtitles,
                () => mediaListPlayerUI.DisableSubtitles());
            commandExecutor.AddExecutor(RemoteControlCommand.ToggleControlBarDisplay,
                () => mediaListPlayerUI.ToggleControlBarDisplay());
            commandExecutor.AddExecutor(RemoteControlCommand.EnableControlBar,
                () => mediaListPlayerUI.EnableControlBar());
            commandExecutor.AddExecutor(RemoteControlCommand.DisableControlBar,
                () => mediaListPlayerUI.DisableControlBar());
            commandExecutor.AddExecutor(RemoteControlCommand.ToggleOverlayDisplay,
                () => mediaListPlayerUI.ToggleOverlayDisplay());
            commandExecutor.AddExecutor(RemoteControlCommand.EnableOverlay, () => mediaListPlayerUI.EnableOverlay());
            commandExecutor.AddExecutor(RemoteControlCommand.DisableOverlay, () => mediaListPlayerUI.DisableOverlay());
        }
    }
}