using Pixelsmao.UnityCommonSolution.Extensions;
using Pixelsmao.UnityCommonSolution.UnityEditorExtensions;
using RenderHeads.Media.AVProVideo;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [CanEditMultipleObjects, CustomEditor(typeof(MultimediaSimplePlayer))]
    public class SimpleMultimediaPlayerEditor : InspectorEditor<MultimediaSimplePlayer>
    {
        private SerializedProperty scaleMode;
        private SerializedProperty progressModifyDelta;
        private SerializedProperty volumeModifyDelta;
        private AnimationCollapseGroup pluginGroup;

        protected override void OnEnable()
        {
            base.OnEnable();
            scaleMode = serializedObject.FindProperty("scaleMode");
            progressModifyDelta = serializedObject.FindProperty(nameof(progressModifyDelta));
            volumeModifyDelta = serializedObject.FindProperty(nameof(volumeModifyDelta));
            pluginGroup = new AnimationCollapseGroup("Plugins", false, false, OnInspectorGUI_Plugins, this, Color.cyan);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(scaleMode);
            EditorGUILayout.PropertyField(progressModifyDelta);
            EditorGUILayout.PropertyField(volumeModifyDelta);
            pluginGroup.Show();
            ApplyModifiedProperties();
        }

        private void OnInspectorGUI_Plugins()
        {
           
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUI.enabled = !self.TryGetComponent<RemoteController>(out _);
                    if (GUILayout.Button("Remote Controller",GUILayout.Height(25)))
                    {
                        self.gameObject.AddComponent<RemoteController>();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GUI.enabled = true;
        }

        [MenuItem("GameObject/Video/Multimedia Simple Player", false)]
        public static void InstantiateMultimediaPlayer()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.layer != 5)
            {
                var canvas = HierarchyUtility.GetOrCreateCanvas();
                var player = InstantiateSimpleMultimediaPlayer(canvas.transform);
                Selection.SetActiveObjectWithContext(player, player);
            }
            else
            {
                var player = InstantiateSimpleMultimediaPlayer(Selection.activeTransform);
                Selection.SetActiveObjectWithContext(player, player);
            }
        }

        private static GameObject InstantiateSimpleMultimediaPlayer(Transform parent)
        {
            var multimediaPlayerGameObject = new GameObject("Multimedia Simple Player");
            var rectTransform = multimediaPlayerGameObject.GetOrAddComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(192, 108);
            multimediaPlayerGameObject.transform.SetParent(parent);
            var audioOutput = multimediaPlayerGameObject.GetOrAddComponent<AudioOutput>();
            var tempMediaPlayer = multimediaPlayerGameObject.GetOrAddComponent<MediaPlayer>();
            audioOutput.SetAudioSource(multimediaPlayerGameObject.GetComponentInChildren<AudioSource>());
            audioOutput.Player = tempMediaPlayer;
            var tempDisplayUGUI = multimediaPlayerGameObject.GetComponentInChildren<DisplayUGUI>();
            if (tempDisplayUGUI == null)
            {
                var rendererObj = new GameObject("Renderer");
                var rendererObjRectTransform = rendererObj.AddComponent<RectTransform>();
                rendererObjRectTransform.SetParent(multimediaPlayerGameObject.transform);
                rendererObjRectTransform.anchorMax = Vector2.one;
                rendererObjRectTransform.anchorMin = Vector2.zero;
                rendererObjRectTransform.pivot = Vector2.one * 0.5f;
                rendererObjRectTransform.offsetMax = Vector2.zero;
                rendererObjRectTransform.offsetMin = Vector2.zero;

                var background = new GameObject("Background");
                var backgroundRectTransform = background.AddComponent<RectTransform>();
                backgroundRectTransform.SetParent(rendererObj.transform);
                backgroundRectTransform.anchorMax = Vector2.one;
                backgroundRectTransform.anchorMin = Vector2.zero;
                backgroundRectTransform.pivot = Vector2.one * 0.5f;
                backgroundRectTransform.offsetMax = Vector2.zero;
                backgroundRectTransform.offsetMin = Vector2.zero;
                var image = background.AddComponent<Image>();
                image.color = new Color(0.1686f, 0.1686f, 0.1686f, 1);

                var tempDisplayUGUIObj = new GameObject("Display UGUI");
                var tempDisplayRectTransform = tempDisplayUGUIObj.AddComponent<RectTransform>();
                tempDisplayRectTransform.SetParent(rendererObj.transform);
                tempDisplayRectTransform.anchorMax = Vector2.one;
                tempDisplayRectTransform.anchorMin = Vector2.zero;
                tempDisplayRectTransform.pivot = Vector2.one * 0.5f;
                tempDisplayRectTransform.offsetMax = Vector2.zero;
                tempDisplayRectTransform.offsetMin = Vector2.zero;
                tempDisplayUGUI = tempDisplayUGUIObj.AddComponent<DisplayUGUI>();
            }

            multimediaPlayerGameObject.AddComponent<MultimediaSimplePlayer>();
            tempDisplayUGUI.Player = tempMediaPlayer;
            tempDisplayUGUI.ScaleMode = ScaleMode.ScaleToFit;
            return multimediaPlayerGameObject;
        }
    }
}