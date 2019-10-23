using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using CharacterCreator2D;

namespace CharacterEditor2D
{    
    public static class CustomMenu
    {
        [MenuItem("Window/Character Creator 2D/Add New Part", false, 100)]
        public static void CreatePart()
        {
            ScriptableWizard.DisplayWizard<CharacterEditor2D.WizardPart>("Add Part", "Create");
        }
    }
    
    [InitializeOnLoad]
    public class PlayCreatorScene
    {
        static PlayCreatorScene()
        {
#if UNITY_2017_1_OR_NEWER
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
#else
            EditorApplication.playmodeStateChanged += OnPlayModeChanged;
#endif
        }

        static string startScene = "Assets/CharacterCreator2D/Creator UI/Creator UI.unity";
        static string prevScene = EditorPrefs.GetString("PlayFromStartPrevScene");
        static bool active = EditorPrefs.GetBool("PlayFromStartActive", false);

        [MenuItem("Window/Character Creator 2D/Create Character", false, 10)]
        static void Play()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                if (PartList.Static != null)
                {
                    InspectorPartList.RefreshPartPackage();
                    InspectorPartList.Refresh(PartList.Static);
                }
                EditorPrefs.SetString("PlayFromStartPrevScene", EditorSceneManager.GetActiveScene().path);
                EditorSceneManager.OpenScene(startScene);
                EditorPrefs.SetBool("PlayFromStartActive", true);
                EditorApplication.isPlaying = true;
            }
        }

#if UNITY_2017_1_OR_NEWER
        private static void OnPlayModeChanged(PlayModeStateChange stateChange)
        {
            if (!active) return;
            if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.update += RestoreScene;
            }
        }
#else
        private static void OnPlayModeChanged()
        {
            if (!active) return;
            if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.update += RestoreScene;
            }
        }
#endif

        public static void RestoreScene()
        {
            if (EditorApplication.isPlaying) return;
            if (prevScene == null || prevScene == "")
            {
                EditorApplication.update -= RestoreScene;
                return;
            }
            EditorSceneManager.OpenScene(prevScene);
            EditorPrefs.DeleteKey("PlayFromStartPrevScene");
            EditorPrefs.DeleteKey("PlayFromStartActive");
            EditorApplication.update -= RestoreScene;
        }
    }
}