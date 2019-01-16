namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public static class HierarchyProEditorIcons
    {
        private static IconPair checkmarkOff;
        private static IconPair checkmarkOn;
        private static IconPair checkmarkPartial;
        private static Texture cog;
        private static Texture delete;
        private static IconPair dropHighlight;
        private static Texture error;
        private static Texture favorite;
        private static Texture folderClosed;
        private static Texture folderOpen;
        private static Texture group;
        private static IconPair helpbox;
        private static Texture info;
        private static Texture labelNumber;
        private static IconPair lockedOff;
        private static IconPair lockedOn;
        private static IconPair minus;
        private static Texture note;
        private static IconPair pin;
        private static IconPair plus;
        private static Texture prefab;
        private static Texture scriptCS;
        private static Texture select;
        private static IconPair toolbarHorizontal;
        private static Texture warning;

        public static IconPair CheckmarkOff { get { return HierarchyProEditorIcons.checkmarkOff; } }
        public static IconPair CheckmarkOn { get { return HierarchyProEditorIcons.checkmarkOn; } }
        public static IconPair CheckmarkPartial { get { return HierarchyProEditorIcons.checkmarkPartial; } }
        public static Texture Cog { get { return HierarchyProEditorIcons.cog; } }
        public static Texture Delete { get { return HierarchyProEditorIcons.delete; } }
        public static IconPair DropHighlight { get { return HierarchyProEditorIcons.dropHighlight; } }
        public static Texture Error { get { return HierarchyProEditorIcons.error; } }
        public static Texture Favorite { get { return HierarchyProEditorIcons.favorite; } }
        public static Texture FolderClosed { get { return HierarchyProEditorIcons.folderClosed; } }
        public static Texture FolderOpen { get { return HierarchyProEditorIcons.folderOpen; } }
        public static Texture Group { get { return HierarchyProEditorIcons.group; } }
        public static IconPair Helpbox { get { return HierarchyProEditorIcons.helpbox; } }
        public static Texture Info { get { return HierarchyProEditorIcons.info; } }
        public static Texture LabelNumber { get { return HierarchyProEditorIcons.labelNumber; } }
        public static IconPair LockedOff { get { return HierarchyProEditorIcons.lockedOff; } }
        public static IconPair LockedOn { get { return HierarchyProEditorIcons.lockedOn; } }
        public static IconPair Minus { get { return HierarchyProEditorIcons.minus; } }
        public static Texture Note { get { return HierarchyProEditorIcons.note; } }
        public static IconPair Pin { get { return HierarchyProEditorIcons.pin; } }
        public static IconPair Plus { get { return HierarchyProEditorIcons.plus; } }
        public static Texture Prefab { get { return HierarchyProEditorIcons.prefab; } }
        public static Texture ScriptCS { get { return HierarchyProEditorIcons.scriptCS; } }
        public static Texture Select { get { return HierarchyProEditorIcons.select; } }
        public static IconPair ToolbarHorizontal { get { return HierarchyProEditorIcons.toolbarHorizontal; } }
        public static Texture Warning { get { return HierarchyProEditorIcons.warning; } }

        public static Texture GetComponentIcon<T>()
        {
            return HierarchyProEditorIcons.GetComponentIcon(typeof(T));
        }

        public static Texture GetComponentIcon(Type type)
        {
            Texture icon = EditorGUIUtility.ObjectContent(null, type).image;
            return icon ?? HierarchyProEditorIcons.ScriptCS;
        }

        public static void Load()
        {
            HierarchyProEditorIcons.select = HierarchyProEditorIcons.LoadCustomIcon("Select");
            HierarchyProEditorIcons.note = HierarchyProEditorIcons.LoadCustomIcon("Note");
            HierarchyProEditorIcons.delete = HierarchyProEditorIcons.LoadCustomIcon("Delete");
            HierarchyProEditorIcons.info = HierarchyProEditorIcons.LoadCustomIcon("Info");
            HierarchyProEditorIcons.warning = HierarchyProEditorIcons.LoadCustomIcon("Warning");
            HierarchyProEditorIcons.error = HierarchyProEditorIcons.LoadCustomIcon("Error");

            HierarchyProEditorIcons.cog = HierarchyProEditorIcons.LoadBuiltIn("d_SettingsIcon");
            HierarchyProEditorIcons.prefab = HierarchyProEditorIcons.LoadBuiltIn("PrefabNormal Icon");
            HierarchyProEditorIcons.scriptCS = HierarchyProEditorIcons.LoadBuiltIn("cs Script Icon");
            HierarchyProEditorIcons.folderClosed = HierarchyProEditorIcons.LoadBuiltIn("Folder Icon");
            HierarchyProEditorIcons.folderOpen = HierarchyProEditorIcons.LoadBuiltIn("FolderEmpty Icon");
            HierarchyProEditorIcons.group = HierarchyProEditorIcons.LoadBuiltIn("d_VerticalLayoutGroup Icon");
            HierarchyProEditorIcons.favorite = HierarchyProEditorIcons.LoadBuiltIn("Favorite Icon");

            HierarchyProEditorIcons.checkmarkOff = IconPair.LoadBuiltIn("toggle");
            HierarchyProEditorIcons.checkmarkPartial = IconPair.LoadBuiltIn("toggle mixed");
            HierarchyProEditorIcons.checkmarkOn = IconPair.LoadBuiltIn("toggle on");
            HierarchyProEditorIcons.lockedOff = IconPair.LoadBuiltIn("IN LockButton");
            HierarchyProEditorIcons.lockedOn = IconPair.LoadBuiltIn("IN LockButton on");
            HierarchyProEditorIcons.pin = IconPair.LoadBuiltIn("eventpin on");
            HierarchyProEditorIcons.plus = IconPair.LoadBuiltIn("ShurikenPlus");
            HierarchyProEditorIcons.minus = IconPair.LoadBuiltIn("ShurikenMinus");
            HierarchyProEditorIcons.helpbox = IconPair.LoadBuiltIn("PingBox");
            HierarchyProEditorIcons.toolbarHorizontal = IconPair.LoadBuiltIn("Toolbar");
            HierarchyProEditorIcons.dropHighlight = IconPair.LoadBuiltIn("PR DropHere");

            HierarchyProEditorIcons.labelNumber = HierarchyProEditorIcons.LoadCustomIcon("NumberPanel");
        }

        public static Texture LoadBuiltIn(string name)
        {
            return EditorGUIUtility.LoadRequired(name) as Texture;
        }

        public static Texture LoadCustomIcon(string icon)
        {
            //string assetPath = Path.Combine(HierarchyProEditorStyles.PathEditorResources, "HierarchyPro-" + icon + ".png");
            //return (Texture) EditorGUIUtility.Load(assetPath);
            return Resources.Load<Texture>("HierarchyPro-" + icon);
        }

        public struct IconPair
        {
            private readonly Texture2D personal;
            private readonly Texture2D pro;

            public IconPair(Texture2D personal, Texture2D pro)
            {
                this.personal = personal;
                this.pro = pro;
            }

            public static implicit operator Texture2D(IconPair iconPair)
            {
                return iconPair.Current;
            }

            public Texture2D Current { get { return EditorGUIUtility.isProSkin ? this.Pro : this.Personal; } }

            public Texture2D Personal { get { return this.personal; } }

            public Texture2D Pro { get { return this.pro; } }

            public static IconPair LoadBuiltIn(string name)
            {
                Texture2D personal = EditorGUIUtility.Load(string.Format("Builtin Skins/LightSkin/Images/{0}.png", name)) as Texture2D;
                Texture2D pro = EditorGUIUtility.Load(string.Format("Builtin Skins/DarkSkin/Images/{0}.png", name)) as Texture2D;
                if (personal == null)
                {
                    personal = pro;
                }
                else if (pro == null)
                {
                    pro = personal;
                }
                return new IconPair(personal, pro);
            }
        }
    }
}
