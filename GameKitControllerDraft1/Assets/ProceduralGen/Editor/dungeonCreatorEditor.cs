using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonCreator))]
public class dungeonCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DungeonCreator dungeonCreator = (DungeonCreator)target;
        if(GUILayout.Button("CreateNewDungeon"))
        {
            dungeonCreator.CreateDungeon();
        }
    }
}
