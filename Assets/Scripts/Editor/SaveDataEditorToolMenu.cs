using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveDataEditorToolMenu
{
    private const string OpenSaveFileMenu = "Tools/Open Save File Directory";
    private const string OpenSaveJsonMenu = "Tools/Open Save File";
    private const string DeleteSaveFileMenu = "Tools/Delete Save File";

    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "SaveData.json");
    
    [MenuItem(OpenSaveFileMenu, false, 1)]
    private static void OpenSaveFileLocation()
    {
        var path = Application.persistentDataPath;
        Debug.Log($"[SaveTool] persistentDataPath = {path}");
        EditorUtility.RevealInFinder(path);
    }

    [MenuItem(OpenSaveFileMenu, true, 2)]
    private static bool ValidateOpenSaveFileLocation()
    {
        return Directory.Exists(Application.persistentDataPath);
    }
    
    [MenuItem(OpenSaveJsonMenu, false, 3)]
    private static void OpenSaveFile()
    {
        var filePath = SaveFilePath;
        Debug.Log($"[SaveTool] Save file path = {filePath}");
        EditorUtility.RevealInFinder(filePath);
    }

    [MenuItem(OpenSaveJsonMenu, true, 4)]
    private static bool ValidateOpenSaveFile()
    {
        return File.Exists(SaveFilePath);
    }
    
    [MenuItem(DeleteSaveFileMenu, false, 5)]
    private static void DeleteSaveFileLocation()
    {
        var path = Application.persistentDataPath;
        Debug.Log($"[SaveTool] Deleting contents of: {path}");

        try
        {
            Directory.Delete(path, true);
            Debug.Log("[SaveTool] Deleted folder and contents.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SaveTool] Failed to delete: " + ex);
        }
    }

    [MenuItem(DeleteSaveFileMenu, true, 6)]
    private static bool ValidateDeleteSaveFileLocation()
    {
        return Directory.Exists(Application.persistentDataPath);
    }
}