namespace UntitledGames.Hierarchy
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class HierarchyProEditorStaticFlags : HierarchyProEditorModuleBase
    {
        /// <inheritdoc />
        public HierarchyProEditorStaticFlags(HierarchyProEditor editor)
            : base(editor)
        {
        }

        public StaticEditorFlags StaticFlags { get { return GameObjectUtility.GetStaticEditorFlags(this.GameObject); } set { GameObjectUtility.SetStaticEditorFlags(this.GameObject, value); } }

        private static void ApplyRecursive(Transform transform, StaticEditorFlags staticFlags)
        {
            GameObjectUtility.SetStaticEditorFlags(transform.gameObject, staticFlags);

            foreach (Transform child in transform)
            {
                HierarchyProEditorStaticFlags.ApplyRecursive(child, staticFlags);
            }
        }

        public override void Draw(Rect rect)
        {
            if (GUI.Button(rect, GUIContent.none, EditorStyles.label))
            {
                if (Event.current.shift)
                {
                    if (EditorUtility.DisplayDialog("Apply to children?", "Do you want to apply these flags to all child objects?", "Yes", "No"))
                    {
                        HierarchyProEditorStaticFlags.ApplyRecursive(this.Transform, this.StaticFlags);
                    }
                }
                else
                {
                    this.ShowMenu();
                }
            }

            Rect iconRect;
            Color disconnectedColor, disconnectedTint = new Color(0.3f, 0.3f, 0.3f, 0.1f);
            Color activeColor = !this.GameObject.activeInHierarchy && this.GameObject.activeSelf ? Color.gray : Color.white;

            Rect left = new Rect(rect) {width = rect.width / 7, height = (rect.height - 4) / 2, y = rect.y + 2};
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.LightmapStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Red.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y += left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.BatchingStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Orange.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y -= left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.NavigationStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Yellow.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y += left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.OccludeeStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Green.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y -= left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.OccluderStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Blue.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y += left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.OffMeshLinkGeneration) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Indigo.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            left.x += left.width;
            left.y -= left.height;
            disconnectedColor = (this.StaticFlags & StaticEditorFlags.ReflectionProbeStatic) > 0 ? Color.white : disconnectedTint;
            iconRect = left.GetCenteredIconRect(HierarchyProEditorIcons.Pin, false);
            GUI.color = HierarchyProEditorColors.Violet.Pastel * activeColor * disconnectedColor;
            GUI.DrawTexture(iconRect, HierarchyProEditorIcons.Pin);

            GUI.color = Color.white;
        }

        public void ShowMenu()
        {
            GenericMenu staticFlagMenu = new GenericMenu();
            staticFlagMenu.AddItem(new GUIContent("Nothing"), (int) this.StaticFlags == 0, this.FlagsChanged, 0);
            staticFlagMenu.AddItem(new GUIContent("Everything"), (int) this.StaticFlags == -1, this.FlagsChanged, -1);
            staticFlagMenu.AddSeparator("");
            foreach (object staticEditorFlagObject in Enum.GetValues(typeof(StaticEditorFlags)))
            {
                StaticEditorFlags staticEditorFlag = (StaticEditorFlags) staticEditorFlagObject;
                staticFlagMenu.AddItem(new GUIContent(staticEditorFlag.ToString()), (this.StaticFlags & staticEditorFlag) > 0, this.FlagsChanged, staticEditorFlag);
            }
            staticFlagMenu.ShowAsContext();
        }

        /// <inheritdoc />
        public override void Update()
        {
        }

        private void FlagsChanged(object userdata)
        {
            int flagsInt = (int) userdata;
            if ((flagsInt == 0) || (flagsInt == -1))
            {
                this.StaticFlags = (StaticEditorFlags) flagsInt;
                return;
            }

            this.StaticFlags ^= (StaticEditorFlags) flagsInt;
        }
    }
}
