using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRCSDK2
{
    [InitializeOnLoad]
    public class VRC_SdkSplashScreen : EditorWindow
    {

        static VRC_SdkSplashScreen()
        {
            EditorApplication.update -= DoSplashScreen;
            EditorApplication.update += DoSplashScreen;
        }

        private static void DoSplashScreen()
        {
            EditorApplication.update -= DoSplashScreen;
            if (!EditorPrefs.HasKey("VRCSDK_ShowSplashScreen"))
            {
                EditorPrefs.SetBool("VRCSDK_ShowSplashScreen", true);
            }
            if (EditorPrefs.GetBool("VRCSDK_ShowSplashScreen"))
                OpenSplashScreen();
        }

        private static GUIStyle vrcSdkHeader;
        //private static GUIStyle vrcLinkButton;
        private static Vector2 changeLogScroll;
        [MenuItem("VRChat SDK/Splash Screen", false, 500)]
        public static void OpenSplashScreen()
        {
            GetWindow<VRC_SdkSplashScreen>(true);
        }
        
        public static void Open()
        {
            OpenSplashScreen();
        }

        public void OnEnable()
        {


            titleContent = new GUIContent("VRChat SDK");

            maxSize = new Vector2(400, 500);
            minSize = maxSize;

            vrcSdkHeader = new GUIStyle
            {
                normal =
                    {
                        background = Resources.Load("vrcSdkHeader") as Texture2D,
                        textColor = Color.white
                    },
                fixedHeight = 200
            };

            //vrcLinkButton = EditorStyles.miniButton;
            //vrcLinkButton.normal.textColor = new Color(0, 42f/255f,1);
        }

        public void OnGUI()
        {
            GUILayout.Box("", vrcSdkHeader);

            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("SDK Docs"))
            {
                Application.OpenURL("https://docs.vrchat.com/");
            }
            if (GUILayout.Button("VRChat FAQ"))
            {
                Application.OpenURL("https://www.vrchat.net/developerfaq");
            }
            if (GUILayout.Button("Help Center"))
            {
                Application.OpenURL("https://vrchat.groovehq.com/knowledge_base/categories/technical-issues-vrchat-sdk");
            }
            if(GUILayout.Button("Examples"))
            {
                Application.OpenURL("https://docs.vrchat.com/docs/vrchat-kits");
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            changeLogScroll = GUILayout.BeginScrollView(changeLogScroll);
            GUILayout.Label(
    @"Changelog:
2018.2.2
Changes
-New triggers added to 'Example - Triggers - 2.unity' included 
    with the SDK
Fixes
-VRCWorld prefab had the UpdateTimeInMS set to 10ms even 
    though the slider value in inspector is capped to 33ms 
    as minimum value. The default is now set to 33ms.

2018.2.1
- Added Bufferone support for actions where it was missing
   --SetLayer
   --SetWebpanelVolume
   --AddHealth
   --AddDamage
   --SetComponentActive
   --SetMaterial
   --ActivateCustomTrigger

 - Added MIDI Driver(opens any available midi - input)
 - Added OSC Driver(input port 9000)
 - Added VRC_MidiNoteIn component for custom triggering note input
 - Added VRC_OscButtonIn component for custom triggering osc button input
 - Added drag and drop support to the trigger references list

 - Fixed ParticleCollision Trigger
 - Fixed some trigger editor issues
 - Fixed enable / disablekinematic action(objectsync)
 - Fixed enable / disablegravity action(objectsync)"
            );
            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            EditorPrefs.SetBool("VRCSDK_ShowSplashScreen", GUILayout.Toggle(EditorPrefs.GetBool("VRCSDK_ShowSplashScreen"), "Show at Startup"));

            GUILayout.EndHorizontal();
        }

    }
}