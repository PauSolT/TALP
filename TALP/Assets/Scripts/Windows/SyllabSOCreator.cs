using System.IO;
using UnityEditor;
using UnityEngine;

public class SyllabSOCreator : EditorWindow
{
    public string romaji;
    public string hiragana;
    public string katakana;
    private string targetFolder = "Assets/";

    [MenuItem("Tools/SyllabSO Creator")]
    public static void ShowWindow()
    {
        // Open the custom editor window
        GetWindow<SyllabSOCreator>("SyllabSO Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("SyllabSO Data", EditorStyles.boldLabel);
        romaji = EditorGUILayout.TextField("Romaji", romaji);
        hiragana = EditorGUILayout.TextField("Hiragana", hiragana);
        katakana = EditorGUILayout.TextField("Katakana", katakana);

        GUILayout.Space(10);

        GUILayout.Label("Target Folder", EditorStyles.boldLabel);
        // Button to select the folder where the assets will be saved
        if (GUILayout.Button("Select Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Target Folder", "Assets", "");
            if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
            {
                // Convert the absolute system path to a relative path
                targetFolder = "Assets" + path.Substring(Application.dataPath.Length);
            }
        }
        EditorGUILayout.LabelField("Current Folder:", targetFolder);

        GUILayout.Space(20);

        // The button that triggers the creation process
        if (GUILayout.Button("Create SyllabSO"))
        {
            CreateSyllabSO();
        }
    }

    private void CreateSyllabSO()
    {
        if (string.IsNullOrEmpty(romaji))
        {
            EditorUtility.DisplayDialog("Error", "Romaji cannot be empty.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(targetFolder))
        {
            EditorUtility.DisplayDialog("Error", "Please select a target folder.", "OK");
            return;
        }

        SyllabSO newSyllab = CreateInstance<SyllabSO>();

        newSyllab.romaji = romaji;
        newSyllab.hiragana = hiragana;
        newSyllab.katakana = katakana;

        string assetPath = Path.Combine(targetFolder, $"{romaji}.asset");
            // Ensure the filename is unique to avoid overwriting existing assets
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            // Create the .asset file in the project
        AssetDatabase.CreateAsset(newSyllab, uniquePath);
        // Save all newly created assets to disk
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        // Ping the target ScriptableObject to highlight it for the user
        Selection.activeObject = newSyllab;

        EditorUtility.DisplayDialog("Success", $"Successfully created {romaji}.asset at {uniquePath}", "OK");
    }
}
