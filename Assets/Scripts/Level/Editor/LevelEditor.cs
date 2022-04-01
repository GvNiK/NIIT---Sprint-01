using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{

    private Vector2 scroll;
    private int switchEditor;
    private GUIEditor guiEditor;
    private FoldOutEditor foldOutEditor;
    private LayoutEditor layoutEditor;
    private AssetEditor assetEditor;
    private AssetsEditorCustom assetsEditorCustom;


    [MenuItem("NIIT/Level Editor")]
    static void ShowEditor()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));  //Also can be written as:- LevelEditor window = EditorWindow.GetWindow<LevelEditor>();
        window.Show();  //Shows the Window.
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        guiEditor = new GUIEditor();
        foldOutEditor = new FoldOutEditor();
        layoutEditor = new LayoutEditor();
        assetEditor = new AssetEditor();
        assetsEditorCustom = new AssetsEditorCustom();
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);   //Start Scroll

            switchEditor = EditorGUILayout.Popup("Editors", switchEditor, new string[] {"GUI", "FoldOut", "Layout Editor", "Asset Editor", "Assets Editor Custom"});

            switch(switchEditor)
            {
                case 0:
                guiEditor.RenderGUI();
                break;

                case 1:
                foldOutEditor.RenderFoldOut();
                break;

                case 2:
                layoutEditor.Render();
                break;
                
                case 3:
                assetEditor.Render();
                break;

                case 4:
                assetsEditorCustom.Render();
                break;

                default:
                break;
            }

        EditorGUILayout.EndScrollView();    //End Scroll
    }
}
