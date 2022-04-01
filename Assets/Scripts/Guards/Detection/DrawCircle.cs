using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawCircle : MonoBehaviour
{
   public static DrawWireArc drawCircle;

   public void DrawLine()
   {
       
   }
}

 [CustomEditor(typeof(DrawWireArc))]
    public class DrawWireArc : Editor
    {
        void OnSceneGUI() 
        {
            Guards guard = (Guards)target;
            Handles.color = Color.white;
            Handles.DrawWireArc
            (
                guard.transform.position,
                guard.transform.up, 
                guard.transform.forward, 
                360, 
                guard.visionData.radius
            );
        }
    }