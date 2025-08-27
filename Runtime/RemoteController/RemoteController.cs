using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [RequireComponent(typeof(IMultimediaPlayer))]
    public class RemoteController : MonoBehaviour
    {
        /// <summary>
        /// 为了避免多线程的资源竞争情况，所以使用并发队列。
        /// </summary>
        private readonly ConcurrentQueue<RemoteCommand> _commandQueue = new ConcurrentQueue<RemoteCommand>();

        private readonly RemoteCommandExecutor _commandExecutor = new RemoteCommandExecutor();

        [Tooltip("是否所有操作均在一帧内执行：请注意，没有验证多个API的组合是否能够顺利在同一帧成功执行，需要自行判断。" +
                 "建议保持False")]
        public bool executeAllWithinOneFrame;

        private IMultimediaPlayer _multimediaPlayer;
        private IMultimediaPlayerUI _multimediaPlayerUI;
        private IMultimediaListPlayer _multimediaListPlayer;
        //private ISplitScreenPlayer splitScreenPlayer;

        private void Start()
        {
            //TODO 找到对应的播放器才执行命令
            _multimediaPlayer = GetComponent<IMultimediaPlayer>();
            _multimediaPlayerUI = GetComponent<IMultimediaPlayerUI>();
            _multimediaListPlayer = GetComponent<IMultimediaListPlayer>();
            InitializeRemoteCommandExecutor();
        }

        private void Update()
        {
            //执行远程控制命令
            if (executeAllWithinOneFrame)
            {
                while (_commandQueue.Count > 0)
                {
                    if (_commandQueue.TryDequeue(out var command)) _commandExecutor.Execute(command);
                }
            }
            else
            {
                if (_commandQueue.TryDequeue(out var command)) _commandExecutor.Execute(command);
            }
        }


        /// <summary>
        /// 添加到命令执行队列中等候执行。
        /// 由于大多网络框架的接收均是在异步多线程中执行，而Unity中的API无法在多线程环境执行。
        /// 所以您的命令将进入命令队列，由主线程从队列中取出并执行。
        /// 默认一帧仅执行一个命令，修改executeAllWithinOneFrame字段以在一帧内执行全部已经加入队列的命令。
        /// 需要注意的是一帧执行多个命令可能无法按照预期响应。
        /// </summary>
        /// <param name="command">控制命令</param>
        public void ExecuteCommand(string command)
        {
            _commandQueue.Enqueue(new RemoteCommand(command));
        }

        private void InitializeRemoteCommandExecutor()
        {
            _commandExecutor.AddExecutor(RemoteControlCommand.TogglePlayPause,
                () => _multimediaPlayer.TogglePlayPause());
            _commandExecutor.AddExecutor(RemoteControlCommand.Play, () => _multimediaPlayer.Play());
            _commandExecutor.AddExecutor(RemoteControlCommand.Pause, () => _multimediaPlayer.Pause());
            _commandExecutor.AddExecutor(RemoteControlCommand.Stop, () => _multimediaPlayer.Stop());
            _commandExecutor.AddExecutor(RemoteControlCommand.StopWithParameter,
                parameter => _multimediaPlayer.Stop((double)parameter));
            _commandExecutor.AddExecutor(RemoteControlCommand.StopCompletely, () => _multimediaPlayer.StopCompletely());
            _commandExecutor.AddExecutor(RemoteControlCommand.RePlayback, () => _multimediaPlayer.RePlayback());
            _commandExecutor.AddExecutor(RemoteControlCommand.AudioVolumeUp, () => _multimediaPlayer.AudioVolumeUp());
            _commandExecutor.AddExecutor(RemoteControlCommand.AudioVolumeDown,
                () => _multimediaPlayer.AudioVolumeDown());
            _commandExecutor.AddExecutor(RemoteControlCommand.SetAudioVolume,
                volume => _multimediaPlayer.SetAudioVolume(Convert.ToSingle(volume)));
            _commandExecutor.AddExecutor(RemoteControlCommand.ToggleMute, () => _multimediaPlayer.ToggleMute());
            _commandExecutor.AddExecutor(RemoteControlCommand.SetMute, () => _multimediaPlayer.SetMute());
            _commandExecutor.AddExecutor(RemoteControlCommand.CancelMute, () => _multimediaPlayer.CancelMute());
            _commandExecutor.AddExecutor(RemoteControlCommand.SetProgress,
                progress => _multimediaPlayer.SetProgress((double)progress));
            _commandExecutor.AddExecutor(RemoteControlCommand.SetPlayingRate,
                rate => _multimediaPlayer.SetPlayingRate(Convert.ToSingle(rate)));
            _commandExecutor.AddExecutor(RemoteControlCommand.RestorePlayingRate,
                () => _multimediaPlayer.RestorePlayingRate());
            _commandExecutor.AddExecutor(RemoteControlCommand.JumpForwards, () => _multimediaPlayer.JumpForwards());
            _commandExecutor.AddExecutor(RemoteControlCommand.JumpBackwards, () => _multimediaPlayer.JumpBackwards());
            _commandExecutor.AddExecutor(RemoteControlCommand.JumpForwardsWithParameter,
                jumpDeltaTime => _multimediaPlayer.JumpForwards((double)jumpDeltaTime));
            _commandExecutor.AddExecutor(RemoteControlCommand.JumpBackwardsWithParameter,
                jumpDeltaTime => _multimediaPlayer.JumpBackwards((double)jumpDeltaTime));
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayIndex,
                index => _multimediaListPlayer.PlayIndex(Convert.ToInt32(index)));
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayListIndex,
                listIndex => _multimediaListPlayer.PlayListIndex(Convert.ToInt32(listIndex)));
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayListIndexMedia,
                (listIndex, index) =>
                    _multimediaListPlayer.PlayListIndexMedia(Convert.ToInt32(listIndex), Convert.ToInt32(index)));
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayNextMedia, () => _multimediaListPlayer.PlayNextMedia());
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayPreviousMedia,
                () => _multimediaListPlayer.PlayPreviousMedia());
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayNextList, () => _multimediaListPlayer.PlayNextList());
            _commandExecutor.AddExecutor(RemoteControlCommand.PlayPreviousList,
                () => _multimediaListPlayer.PlayPreviousList());
            _commandExecutor.AddExecutor(RemoteControlCommand.TogglePlayMode, () => _multimediaListPlayer.TogglePlayMode());
            _commandExecutor.AddExecutor(RemoteControlCommand.SetPlayMode,
                modeIndex => _multimediaListPlayer.SetPlayMode(Convert.ToInt32(modeIndex)));
            _commandExecutor.AddExecutor(RemoteControlCommand.FullScreen, () => _multimediaPlayerUI.Maximize());
            _commandExecutor.AddExecutor(RemoteControlCommand.ExitFullScreen, () => _multimediaPlayerUI.Normal(true));
            _commandExecutor.AddExecutor(RemoteControlCommand.ToggleSubtitlesDisplay,
                () => _multimediaPlayerUI.ToggleSubtitlesDisplay());
            _commandExecutor.AddExecutor(RemoteControlCommand.EnableSubtitles,
                () => _multimediaPlayerUI.EnableSubtitles());
            _commandExecutor.AddExecutor(RemoteControlCommand.DisableSubtitles,
                () => _multimediaPlayerUI.DisableSubtitles());
            _commandExecutor.AddExecutor(RemoteControlCommand.ToggleControlBarDisplay,
                () => _multimediaPlayerUI.ToggleControlBarDisplay());
            _commandExecutor.AddExecutor(RemoteControlCommand.EnableControlBar,
                () => _multimediaPlayerUI.EnableControlBar());
            _commandExecutor.AddExecutor(RemoteControlCommand.DisableControlBar,
                () => _multimediaPlayerUI.DisableControlBar());
            _commandExecutor.AddExecutor(RemoteControlCommand.ToggleOverlayDisplay,
                () => _multimediaPlayerUI.ToggleOverlayDisplay());
            _commandExecutor.AddExecutor(RemoteControlCommand.EnableOverlay, () => _multimediaPlayerUI.EnableOverlay());
            _commandExecutor.AddExecutor(RemoteControlCommand.DisableOverlay, () => _multimediaPlayerUI.DisableOverlay());
        }
    }
}