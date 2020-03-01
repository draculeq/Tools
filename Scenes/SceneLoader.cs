using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Deadbit.Tools
{
    [ExecuteInEditMode]
    public class SceneLoader : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField, OnValueChanged("AssignSceneName")] private Object sceneToLoad;
        [SerializeField] private string sceneName;

        [SerializeField] private bool useAsyncLoad = true;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;
        [SerializeField] private bool loadOnStart;
        [SerializeField] private bool loadOnlyOnce = true;
        [ShowIf("useAsyncLoad"), SerializeField] private bool allowSceneActivation = true;

        [SerializeField] private UnityEvent sceneLoaded;
#pragma warning restore 649

        private AsyncOperation asyncLoad;

        [Button]
        private void AssignSceneName()
        {
            if (sceneToLoad && sceneName != sceneToLoad.name)
            {
                sceneName = sceneToLoad.name;
                Debug.LogWarning("Fixed scene name: " + sceneName, this);
#if UNITY_EDITOR
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
            }
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                if (loadOnStart)
                {
                    LoadScene();
                }
            }
            else
            {
                AssignSceneName();
            }
        }

        public void LoadScene()
        {
            if (loadOnlyOnce && SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                sceneLoaded.Invoke();
                return;
            }

            if (useAsyncLoad)
            {
                asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
                asyncLoad.allowSceneActivation = allowSceneActivation;
            }
            else
            {
                SceneManager.LoadScene(sceneName, loadSceneMode);
            }
        }

        public void ActivateScene()
        {
            allowSceneActivation = true;
            if (asyncLoad != null)
                asyncLoad.allowSceneActivation = true;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene name, LoadSceneMode mode)
        {
            if (name.name == sceneName)
                sceneLoaded.Invoke();
        }
    }
}
