namespace UntitledGames.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public static class HierarchyProEditorStyles
    {
        private static GUIStyle dropHighlight;
        private static GUIStyle helpbox;
        private static GUIStyle labelLarge;
        private static GUIStyle labelLargeCentered;
        private static GUIStyle labelNumber;
        private static GUIStyle labelNumberBackground;
        private static GUIStyle labelTiny;
        private static GUIStyle labelTinyCentered;
        private static GUIStyle preferencesTitle;
        private static GUIStyle richTextArea;
        private static GUIStyle toolbarHorizontal;

        public static GUIStyle DropHighlight { get { return HierarchyProEditorStyles.dropHighlight; } }
        public static GUIStyle Helpbox { get { return HierarchyProEditorStyles.helpbox; } }
        public static GUIStyle LabelLarge { get { return HierarchyProEditorStyles.labelLarge; } }
        public static GUIStyle LabelLargeCentered { get { return HierarchyProEditorStyles.labelLargeCentered; } }
        public static GUIStyle LabelNumber { get { return HierarchyProEditorStyles.labelNumber; } }
        public static GUIStyle LabelNumberBackground { get { return HierarchyProEditorStyles.labelNumberBackground; } }
        public static GUIStyle LabelTiny { get { return HierarchyProEditorStyles.labelTiny; } }
        public static GUIStyle LabelTinyCentered { get { return HierarchyProEditorStyles.labelTinyCentered; } }
        public static GUIStyle PreferencesTitle { get { return HierarchyProEditorStyles.preferencesTitle; } }

        public static GUIStyle RichTextArea
        {
            get
            {
                if (HierarchyProEditorStyles.richTextArea != null)
                {
                    return HierarchyProEditorStyles.richTextArea;
                }

                HierarchyProEditorStyles.richTextArea = new GUIStyle(HierarchyProEditorStyles.GetStyle("textField"))
                                                        {
                                                            fontSize = 14,
                                                            fontStyle = FontStyle.Bold,
                                                            wordWrap = true,
                                                            richText = true,
                                                            normal = {background = null},
                                                            active = {background = null},
                                                            focused = {background = null},
                                                            hover = {background = null}
                                                        };
                return HierarchyProEditorStyles.richTextArea;
            }
        }

        public static GUIStyle ToolbarHorizontal { get { return HierarchyProEditorStyles.toolbarHorizontal; } }

        public static void Load()
        {
            HierarchyProEditorStyles.toolbarHorizontal = new GUIStyle
                                                         {
                                                             normal =
                                                             {
                                                                 background = HierarchyProEditorIcons.ToolbarHorizontal
                                                             }
                                                         };

            HierarchyProEditorStyles.labelTiny = new GUIStyle
                                                 {
                                                     normal =
                                                     {
                                                         textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black
                                                     },
                                                     fontSize = 8,
                                                     alignment = TextAnchor.MiddleLeft
                                                 };
            HierarchyProEditorStyles.labelTinyCentered = new GUIStyle(HierarchyProEditorStyles.labelTiny) {alignment = TextAnchor.MiddleCenter};

            HierarchyProEditorStyles.labelLarge = new GUIStyle
                                                  {
                                                      normal =
                                                      {
                                                          textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black
                                                      },
                                                      fontSize = 26,
                                                      fontStyle = FontStyle.Normal,
                                                      alignment = TextAnchor.MiddleLeft
                                                  };
            HierarchyProEditorStyles.labelLargeCentered = new GUIStyle(HierarchyProEditorStyles.labelLarge) {alignment = TextAnchor.MiddleCenter};

            HierarchyProEditorStyles.helpbox = new GUIStyle
                                               {
                                                   normal =
                                                   {
                                                       background = HierarchyProEditorIcons.Helpbox
                                                   },
                                                   padding = new RectOffset(0, 0, 0, 0),
                                                   margin = new RectOffset(0, 0, 0, 0),
                                                   border = new RectOffset(8, 8, 8, 8)
                                               };

            HierarchyProEditorStyles.dropHighlight = new GUIStyle
                                                     {
                                                         normal =
                                                         {
                                                             background = HierarchyProEditorIcons.DropHighlight
                                                         },
                                                         padding = new RectOffset(0, 0, 0, 0),
                                                         margin = new RectOffset(0, 0, 0, 0),
                                                         border = new RectOffset(8, 8, 8, 8)
                                                     };

            HierarchyProEditorStyles.preferencesTitle = new GUIStyle
                                                        {
                                                            normal = {textColor = Color.white},
                                                            fontSize = 16,
                                                            fontStyle = FontStyle.Bold,
                                                            padding = new RectOffset(0, 0, 12, 0),
                                                            margin = new RectOffset(0, 0, 0, 0),
                                                            border = new RectOffset(0, 0, 0, 0)
                                                        };

            HierarchyProEditorStyles.labelNumber = new GUIStyle
                                                   {
                                                       normal = {textColor = Color.white},
                                                       fontSize = 8,
                                                       fontStyle = FontStyle.Bold,
                                                       padding = new RectOffset(0, 0, 0, 0),
                                                       margin = new RectOffset(0, 0, 0, 0),
                                                       border = new RectOffset(0, 0, 0, 0),
                                                       fixedHeight = 0,
                                                       stretchHeight = false,
                                                       fixedWidth = 0,
                                                       stretchWidth = false,
                                                       alignment = TextAnchor.UpperRight
                                                   };

            Texture2D labelTexture = HierarchyProEditorIcons.LabelNumber as Texture2D;
            HierarchyProEditorStyles.labelNumberBackground = new GUIStyle
                                                             {
                                                                 normal = {background = labelTexture},
                                                                 active = {background = labelTexture},
                                                                 hover = {background = labelTexture},
                                                                 focused = {background = labelTexture},
                                                                 fontSize = 6,
                                                                 padding = new RectOffset(0, 0, 0, 0),
                                                                 margin = new RectOffset(0, 0, 0, 0),
                                                                 border = new RectOffset(2, 2, 2, 2),
                                                                 fixedHeight = 0,
                                                                 stretchHeight = false,
                                                                 fixedWidth = 0,
                                                                 stretchWidth = false,
                                                                 alignment = TextAnchor.MiddleCenter
                                                             };
        }

        private static GUIStyle GetStyle(string styleName)
        {
            GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
            if (guiStyle != null)
            {
                return guiStyle;
            }

            Debug.LogError("Missing built-in guistyle " + styleName);
            return null;
        }
    }
}
