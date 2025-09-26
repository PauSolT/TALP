using UnityEngine;
using UnityEditor;
using System.IO;

public class SyllabSOMassCreator : EditorWindow
{
    private TextAsset csvFile; 
    private string targetFolder = "Assets/";

    [MenuItem("Tools/SyllabSO Mass Creator")]
    public static void ShowWindow()
    {
        // Open the custom editor window
        GetWindow<SyllabSOMassCreator>("SyllabSO Mass Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Mass SyllabSO Creator", EditorStyles.boldLabel);
        GUILayout.Space(10);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV Data File", csvFile, typeof(TextAsset), false);

        GUILayout.Space(10);

        GUILayout.Label("Select Target Folder", EditorStyles.boldLabel);
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
        if (GUILayout.Button("Create All SyllabSO from CSV"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a CSV file.", "OK");
                return;
            }
            if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid target folder.", "OK");
                return;
            }

            MassCreateSyllabSO();
        }
    }

    private void MassCreateSyllabSO()
    {
        // Split the CSV file's text into individual lines
        string[] lines = csvFile.text.Split('\n');
        int createdCount = 0;

        // Start the loop from 1 to skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue; // Skip any empty lines

            // Split the line into values based on the comma delimiter
            string[] values = line.Split(',');

            if (values.Length < 3)
            {
                Debug.LogWarning($"Skipping malformed line {i + 1}: Not enough columns.");
                continue;
            }

            SyllabSO newSyllab = CreateInstance<SyllabSO>();

            // Assign data from the CSV columns to the ScriptableObject fields
            newSyllab.romaji = values[0].Trim();
            newSyllab.hiragana = values[1].Trim();
            newSyllab.katakana = values[2].Trim();

            string assetPath = Path.Combine(targetFolder, $"{newSyllab.romaji}.asset");
            // Ensure the filename is unique to avoid overwriting existing assets
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            // Create the .asset file in the project
            AssetDatabase.CreateAsset(newSyllab, uniquePath);
            createdCount++;
        }

        // Save all newly created assets to disk
        AssetDatabase.SaveAssets();
        // Refresh the Asset Database to make sure the new assets appear in the Project window
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"{createdCount} ScriptableObjects created successfully in:\n{targetFolder}", "OK");

        // Ping the target folder to highlight it for the user
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(targetFolder));
    }
}