using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this type of scripts in the Editor Folder as they are not used in Runtime. That is, when the Game runs into the GameLogic or something.
public class FoldOutObject 
{
    public string name;
    public int no1;
    public int no2;
    public bool expanded;

    public FoldOutObject(string name, int no1, int no2)
    {
        this.name = name;
        this.no1 = no1;
        this.no2 = no2;
    }

}

