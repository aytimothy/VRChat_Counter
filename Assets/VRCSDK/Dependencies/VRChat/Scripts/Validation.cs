using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace VRCSDK2
{
    public static class Validation
    {
        public static IEnumerable<Component> FindIllegalComponents(GameObject target, System.Type[] whitelist)
        {
            List<Component> bad = new List<Component>();
            IEnumerator seeker = FindIllegalComponentsEnumerator(target, whitelist, (c) => bad.Add(c));
            while (seeker.MoveNext()) ;
            return bad;
        }

        public static void RemoveIllegalComponents(GameObject target, System.Type[] whitelist, bool retry = true, bool onlySceneObjects = false, bool logStripping = true)
        {
            bool foundBad = false;
            IEnumerator remover = FindIllegalComponentsEnumerator(target, whitelist, (c) => {
                if (c != null)
                {
                    if(onlySceneObjects && c.GetInstanceID() < 0)
                        return;

                    if(logStripping)
                        Debug.LogWarningFormat("Removed component of type {0} found on {1}", c.GetType().Name, c.gameObject.name);

                    RemoveComponent(c);

                    foundBad = true;
                }
            }, false);
            while (remover.MoveNext());

            if (retry && foundBad)
                RemoveIllegalComponents(target, whitelist, false, onlySceneObjects);
        }

        public static IEnumerator RemoveIllegalComponentsEnumerator(GameObject target, System.Type[] whitelist, bool retry = true, bool onlySceneObjects = false)
        {
            bool foundBad = false;
            yield return FindIllegalComponentsEnumerator(target, whitelist, (c) => {
                if (c != null)
                {
                    if(onlySceneObjects && c.GetInstanceID() < 0)
                        return;

                    Debug.LogWarningFormat("Removed component of type {0} found on {1}", c.GetType().Name, c.gameObject.name);

                    RemoveComponent(c);

                    foundBad = true;
                }
            });

            if (retry && foundBad)
                yield return RemoveIllegalComponentsEnumerator(target, whitelist, false, onlySceneObjects);
        }

        public static IEnumerator FindIllegalComponentsEnumerator(GameObject target, System.Type[] whitelist, System.Action<Component> onFound, bool useWatch = true)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            if(useWatch)
                watch.Start();

            HashSet<System.Type> typesInUse = new HashSet<System.Type>();
            List<Component> componentsInUse = new List<Component>();
            Queue<GameObject> children = new Queue<GameObject>();
            children.Enqueue(target.gameObject);
            while (children.Count > 0)
            {
                GameObject child = children.Dequeue();
                if(child == null)
                    continue;
                
                int childCount = child.transform.childCount;
                for (int idx = 0; idx < child.transform.childCount; ++idx)
                    children.Enqueue(child.transform.GetChild(idx).gameObject);
                foreach (Component c in child.transform.GetComponents<Component>())
                {
                    if (c == null)
                        continue;

                    if (typesInUse.Contains(c.GetType()) == false)
                        typesInUse.Add(c.GetType());

                    if (!whitelist.Any(allowedType => c.GetType() == allowedType || c.GetType().IsSubclassOf(allowedType)))
                    {
                        onFound(c);
                        yield return null;
                    }

                    if (useWatch && watch.ElapsedMilliseconds > 1)
                    {
                        yield return null;
                        watch.Reset();
                    }
                }
            }
        }

        private static Dictionary<string, System.Type> _typeCache = new Dictionary<string, System.Type>();
        private static Dictionary<string, System.Type[]> _whitelistCache = new Dictionary<string, System.Type[]>();
        public static System.Type[] WhitelistedTypes(string whitelistName, string[] ComponentTypeWhitelist)
        {
            if (_whitelistCache.ContainsKey(whitelistName))
                return _whitelistCache[whitelistName];

            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            _whitelistCache[whitelistName] = ComponentTypeWhitelist.Select((name) => GetTypeFromName(name, assemblies)).Where(t => t != null).ToArray();

            return _whitelistCache[whitelistName];
        }

        public static System.Type GetTypeFromName(string name, Assembly[] assemblies = null)
        {
            if(_typeCache.ContainsKey(name))
                return _typeCache[name];

            if(assemblies == null)
                assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly a in assemblies)
            {
                System.Type found = a.GetType(name);
                if (found != null)
                {
                    _typeCache[name] = found;
                    return found;
                }
            }

            //This is really verbose for some SDK scenes, eg.
            //If they don't have FinalIK installed
#if VRC_CLIENT && UNITY_EDITOR
                Debug.LogError("Could not find type " + name);
#endif
            _typeCache[name] = null;
            return null;
        }

        public static void RemoveDependancies(Component component)
        {
            if (component == null)
                return;

            Component[] components = component.GetComponents<Component>();
            if (components == null || components.Length == 0)
                return;

            System.Type compType = component.GetType();
            foreach (var c in components)
            {
                if (c == null)
                    continue;

                bool deleteMe = false;
                object[] requires = c.GetType().GetCustomAttributes(typeof(RequireComponent), true);
                if (requires == null || requires.Length == 0)
                    continue;

                foreach (var r in requires)
                {
                    RequireComponent rc = r as RequireComponent;
                    if (rc == null)
                        continue;

                    if (rc.m_Type0 == compType ||
                        rc.m_Type1 == compType ||
                        rc.m_Type2 == compType)
                    {
                        deleteMe = true;
                        break;
                    }
                }

                if (deleteMe)
                {
                    Debug.LogWarningFormat("Deleting component dependency {0} found on {1}", c.GetType().Name, component.gameObject.name);

                    RemoveComponent(c);
                }
            }
        }

        public static void RemoveComponent(Component comp)
        {
            RemoveDependancies(comp);

#if VRC_CLIENT
            Object.DestroyImmediate(comp, true);
#else
            Object.DestroyImmediate(comp, false);
#endif
        }

        public static void RemoveComponentsOfType<T>(GameObject target) where T : Component
        {
            if (target == null)
                return;

            foreach (T comp in target.GetComponentsInChildren<T>(true))
            {
                if (comp == null || comp.gameObject == null)
                    continue;

                Debug.LogWarningFormat("Removing {0} comp from {1}", comp.GetType().Name, comp.gameObject.name);

                RemoveComponent(comp);
            }
        }
    }
}
