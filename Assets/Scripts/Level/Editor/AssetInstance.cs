using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetInstance 
{
    public GameObject instance;
    public bool expanded;
   /* public object[] assets;
    private string[] assetPaths;
    private string PATH_TO_BARRIERS = "Assets/Prefabs/Environment/Barriers";
    private string[] prefabsPaths;
    private List<GameObject> prefablist;*/

    public AssetInstance(GameObject instance)
    {
        this.instance = instance;  
    }

    /*public void GetAllAssets()
    {
        
        /*prefablist = new List<GameObject>();
        string path = Path.Combine (Application.dataPath, "Assets/Prefabs/Environment/Barriers/");
        path = path.Replace(@"\", "/");
        DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles("*.prefab"))
        {
            GameObject prefab = (GameObject)AssetDatabase. LoadAssetAtPath<GameObject>("Assets/" + "Prefabs/Environment/Barriers/" + file.Name);
            prefablist.Add(prefab);
            Debug.Log(prefablist);
        }

        //assetPaths = AssetDatabase.GetAllAssetPaths();

        foreach(object assetPaths in prefabsPaths)
        {
            //asset = PrefabUtility.GetPrefabType(instance);
            Debug.Log(assetPaths);
            Debug.Log("Button Pressed!");
        }

    }*/
}
