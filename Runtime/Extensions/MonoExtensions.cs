using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public static class HierarchyUtility
    {
        public static Canvas GetOrCreateCanvas()
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null) return canvas;
            var canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            var eventSystem = Object.FindObjectOfType<EventSystem>();
            if (eventSystem != null) return canvas;
            eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            return canvas;
        }
    }
}