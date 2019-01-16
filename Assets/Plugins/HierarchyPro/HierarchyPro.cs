namespace UntitledGames.Hierarchy
{
    using UnityEngine;

    public static class HierarchyPro
    {
        private static bool redrawPending;

        public static bool RedrawPending
        {
            get
            {
                bool result = HierarchyPro.redrawPending;
                HierarchyPro.redrawPending = false;
                return result;
            }
        }

        public static HierarchyProShouldDrawItemEventArgs OnShouldDrawItem(GameObject gameObject, Rect rect)
        {
            HierarchyProShouldDrawItemDelegate handler = HierarchyPro.ShouldDrawItem;
            HierarchyProShouldDrawItemEventArgs e = new HierarchyProShouldDrawItemEventArgs(gameObject, rect);
            if (handler != null)
            {
                handler(null, e);
            }
            return e;
        }

        public static void Redraw()
        {
            HierarchyPro.redrawPending = true;
        }

        public static event HierarchyProShouldDrawItemDelegate ShouldDrawItem;
    }
}
