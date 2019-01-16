namespace UntitledGames.Hierarchy
{
    using UnityEngine;

    public static class HierarchyProColorExtensions
    {
        public static Color HighContrast(this Color color)
        {
            float h, s, v;
            HierarchyProColorHelpers.RGBToHSV(color, out h, out s, out v);
            return v > 0.5f ? Color.black : Color.white;
        }
    }
}
