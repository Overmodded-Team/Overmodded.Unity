//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.UnityEngine.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple scene loader.
    /// </summary>
    public sealed class JEMSceneLoader : MonoBehaviour
    {
        /// <summary>
        ///     Loading panel.
        /// </summary>
        [Header("Settings")]
        public JEMInterfaceFadeAnimation LoadingPanel;
        public bool StartPanelAsDeactivated = true;

        /// <summary>
        ///     Load mode.
        /// </summary>
        public LoadSceneMode LoadMode = LoadSceneMode.Single;

        /// <summary>
        ///     On level loaded event.
        /// </summary>
        [Header("Events")]
        public UnityEvent OnLevelLoaded;

        /// <summary>
        ///     Async load scene of given name.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="sleep"></param>
        public void Load(string sceneName, float sleep = 0f)
        {
            if (IsLoading)
            {
                JEMLogger.LogError("JEMSceneLoader was unable to load scene of name " + sceneName +
                                   ". Another scene is currently loading.");
                return;
            }

            StartCoroutine(Slave(sceneName, sleep));
        }

        private IEnumerator Slave(string sceneName, float sleep)
        {
            IsLoading = true;
            if (StartPanelAsDeactivated)
            {
                LoadingPanel.ForceDeactivation();
            }

            LoadingPanel.SetActive(true);
            if (!StartPanelAsDeactivated)
            {
                LoadingPanel.ForceActiveState();
            }

            yield return new WaitForSeconds(sleep);
            var async = SceneManager.LoadSceneAsync(sceneName, LoadMode);
            yield return async;
            yield return new WaitForEndOfFrame();
            LoadingPanel.SetActive(false);
            yield return new WaitForEndOfFrame();
            OnLevelLoaded.Invoke();
            IsLoading = false;
        }

        /// <summary>
        ///     Reset default loader.
        /// </summary>
        public static void SetDefaultLoader(JEMSceneLoader loader = null)
        {
            Default = loader ? loader : FindObjectOfType<JEMSceneLoader>();
            DontDestroyOnLoad(Default.gameObject);
        }

        /// <summary>
        ///     Defines if any scene is currently loading.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        ///     Current instance of script.
        /// </summary>
        public static JEMSceneLoader Default { get; private set; }
    }
}
