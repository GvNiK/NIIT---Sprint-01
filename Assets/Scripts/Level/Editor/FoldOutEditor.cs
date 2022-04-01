using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FoldOutEditor 
{
    private List<FoldOutObject> foldOutObject;

    public FoldOutEditor()
    {
        foldOutObject = new List<FoldOutObject>();

        for(int i = 0; i < 3; i++)
        {
            foldOutObject.Add(new FoldOutObject("Name" + i, i, i));
        }
    }

    public void RenderFoldOut()
    {
        foreach(FoldOutObject fo in foldOutObject)
        {
            fo.expanded = EditorGUILayout.Foldout(fo.expanded, fo.name);
            if(fo.expanded)
            {
                GUILayout.Label(fo.name); 
            }
        }
    }
}
