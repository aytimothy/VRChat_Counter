namespace UntitledGames.Hierarchy
{
    using UnityEngine;

    public static class HierarchyProRectExtensions
    {
        public static Rect GetCenteredIconRect(this Rect rect, Texture icon, bool changeSize = true)
        {
            if (changeSize)
            {
                return rect.GetCenteredIconRect(icon, rect.width, rect.height);
            }
            return rect.GetCenteredIconRect(icon, icon.width, icon.height);
        }

        public static Rect GetCenteredIconRect(this Rect rect, Texture icon, float maxWidth, float maxHeight)
        {
            if (icon == null)
            {
                return rect;
            }

            float width = icon.width;
            float height = icon.height;
            if ((width > maxWidth) || (height > maxHeight))
            {
                float widthMultiplier = maxWidth / width;
                float heightMultiplier = maxHeight / height;
                float multiplier = Mathf.Min(widthMultiplier, heightMultiplier);
                width *= multiplier;
                height *= multiplier;
            }

            return new Rect((rect.x + (rect.width / 2f)) - (width / 2f),
                            (rect.y + (rect.height / 2f)) - (height / 2f),
                            width,
                            height);
        }
    }
}
