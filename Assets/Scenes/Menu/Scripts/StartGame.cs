using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Button))]
public class StartGame : MonoBehaviour
{
    [Delayed]
    [SerializeField] string sceneName = "";

#if UNITY_EDITOR
    void OnValidate()
    {
        // Check if the scene exists in build settings.
        if(sceneName.Length > 0)
        {
            bool foundScene = false;
            for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(
                    SceneUtility.GetScenePathByBuildIndex(i)
                );

                if(this.sceneName.ToLower() == sceneName.ToLower())
                {
                    foundScene = true;
                    break;
                }
            }
            if (!foundScene)
            {
                EditorUtility.DisplayDialog(
                    "Cannot find scene", 
                    $"Cannot find scene \"{sceneName}\"\nPlease make sure the name is correct or add the scene to build settings", 
                    "Ok"
                );
                sceneName = "";
            }
        }
    }
#endif

    public void StartGameCallback()
    {
        SceneManager.LoadScene(sceneName);
    }
}
