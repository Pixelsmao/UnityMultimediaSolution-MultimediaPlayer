using Pixelsmao.UnityCommonSolution.UnityEditorExtensions;
using UnityEditor;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [CanEditMultipleObjects, CustomEditor(typeof(MultimediaStandardPlayer))]
    public class MultimediaStandardPlayerEditor : InspectorEditor<MultimediaStandardPlayer>
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

        [MenuItem("GameObject/Video/Multimedia Standard Player", false)]
        public static void InstantiateMultimediaPlayer()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.layer != 5)
            {
                var canvas = HierarchyUtility.GetOrCreateCanvas();
                var player = Instantiate(Resources.Load<GameObject>("Multimedia Standard Player"), canvas.transform);
                Selection.SetActiveObjectWithContext(player, player);
            }
            else
            {
                var player = Instantiate(Resources.Load<GameObject>("Multimedia Standard Player"),
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