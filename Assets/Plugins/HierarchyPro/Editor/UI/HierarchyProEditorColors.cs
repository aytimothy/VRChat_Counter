namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditorColors
    {
        private static ColorSet blue;
        private static ColorSet green;
        private static ColorSet indigo;
        private static ColorSet orange;
        private static ColorSet red;
        private static ColorSet violet;
        private static ColorSet yellow;

        public static Color Background { get { return EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255); } }

        public static ColorSet Blue { get { return HierarchyProEditorColors.blue; } }
        public static ColorSet Green { get { return HierarchyProEditorColors.green; } }
        public static ColorSet Indigo { get { return HierarchyProEditorColors.indigo; } }
        public static ColorSet Orange { get { return HierarchyProEditorColors.orange; } }
        public static ColorSet Red { get { return HierarchyProEditorColors.red; } }
        public static ColorSet Violet { get { return HierarchyProEditorColors.violet; } }
        public static ColorSet Yellow { get { return HierarchyProEditorColors.yellow; } }

        public static void Load()
        {
            HierarchyProEditorColors.red = new ColorSet(new Color(1, 0, 0));
            HierarchyProEditorColors.orange = new ColorSet(new Color(1, 0.5f, 0));
            HierarchyProEditorColors.yellow = new ColorSet(new Color(1, 1, 0));
            HierarchyProEditorColors.green = new ColorSet(new Color(0, 1, 0));
            HierarchyProEditorColors.blue = new ColorSet(new Color(0, 0, 1));
            HierarchyProEditorColors.indigo = new ColorSet(new Color(0.5f, 0, 1));
            HierarchyProEditorColors.violet = new ColorSet(new Color(1, 0, 1));
        }

        public struct ColorSet
        {
            private readonly Color main;
            private readonly Color pastel;
            private readonly Color faint;

            public ColorSet(Color color)
            {
                this.main = color;

                float h, s, v;
                HierarchyProColorHelpers.RGBToHSV(color, out h, out s, out v);

                this.pastel = HierarchyProColorHelpers.HSVToRGB(h, 0.25f, 1);
                this.faint = HierarchyProColorHelpers.HSVToRGB(h, 0.5f, 1);
            }

            public static implicit operator Color(ColorSet colorSet)
            {
                return colorSet.Main;
            }

            public Color Main { get { return this.main; } }
            public Color Pastel { get { return this.pastel; } }
            public Color Faint { get { return this.faint; } }
        }
    }
}
