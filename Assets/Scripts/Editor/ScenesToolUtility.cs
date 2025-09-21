using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class ScenesToolUtility
{
    [MenuItem("Scenes/InitScene")]
    public static void InitScene() => OpenEditorScene("InitScene");

    [MenuItem("Scenes/BattleScene")]
    public static void BattleScene() => OpenEditorScene("BattleScene");

    [MenuItem("Scenes/InventoryScene")]
    public static void InventoryScene() => OpenEditorScene("InventoryScene");

    [MenuItem("Scenes/MenuScene")]
    public static void MenuScene() => OpenEditorScene("MenuScene");


    static void OpenEditorScene(string sceneName)
    {
        if (Application.isPlaying)
            return;

        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/" + sceneName + ".unity");
    }
}