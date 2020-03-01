using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deadbit.Tools
{
    [ExecuteInEditMode]
    public class SceneUnloader : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField, OnValueChanged("AssignSceneName")]
        private Object sceneToUnload;
        [SerializeField]
        private string sceneName;
#pragma warning restore 649

        public bool UnloadOnAwake;

        [Button]
        private void AssignSceneName()
        {
            if (sceneToUnload && sceneName != sceneToUnload.name)
            {
                sceneName = sceneToUnload.name;
                Debug.LogWarning("Fixed scene name: " + sceneName, this);
#if UNITY_EDITOR
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
            }
        }

        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (UnloadOnAwake)
                {
                    UnloadScene();
                }
            }
        }

        private void Start()
        {
            if (Application.isEditor)
            {
                AssignSceneName();
            }
        }

        public void UnloadScene()
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}