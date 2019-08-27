using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    public static class JEMExtensionRectTransform
    {
        /// <summary>
        ///     Tests if mouse have collision with given rectTransform.
        /// </summary>
        public static void TestRectTransformCollision(this RectTransform t, out bool isCollision)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(t, Input.mousePosition, t.GetComponentInParent<Canvas>().worldCamera, out var point);
            var p = Rect.PointToNormalized(t.rect, point);
            var pos = new Vector2(t.rect.width * p.x, t.rect.height * p.y);

            isCollision = false;
            if (pos.x > 1f && pos.y > 1f)
            {
                if (pos.x <= t.rect.width - 1f && pos.y <= t.rect.height - 1f)
                {
                    isCollision = true;
                }
            }
        }
    }
}
