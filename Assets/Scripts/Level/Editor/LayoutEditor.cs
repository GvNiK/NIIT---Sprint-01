using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LayoutEditor 
{
    public LayoutEditor()
    {
        
    }
    
    public void Render()
    {
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Button 1", GUILayout.Width(100), GUILayout.Height(40))) {};
            
            GUILayout.BeginVertical();

                if(GUILayout.Button("Button 2",  GUILayout.Width(100))) {};
                if(GUILayout.Button("Button 3",  GUILayout.Width(100))) {};
                
            GUILayout.EndVertical();

        GUILayout.EndHorizontal();

    }
}
