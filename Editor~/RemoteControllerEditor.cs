using System;
using UnityEditor;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class RemoteControllerEditor : Editor
    {
        private RemoteController self;

        private void OnEnable()
        {
            self = target as RemoteController;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox(
                $"要使用远程控制功能，需要将【{nameof(RemoteController)}】挂载到任何需要使用远程控制的IPlayer播放器上，" +
                $"然后在任何你的网络框架中的接收部分使用【{nameof(RemoteController)}】的【QueueUpExecuteCommand()】方法执行远程控制。" +
                $"详细的可以控制命令见【{nameof(RemoteControlCommand)}】脚本。",
                MessageType.Warning);
        }
    }
}