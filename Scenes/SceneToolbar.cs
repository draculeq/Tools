using UnityEditor;
using UnityEditor.SceneManagement;

namespace Assets.Plugins.Deadbit.Editor
{
    public class ScenesToolbar
    {
        //Most important scenes
        private const string introScenePath = "Assets/Scenes/Intro.unity";
        private const string mainMenuScenePath = "Assets/Scenes/MainMenu.unity";
        private const string unitTestsScenePath = "Assets/Scenes/UnitTests.unity";

        //Our necessary objects with game logic independent from level
        private const string necessaryLevelObjectsScenePath = "Assets/Scenes/Levels/Necessary Objects.unity";

        //Levels
        private const string testLevelScenePath = "Assets/Scenes/Levels/TestLevel.unity";
        private const string w1l1ScenePath = "Assets/Scenes/Levels/World 1/W1L1.unity";
        private const string w1l2ScenePath = "Assets/Scenes/Levels/World 1/W1L2.unity";
        private const string w1l3ScenePath = "Assets/Scenes/Levels/World 1/W1L3.unity";
        private const string w2l1ScenePath = "Assets/Scenes/Levels/World 2/W2L1.unity";
        private const string w3l1ScenePath = "Assets/Scenes/Levels/World 3/W3L1.unity";


        [MenuItem("Scenes/General/Intro")]
        private static void OpenIntroScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(introScenePath);
        }

        [MenuItem("Scenes/General/MainMenu")]
        private static void OpenMainMenuScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(mainMenuScenePath);
        }

        [MenuItem("Scenes/General/UnitTests")]
        private static void OpenUnitTestsScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(unitTestsScenePath);
        }

        [MenuItem("Scenes/Levels/TestLevel")]
        private static void OpenTestLevel()
        {
            OpenLevel(testLevelScenePath);
        }

        [MenuItem("Scenes/Levels/World 1/Level 1")]
        private static void OpenW1L1()
        {
            OpenLevel(w1l1ScenePath);
        }

        [MenuItem("Scenes/Levels/World 1/Level 2")]
        private static void OpenW1L2()
        {
            OpenLevel(w1l2ScenePath);
        }

        [MenuItem("Scenes/Levels/World 1/Level 3")]
        private static void OpenW1L3()
        {
            OpenLevel(w1l3ScenePath);
        }

        [MenuItem("Scenes/Levels/World 2/Level 1")]
        private static void OpenW2L1()
        {
            OpenLevel(w2l1ScenePath);
        }

        [MenuItem("Scenes/Levels/World 3/Level 1")]
        private static void OpenW3L1()
        {
            OpenLevel(w3l1ScenePath);
        }

        /// <summary>
        /// Loads scene with some necessary objects, then loads level as additive scene
        /// </summary>
        /// <param name="path"></param>
        private static void OpenLevel(string path)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(necessaryLevelObjectsScenePath);
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }
    }
}