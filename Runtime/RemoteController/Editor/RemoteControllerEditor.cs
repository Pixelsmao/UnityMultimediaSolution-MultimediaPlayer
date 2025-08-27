using System;
using System.IO;
using Pixelsmao.UnityCommonSolution.UnityEditorExtensions;
using UnityEditor;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [CanEditMultipleObjects, CustomEditor(typeof(RemoteController))]
    public class RemoteControllerEditor : InspectorEditor<RemoteController>
    {
        private const string helpMessage = "1. 要使用远程控制功能，在你的远程通讯框架的接收部分使用此脚本的【ExecuteCommand】方法。" + "\n2. 远程控制码见【RemoteControlCommand】类。";

        private SerializedProperty executeAllWithinOneFrame;

        private AnimationCollapseGroup _helpGroup;

        protected override void OnEnable()
        {
            base.OnEnable();
            executeAllWithinOneFrame = serializedObject.FindProperty(nameof(executeAllWithinOneFrame));
            _helpGroup = new AnimationCollapseGroup("Help", false, false, OnInspectorGUI_Help, this, Color.green);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(executeAllWithinOneFrame);
            _helpGroup.Show();
            ApplyModifiedProperties();
        }

        private void OnInspectorGUI_Help()
        {
            EditorGUILayout.HelpBox(helpMessage, MessageType.Info);
            if (GUILayout.Button("Save RemoteControlCommand To Desktop"))
            {
                var desktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "RemoteControlCommand.txt");
                File.WriteAllText(desktop, RemoteControlCommand.ToString());
            }
        }
    }
}