using UnityEngine;
using UnityEditor;
using System;

//
// MaterialsCreator
// A very simple Editor script to create Materials from Textures.
//
// Made with Unity 2018.1.0f2. Probably works in older versions, but was not tested with them.
//
// Usage:
// Select one or more Textures (e.g. in Assets/MyProjectFolder/Textures/)
// Select Tools -> Create Materials From Selected Textures
// The Textures will be created in a SIBLING folder called [MATERIAL_FOLDER_NAME] (Default: "Materials")
//
public class MaterialsCreator : Editor
{

    private static readonly string MATERIAL_FOLDER_NAME = "Materials";

    [MenuItem("Tools/Create Materials From Selected Textures")]
    static void CreateMaterials()
    {

        try
        {

            AssetDatabase.StartAssetEditing();

            // Get all Textures from current editor selection
            Texture[] textures = Selection.GetFiltered<Texture>(SelectionMode.Assets);

            foreach (Texture tex in textures)
            {

                // Yes, this IS a lot of string declarations. But it works, so I cannot be arsed to make it more 
                // compact. Just don't put this code in a self-driving car or rocket ship or anything.
                string rawTexturePath = AssetDatabase.GetAssetPath(tex);
                string textureFilename = rawTexturePath.Substring(rawTexturePath.LastIndexOf("/") + 1);
                string rawName = textureFilename.Substring(0, textureFilename.LastIndexOf("."));
                string currentFolderPath = rawTexturePath.Substring(0, rawTexturePath.LastIndexOf("/"));
                string parentFolderPath = rawTexturePath.Substring(0, currentFolderPath.LastIndexOf("/"));
                string materialFolderPath = parentFolderPath + "/" + MATERIAL_FOLDER_NAME;
                string finalNewMaterialFilePath = materialFolderPath + "/" + rawName + ".mat";

                // Create the materials folder as a sibling folder if it's not there already
                if (AssetDatabase.IsValidFolder(materialFolderPath) == false)
                {
                    AssetDatabase.CreateFolder(parentFolderPath, MATERIAL_FOLDER_NAME);
                }

                if (AssetDatabase.LoadAssetAtPath(finalNewMaterialFilePath, typeof(Material)) != null)
                {
                    Debug.LogWarning("MaterialsCreator:: Material already exists for '"+ finalNewMaterialFilePath + "'. Skipping this Texture.");
                }
                else
                {

                    // Create new material based off of the Standard shader, but cheaper (no reflections or other goo)
                    Material myMaterial = new Material(Shader.Find("Standard"));
                    myMaterial.SetTexture("_MainTex", tex);
                    myMaterial.SetFloat("_Glossiness", 0.0f);
                    myMaterial.SetFloat("_SpecularHighlights", 0.0f);
                    myMaterial.SetFloat("_GlossyReflections", 0.0f);

                    AssetDatabase.CreateAsset(myMaterial, finalNewMaterialFilePath);

                }

            }

        }
        catch(Exception e)
        {
            Debug.LogError("MaterialsCreator:: Excepton occurred. Reason: " + e.Message);
        }
        finally
        {

            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();

        }

    }

}