using UnityEngine;

namespace UI
{
    public class SafeAreapr : MonoBehaviour
    {
        public bool dontSafeBottompr;
        RectTransform fittedRectTransformpr;
        Rect safeRectComponentpr;
        Vector2 minAnchorVectorpr;
        Vector2 maxAnchorVectorpr;

        private void Awake()
        {
            fittedRectTransformpr = GetComponent<RectTransform>();
            safeRectComponentpr = Screen.safeArea;
            minAnchorVectorpr = safeRectComponentpr.position;
            maxAnchorVectorpr = minAnchorVectorpr + safeRectComponentpr.size;
        
            minAnchorVectorpr.x /= Screen.width;
            minAnchorVectorpr.y = dontSafeBottompr ? minAnchorVectorpr.y = 0 : minAnchorVectorpr.y /= Screen.height;
            maxAnchorVectorpr.x /= Screen.width;
            maxAnchorVectorpr.y /= Screen.height;
        
            fittedRectTransformpr.anchorMin = minAnchorVectorpr;
            fittedRectTransformpr.anchorMax = maxAnchorVectorpr;

        }
    }
}
