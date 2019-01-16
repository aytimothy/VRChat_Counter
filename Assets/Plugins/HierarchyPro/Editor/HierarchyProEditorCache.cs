namespace UntitledGames.Hierarchy
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class HierarchyProEditorCache
    {
        private static Dictionary<GameObject, HierarchyProEditor> lines;

        public static void Draw(GameObject gameObject, Rect rect)
        {
            HierarchyProEditor line;

            if (HierarchyProEditorCache.lines.ContainsKey(gameObject))
            {
                line = HierarchyProEditorCache.lines[gameObject];
                line.Draw(rect);
                return;
            }

            line = new HierarchyProEditor(gameObject);
            HierarchyProEditorCache.lines.Add(gameObject, line);
            line.Draw(rect);
        }

        public static void Initialize()
        {
            HierarchyProEditorCache.lines = new Dictionary<GameObject, HierarchyProEditor>();
        }
    }
}
