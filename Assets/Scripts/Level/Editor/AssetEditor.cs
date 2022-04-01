using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetEditor
{
    private GameObject initialPrefab;
    private Transform environment;
    private List<AssetInstance> assetInstances;
    private AssetInstance instanceOfAssets;
    private int pathInded;
    private int tempPathIndex;
    private List<GameObject> prefablist;

    public AssetEditor()
    {
        //AssetDatabase: An Interface for accessing assets and performing operations on assets.
        initialPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Environment/Barriers/Metal_Barrier_Half.prefab");
        environment = GameObject.Find("Environment").gameObject.transform;
        //initialPrefab.transform.position += new Vector3(0, 0, 10);

        assetInstances = new List<AssetInstance>();

        instanceOfAssets = new AssetInstance(initialPrefab);

    }
 
    public void Render()
    {
        for(int i = assetInstances.Count - 1; i >= 0; i--)   //each(AssetInstance asset in assetInstances)
        {
            AssetInstance asset = assetInstances[i];

            asset.expanded = EditorGUILayout.Foldout(asset.expanded, asset.instance.name);

            if(asset.expanded)
            {
                asset.instance.name = GUILayout.TextField(asset.instance.name);

                GUILayout.BeginHorizontal();

                    if(GUILayout.Button("Select", GUILayout.Width(100)))
                    {
                        //Selection: Access to the selection in the editor.
                        Selection.activeObject = asset.instance;
                        SceneView.lastActiveSceneView.LookAt(asset.instance.transform.position);
                    }

                    if(GUILayout.Button("Duplicate", GUILayout.Width(100)))
                    {
                        //We get the recently created or instantiated GameObject.
                        GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(asset.instance);

                        //GetPrefabAssetPathOfNearestInstanceRoot: Returns the asset path of the nearest Prefab instance root the specified object is part of.
                        string _path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabRoot);

                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_path);
                        assetInstances.Add(new AssetInstance( (GameObject)PrefabUtility.InstantiatePrefab(prefab) ));     //Typecasting.
                        //Also can be written as below: 
                        //assetInstances.Add(new AssetInstance( PrefabUtility.InstantiatePrefab(prefab) as GameObject )); 

                    }                

                    if(GUILayout.Button("Delete", GUILayout.Width(100)))
                    {
                        GameObject.DestroyImmediate(asset.instance);
                        assetInstances.Remove(asset);   //Removing from the List.
                    }                

                GUILayout.EndHorizontal();

            }
        }

        if(GUILayout.Button("+", GUILayout.Width(50)))
        {
            if(environment != null)
                assetInstances.Add(new AssetInstance((GameObject)PrefabUtility.InstantiatePrefab(initialPrefab, environment))); //TypeCasting = (GameObject).
        }

        
        /*if(GUILayout.Button("Display Assets", GUILayout.Width(100)))
        {
           instanceOfAssets.GetAllAssets();
           //Debug.Log("Button!");
        }*/
    }
}
