using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GUIEditor 
{
    private float slider;
    private bool toggleValue;
    private bool foldOut;
    private int popupValue;

    public GUIEditor()
    {

    }

    public void RenderGUI()
    {
        GUILayout.Label("You have opened the Editor Window!");

        if(GUILayout.Button("Click Me"))
        {
            Debug.Log("You clicked the Button");
        }

        slider = EditorGUILayout.Slider("Volume", slider, 0, 1);
        toggleValue = EditorGUILayout.Toggle("Toggle", toggleValue);

        foldOut = EditorGUILayout.Foldout(foldOut, "FoldeOut Menu");
        if(foldOut)
        {
            GUILayout.Label("You have opened FoldOut!");
        }

        popupValue = EditorGUILayout.Popup("Languages", popupValue, new string [] {"English", "Hindi", "Marathi"});
    }
}
