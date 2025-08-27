using System.Collections;
using System.Collections.Generic;
using Pixelsmao.UnityCommonSolution.UnityEditorExtensions;
using Pixelsmao.UnityMultimediaSolution.MultimediaPlayer;
using UnityEditor;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [CanEditMultipleObjects, CustomEditor(typeof(MultimediaListPlayer))]
    public class MultimediaListPlayerEditor : InspectorEditor<MultimediaListPlayer>
    {
        private SerializedProperty scaleMode;
        private SerializedProperty progressModifyDelta;
        private SerializedProperty volumeModifyDelta;
        private SerializedProperty enableControlBar;
        private SerializedProperty mediaSelectorScale;
        private SerializedProperty animDuration;
        private SerializedProperty themeColor;
        private SerializedProperty uiState;
        private SerializedProperty mediaSectionsTrack;
        private SerializedProperty mediaSectionPrefabs;
        private SerializedProperty notificationText;
        private SerializedProperty fullScreenButton;
        private SerializedProperty fullScreenImage;
        private SerializedProperty fullScreenTexture2D;
        private SerializedProperty exitFullScreenTexture2D;
        private SerializedProperty controlBarRectTransform;

        private SerializedProperty autoOpenMedia;
        private SerializedProperty autoPlayMedia;
        private SerializedProperty currentListIndex;
        private SerializedProperty playingMode;
        private SerializedProperty completedDeviation;
        private SerializedProperty onCompleted;
        private SerializedProperty multimediaLists;

        private AnimationCollapseGroup pluginGroup;

        protected override void OnEnable()
        {
            base.OnEnable();
            scaleMode = serializedObject.FindProperty("scaleMode");
            progressModifyDelta = serializedObject.FindProperty(nameof(progressModifyDelta));
            volumeModifyDelta = serializedObject.FindProperty(nameof(volumeModifyDelta));

            enableControlBar = serializedObject.FindProperty(nameof(enableControlBar));
            mediaSelectorScale = serializedObject.FindProperty(nameof(mediaSelectorScale));
            animDuration = serializedObject.FindProperty(nameof(animDuration));
            themeColor = serializedObject.FindProperty(nameof(themeColor));
            uiState = serializedObject.FindProperty(nameof(uiState));
            mediaSectionsTrack = serializedObject.FindProperty(nameof(mediaSectionsTrack));
            mediaSectionPrefabs = serializedObject.FindProperty(nameof(mediaSectionPrefabs));
            notificationText = serializedObject.FindProperty(nameof(notificationText));
            fullScreenButton = serializedObject.FindProperty(nameof(fullScreenButton));
            fullScreenImage = serializedObject.FindProperty(nameof(fullScreenImage));
            fullScreenTexture2D = serializedObject.FindProperty(nameof(fullScreenTexture2D));
            exitFullScreenTexture2D = serializedObject.FindProperty(nameof(exitFullScreenTexture2D));
            controlBarRectTransform = serializedObject.FindProperty(nameof(controlBarRectTransform));
            autoOpenMedia = serializedObject.FindProperty(nameof(autoOpenMedia));
            autoPlayMedia = serializedObject.FindProperty(nameof(autoPlayMedia));
            currentListIndex = serializedObject.FindProperty(nameof(currentListIndex));
            completedDeviation = serializedObject.FindProperty(nameof(completedDeviation));
            playingMode = serializedObject.FindProperty(nameof(playingMode));
            onCompleted = serializedObject.FindProperty(nameof(onCompleted));
            multimediaLists = serializedObject.FindProperty(nameof(multimediaLists));
            pluginGroup = new AnimationCollapseGroup("Plugins", false, false, OnInspectorGUI_Plugins, this, Color.cyan);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            EditorGUILayout.PropertyField(scaleMode);
            EditorGUILayout.PropertyField(progressModifyDelta);
            EditorGUILayout.PropertyField(volumeModifyDelta);
            EditorGUILayout.PropertyField(enableControlBar);
            EditorGUILayout.PropertyField(mediaSelectorScale);
            EditorGUILayout.PropertyField(animDuration);
            EditorGUILayout.PropertyField(themeColor);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(uiState);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(autoOpenMedia);
            EditorGUILayout.PropertyField(autoPlayMedia);
            EditorGUILayout.PropertyField(playingMode);
            EditorGUILayout.PropertyField(completedDeviation);
            EditorGUILayout.PropertyField(onCompleted);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(currentListIndex);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(multimediaLists);
            EditorGUILayout.PropertyField(mediaSectionsTrack);
            EditorGUILayout.PropertyField(mediaSectionPrefabs);
            EditorGUILayout.PropertyField(notificationText);
            EditorGUILayout.PropertyField(fullScreenButton);
            EditorGUILayout.PropertyField(fullScreenImage);
            EditorGUILayout.PropertyField(fullScreenTexture2D);
            EditorGUILayout.PropertyField(exitFullScreenTexture2D);
            EditorGUILayout.PropertyField(controlBarRectTransform);
            pluginGroup.Show();
            ApplyModifiedProperties();
        }

        [MenuItem("GameObject/Video/Multimedia List Player", false)]
        public static void InstantiateMultimediaPlayer()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.layer != 5)
            {
                var canvas = HierarchyUtility.GetOrCreateCanvas();
                var player = Instantiate(Resources.Load<GameObject>("Multimedia List Player"), canvas.transform);
                Selection.SetActiveObjectWithContext(player, player);
            }
            else
            {
                var player = Instantiate(Resources.Load<GameObject>("Multimedia List Player"),
                    Selection.activeTransform);
                Selection.SetActiveObjectWithContext(player, player);
            }
        }

        private void OnInspectorGUI_Plugins()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUI.enabled = !self.TryGetComponent<RemoteController>(out _);
                    if (GUILayout.Button("Remote Controller", GUILayout.Height(25)))
                    {
                        self.gameObject.AddComponent<RemoteController>();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GUI.enabled = true;
        }
    }
}