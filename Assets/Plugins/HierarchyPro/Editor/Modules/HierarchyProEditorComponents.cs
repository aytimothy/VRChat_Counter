namespace UntitledGames.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class HierarchyProEditorComponents : HierarchyProEditorModuleBase
    {
        private static EditorWindow lightingWindow;
        private Dictionary<Texture, List<KeyValuePair<Component, Behaviour>>> stacks = new Dictionary<Texture, List<KeyValuePair<Component, Behaviour>>>();
        private int maximumComponents;

        /// <inheritdoc />
        public HierarchyProEditorComponents(HierarchyProEditor editor)
            : base(editor)
        {
        }

        public int Count { get { return this.stacks.Count; } }

        public int MaximumComponents { get { return this.maximumComponents; } set { this.maximumComponents = value; } }

        private static EditorWindow GetLightingWindow()
        {
            if (HierarchyProEditorComponents.lightingWindow != null)
            {
                return HierarchyProEditorComponents.lightingWindow;
            }

            EditorWindow resetWindow = EditorWindow.focusedWindow;
            EditorApplication.ExecuteMenuItem("Window/Lighting/Light Explorer");
            HierarchyProEditorComponents.lightingWindow = EditorWindow.focusedWindow;
            resetWindow.Focus();
            return HierarchyProEditorComponents.lightingWindow;
        }

        public override void Draw(Rect rect)
        {
            if (!this.stacks.Any())
            {
                return;
            }

            foreach (KeyValuePair<Texture, List<KeyValuePair<Component, Behaviour>>> stack in this.stacks)
            {
                rect = this.DrawStack(rect, stack);
            }
        }

        [Obsolete("Untested")]
        public int GetCount<T>()
        {
            return this.stacks.SelectMany(x => x.Value).Count(x => x.Key.GetType().IsAssignableFrom(typeof(T)));
        }

        public override void Update()
        {
            foreach (List<KeyValuePair<Component, Behaviour>> stacks in this.stacks.Values)
            {
                stacks.Clear();
            }
            this.stacks.Clear();

            Component[] components = this.GameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is Transform)
                {
                    continue;
                }

                Texture icon = HierarchyProEditorIcons.GetComponentIcon(component.GetType());
                Behaviour behaviour = component as Behaviour;

                List<KeyValuePair<Component, Behaviour>> stack = null;
                stack = this.stacks.ContainsKey(icon) ? this.stacks[icon] : new List<KeyValuePair<Component, Behaviour>>();
                stack.Add(new KeyValuePair<Component, Behaviour>(component, behaviour));
                this.stacks[icon] = stack;
            }
        }

        private void DisableAllScripts(object userdata)
        {
            Behaviour[] behaviours = userdata as Behaviour[];
            if (behaviours == null)
            {
                return;
            }
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }

        private Rect DrawStack(Rect rect, KeyValuePair<Texture, List<KeyValuePair<Component, Behaviour>>> stack)
        {
            Texture icon = stack.Key;
            int stackCount = stack.Value.Count;
            bool stackMultiple = stackCount > 1;
            Component[] components = stack.Value.Select(x => x.Key).ToArray();
            Behaviour[] behaviours = stack.Value.Select(x => x.Value).ToArray();
            bool disabled = behaviours.Any(x => (x != null) && !x.enabled);

            Rect right = new Rect(rect) {xMin = rect.xMax - 16, width = 16};
            GUI.color = disabled ? new Color(1, 1, 1, 0.2f) : Color.white;
            GUI.DrawTexture(right, icon);
            GUI.color = Color.white;

            if (stackMultiple)
            {
                Rect rectNumber = new Rect(right);
                rectNumber.x -= 1;
                GUIContent content = new GUIContent(stackCount.ToString());
                GUI.color = new Color(0, 0, 0, 0.5f);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x - 2}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x + 2}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x - 1, y = rectNumber.y - 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x + 1, y = rectNumber.y - 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x - 1, y = rectNumber.y + 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x + 1, y = rectNumber.y + 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x - 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {x = rectNumber.x + 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {y = rectNumber.y - 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.Label(new Rect(rectNumber) {y = rectNumber.y + 1}, content, HierarchyProEditorStyles.LabelNumber);
                GUI.color = Color.white;
                GUI.Label(rectNumber, content, HierarchyProEditorStyles.LabelNumber);
            }

            if (GUI.Button(right, GUIContent.none, GUIStyle.none))
            {
                if (Event.current.button == 0)
                {
                    if (stackMultiple)
                    {
                        if (Event.current.shift)
                        {
                            if (icon == HierarchyProEditorIcons.ScriptCS)
                            {
                                MonoScript[] scripts = behaviours.Select(x => MonoScript.FromMonoBehaviour(x as MonoBehaviour)).Where(x => x != null).ToArray();
                                if (!scripts.Any())
                                {
                                    HierarchyProEditorWindow.EditorWindow.ShowNotification(new GUIContent("No MonoScripts"));
                                }
                                else
                                {
                                    GenericMenu menu = new GenericMenu();
                                    menu.AddDisabledItem(new GUIContent("Open Script"));
                                    menu.AddSeparator("");
                                    foreach (MonoScript script in scripts)
                                    {
                                        menu.AddItem(new GUIContent(string.Format("{0}.cs", script.GetClass().Name)), false, this.OpenScript, script);
                                    }
                                    menu.ShowAsContext();
                                    Event.current.Use();
                                }
                            }
                        }
                        else
                        {
                            MonoBehaviour[] monoBehaviours = behaviours.OfType<MonoBehaviour>().ToArray();
                            if (!monoBehaviours.Any())
                            {
                                HierarchyProEditorWindow.EditorWindow.ShowNotification(new GUIContent("No Behaviours"));
                            }
                            else
                            {
                                GenericMenu menu = new GenericMenu();
                                menu.AddDisabledItem(new GUIContent("Enable \u2215 Disable"));
                                menu.AddSeparator("");
                                foreach (Behaviour behaviour in behaviours)
                                {
                                    MonoBehaviour monoBehvaiour = behaviour as MonoBehaviour;
                                    if (monoBehvaiour == null)
                                    {
                                        continue;
                                    }
                                    MonoScript script = MonoScript.FromMonoBehaviour(monoBehvaiour);
                                    menu.AddItem(new GUIContent(script.GetClass().Name), behaviour.enabled, this.ToggleScript, behaviour);
                                }
                                menu.AddSeparator("");
                                menu.AddItem(new GUIContent("Enable All"), false, this.EnableAllScripts, behaviours);
                                menu.AddItem(new GUIContent("Disable All"), false, this.DisableAllScripts, behaviours);
                                menu.ShowAsContext();
                                Event.current.Use();
                            }
                        }
                    }
                    else
                    {
                        Component singleComponent = components.First();
                        Behaviour singleBehaviour = behaviours.First();
                        if (Event.current.shift)
                        {
                            if (icon == HierarchyProEditorIcons.ScriptCS)
                            {
                                MonoScript script = MonoScript.FromMonoBehaviour(singleComponent as MonoBehaviour);
                                AssetDatabase.OpenAsset(script, 1);
                            }
                            else if (singleComponent is Light)
                            {
                                EditorWindow lightingWindow = HierarchyProEditorComponents.GetLightingWindow();
                                lightingWindow.Show();
                            }
                        }
                        else
                        {
                            if (singleBehaviour != null)
                            {
                                singleBehaviour.enabled = !singleBehaviour.enabled;
                            }
                        }
                    }
                }
            }

            right.x -= 16;
            return right;
        }

        private void EnableAllScripts(object userdata)
        {
            Behaviour[] behaviours = userdata as Behaviour[];
            if (behaviours == null)
            {
                return;
            }
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.enabled = true;
            }
        }

        private void OpenScript(object userdata)
        {
            MonoScript script = userdata as MonoScript;
            AssetDatabase.OpenAsset(script, 1);
        }

        private void ToggleScript(object userdata)
        {
            Behaviour behaviour = userdata as Behaviour;
            behaviour.enabled = !behaviour.enabled;
        }
    }
}
