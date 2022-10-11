using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;
using System;

namespace StudioLand
{
    /// <summary>
    /// A messy implementation of scene loading management -- done this way for the sake of time
    /// </summary>
    public class SceneLoadManager : MonoBehaviour
    {
        [SerializeField] InputReaderSO inputReader;
        [SerializeField] float fadeDuration = 3;
        AssetReference currentlyLoadedGameScene;
        
        [Header("Broadcasts on")]
        [SerializeField] FloatEventChannelSO fadeInRequestChannel;
        [SerializeField] FloatEventChannelSO fadeOutRequestChannel;
        [SerializeField] VoidEventChannelSO sceneReadyEventChannel;

        [Header("Listens on")]
        [SerializeField] LoadEventChannelSO requestLoadSceneChannel;
        [SerializeField] LoadEventChannelSO coldStartLoadSceneChannel;   // Used for the cold start code to tell the load system what scene it started on
        


        void OnEnable()
        {
            requestLoadSceneChannel.OnEventRaised += HandleSceneRequested;
            coldStartLoadSceneChannel.OnEventRaised += HandleColdStartScene;
        }

        void OnDisable()
        {
            requestLoadSceneChannel.OnEventRaised -= HandleSceneRequested;
            coldStartLoadSceneChannel.OnEventRaised -= HandleColdStartScene;
        }

        void HandleSceneRequested(AssetReference requestedScene)
        {
            StopAllCoroutines();
            StartCoroutine(SwapScene(requestedScene));
        }

        void HandleColdStartScene(AssetReference coldStartScene)
        {
            currentlyLoadedGameScene = coldStartScene;
        }

        IEnumerator SwapScene(AssetReference requestedScene)
        {
            inputReader.DisableAllInput();
            fadeOutRequestChannel.RaiseEvent(fadeDuration);

            yield return new WaitForSecondsRealtime(fadeDuration);

            try
            {
                currentlyLoadedGameScene.UnLoadScene();
                currentlyLoadedGameScene.UnLoadScene().WaitForCompletion(); // Prevents new scene from loading before old one has finished unloading
            }
            catch
            {
                SceneManager.UnloadSceneAsync(currentlyLoadedGameScene.editorAsset.name);
            }

            void EnableInputAndFadeIn(AsyncOperationHandle<SceneInstance> op)
            {
                //inputReader.EnableGameplayInput();
                fadeInRequestChannel.RaiseEvent(fadeDuration);
                currentlyLoadedGameScene = requestedScene;
                sceneReadyEventChannel.RaiseEvent();
            }
            requestedScene.LoadSceneAsync(LoadSceneMode.Additive).Completed += EnableInputAndFadeIn;
        }
    }
}

