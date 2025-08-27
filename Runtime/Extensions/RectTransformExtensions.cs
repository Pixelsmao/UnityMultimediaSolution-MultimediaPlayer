using Pixelsmao.UnityCommonSolution.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public static class RectTransformExtensions
    {
        
        public static Vector2 GetPivotScreenPosition(this RectTransform rectTransform, TextAnchor anchor)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var width = corners[2].x - corners[0].x; // topRight - bottomLeft
            var height = corners[2].y - corners[0].y;

            var pivotPos = Vector2.zero;

            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    pivotPos = new Vector2(corners[0].x, corners[1].y);
                    break;
                case TextAnchor.UpperCenter:
                    pivotPos = new Vector2(corners[0].x + width * 0.5f, corners[1].y);
                    break;
                case TextAnchor.UpperRight:
                    pivotPos = new Vector2(corners[2].x, corners[3].y);
                    break;

                case TextAnchor.MiddleLeft:
                    pivotPos = new Vector2(corners[0].x, corners[0].y + height * 0.5f);
                    break;
                case TextAnchor.MiddleCenter:
                    pivotPos = new Vector2(corners[0].x + width * 0.5f, corners[0].y + height * 0.5f);
                    break;
                case TextAnchor.MiddleRight:
                    pivotPos = new Vector2(corners[2].x, corners[0].y + height * 0.5f);
                    break;

                case TextAnchor.LowerLeft:
                    pivotPos = new Vector2(corners[0].x, corners[0].y);
                    break;
                case TextAnchor.LowerCenter:
                    pivotPos = new Vector2(corners[0].x + width * 0.5f, corners[0].y);
                    break;
                case TextAnchor.LowerRight:
                    pivotPos = new Vector2(corners[2].x, corners[3].y);
                    break;
            }

            Vector2 screenPos = Camera.main.WorldToScreenPoint(pivotPos);
            return screenPos;
        }
    }
}