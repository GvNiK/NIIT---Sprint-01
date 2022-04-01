using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System;
using System.IO;

public class AssetsEditorCustom 
{
    private GameObject escapePoint;
    private GameObject navMesh;
    private NavMeshSurface navMeshSurface;
    private List<string> assetsPaths;
    private List<string> assetNames;
    private string groupName;
    private GameObject newGroup;
    private List<FoldOutObject> groupList;
    private int groupCount;
    private List<AssetInstance> prefabInstances;
    private List<string> prefabNames;
    private List<string> prefabPaths;
    //private GameObject[] AssetGroupObjects;

    public AssetsEditorCustom()
    {
       escapePoint = GameObject.Find("EscapePoint");  

        navMesh = GameObject.Find("NavMesh");
        navMeshSurface = navMesh.GetComponent<NavMeshSurface>();    //Created the Surface of
        prefabInstances = new List<AssetInstance>(); 
        groupList = new List<FoldOutObject>();
        groupCount = 0;

        prefabPaths = new List<string>();
        prefabPaths.Add("-- Select Asset Folder --");
        prefabPaths.Add("Barriers");
        prefabPaths.Add("Props");
        prefabPaths.Add("Terrain");
    }

    public void Render()
    {
        GUILayout.BeginHorizontal();

            if(GUILayout.Button("Select EsacapePoint"))     //Escape Point
            {
                Selection.activeObject = escapePoint;
                SceneView.lastActiveSceneView.LookAt(escapePoint.transform.position);
                Debug.Log(Selection.activeObject.name);
            }

            if(GUILayout.Button("Updtae NavMesh"))      //Updated & Build NavMeshSurface
            {
                Selection.activeObject = navMesh;
                SceneView.lastActiveSceneView.LookAt(navMesh.transform.position);

                if(navMeshSurface != null)
                {
                    navMeshSurface.BuildNavMesh();
                    Debug.Log("NavMesh Updated!");
                }
            }

        GUILayout.EndHorizontal();

        /////--------------Creating New Group------------------------//////
        GUILayout.BeginHorizontal();

            GUILayout.Label("New Group :", GUILayout.Width(80)); 

            //TextField: Make a single-line text field where the user can edit a string.                                                               
            groupName = GUILayout.TextField(groupName);  
            
            //Create Group Button.
            if (GUILayout.Button("Add Group", GUILayout.Width(100)) && groupName != null && GameObject.Find(groupName) == false) 
            {
                newGroup = new GameObject(groupName);  //Create Empty GameObject with Name.
                newGroup.tag = "AssetGroup";           //Adding Tag.
                groupList = new List<FoldOutObject>();
            }

        GUILayout.EndHorizontal();

        DisplayGroup(); 
        
    }

    private void DisplayGroup()     //Creating & Deleting Group(s).
    {
        GameObject[] AssetGroupObjects = GameObject.FindGameObjectsWithTag("AssetGroup"); //Finds AssetGroup gameObjects with Tag "AssetGroup"
        if (groupCount != AssetGroupObjects.Length)
        {
            groupList = new List<FoldOutObject>();
            groupCount = AssetGroupObjects.Length; 

            foreach (GameObject group in AssetGroupObjects)
            {
                groupList.Add(new FoldOutObject(group.name, 0, 0));  //Takes String - Name & Int - Number.
            }
        }
    
        //g = Group & sb = Sub-Group.
        for (int g = groupList.Count - 1; g >= 0; g--)
        {
            groupList[g].expanded = EditorGUILayout.Foldout(groupList[g].expanded, groupList[g].name); 
            if (groupList[g].expanded)                                                                 
            {       
                //This Line Closes a Sub-Group, when another Group is Expanded.
                for (int sb = groupList.Count - 1; sb >= 0; sb--)                                    
                {
                    if(groupList[sb].name != groupList[g].name && groupList[sb].expanded == true)
                    {
                        groupList[sb].expanded = false;
                        //prefabInstances = new List<AssetInstance>();
                    }                    
                }
                
                //Creating the Sub-Group.
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        groupList[g].name = GUILayout.TextField(AssetGroupObjects[g].name);                    
                        AssetGroupObjects[g].name = groupList[g].name;

                        if (GUILayout.Button("Delete Group", GUILayout.Width(100)))   //Delete                         
                        {
                            GameObject.DestroyImmediate(AssetGroupObjects[g]);
                            groupList.Remove(groupList[g]);  
                            //prefabInstances = new List<AssetInstance>();
                        }
                    }
                    GUILayout.EndHorizontal(); 

                    
                    GUILayout.BeginHorizontal(); 
                    {
                        //Get the Path of All the Assets present in the Particular Folder (Ex. Barrires).
                        //prefabPaths = Constsits of 3 Assets Groups decalred above.
                        groupList[g].no1 = EditorGUILayout.Popup(groupList[g].no1, prefabPaths.ToArray());   // Dropdown - 3 Assets Groups.  

                        prefabNames = new List<string>();   //Bcoz - We need to create an Instance of it before we use it.
                        
                        if (groupList[g].no1 > 0)                                                              
                        {
                            string[] files = Directory.GetFiles("Assets/Prefabs/Environment/" + prefabPaths[groupList[g].no1], "*.prefab", SearchOption.AllDirectories);
                            foreach (string file in files)
                            {
                                prefabNames.Add(Path.GetFileName(file)); 
                                //Debug.Log(prefabNames);
                            }
                        }

                        //Getting the Each Individual Prefab present inside the Folders (Ex. Barrires/abc.prefab).
                        groupList[g].no2 = EditorGUILayout.Popup(groupList[g].no2, prefabNames.ToArray());     // Dropdown - Individual Assets.

                        if (GUILayout.Button("+", GUILayout.Width(50)) && groupList[g].no2 > 0)                
                        {
                            string prefabPath = "Assets/Prefabs/Environment/" + prefabPaths[groupList[g].no1] + "/" + prefabNames[groupList[g].no2];
                            GameObject initialPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                            GameObject child = (GameObject)PrefabUtility.InstantiatePrefab(initialPrefab);   
                            child.transform.parent = AssetGroupObjects[g].transform;    //Parenting Child into the Specific Group.
                            //prefabInstances = new List<AssetInstance>();
                        }
                    }    
                    GUILayout.EndHorizontal();

                    ////-------------------------/////
                    //IndividualAssetOptions();
                    int AssetCount = AssetGroupObjects[g].transform.childCount;     //Get Count of Assets Under Group from Hierarchy.

                    for (int i = 0; i < AssetCount; i++)    //Adds Group into AssetInstance.
                    {
                        prefabInstances.Add(new AssetInstance(AssetGroupObjects[g].transform.GetChild(i).gameObject));
                    }

                    for (int i = 0; i < AssetCount; i++)    //Get Individual Assets from the AssetInstance List.
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            prefabInstances[i].expanded = EditorGUILayout.Foldout(prefabInstances[i].expanded, prefabInstances[i].instance.name);
                            if (prefabInstances[i].expanded)                            
                            {
                                //Collapses the Active Asset, if we open any other Asset.
                                for (int k = 0; k < AssetCount; k++)                    
                                {
                                    if (prefabInstances[k].instance.name != prefabInstances[i].instance.name && prefabInstances[k].expanded == true)
                                    {
                                        prefabInstances[k].expanded = false;
                                    }
                                }

                                prefabInstances[i].instance.name = GUILayout.TextField(AssetGroupObjects[g].transform.GetChild(i).name); //Asset TextBox
                                AssetGroupObjects[g].transform.GetChild(i).name = prefabInstances[i].instance.name;

                                GUILayout.BeginHorizontal();
                                {
                                    if (GUILayout.Button("Select"))                         
                                    {
                                        if (Selection.activeGameObject != null)
                                        {
                                            Debug.Log(Selection.activeGameObject.name);
                                        }
                                        SceneView.lastActiveSceneView.LookAt(AssetGroupObjects[g].transform.GetChild(i).position);
                                        Selection.activeGameObject = AssetGroupObjects[g].transform.GetChild(i).gameObject;
                                        Debug.Log(Selection.activeGameObject.name);
                                    }

                                    if (GUILayout.Button("Duplicate"))                      
                                    {
                                        GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(AssetGroupObjects[g].transform.GetChild(i).gameObject);
                                        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabRoot);
                                        GameObject initialPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                                        GameObject child = (GameObject)PrefabUtility.InstantiatePrefab(initialPrefab);
                                        child.transform.parent = AssetGroupObjects[g].transform;
                                        //prefabInstances = new List<AssetInstance>();
                                    }

                                    if (GUILayout.Button("Delete"))                         
                                    {
                                        GameObject.DestroyImmediate(AssetGroupObjects[g].transform.GetChild(i).gameObject);
                                        //prefabInstances = new List<AssetInstance>();
                                        //prefabInstances.Remove(AssetGroupObjects[g].transform.GetChild(i).gameObject);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                        }    
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndVertical();

                }
            }
        }
    }
}

