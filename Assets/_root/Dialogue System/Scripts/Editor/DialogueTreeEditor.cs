using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSG;

[CustomEditor(typeof(DialogueTree))]
public class DialogueTreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueTree s = (DialogueTree)target;

        if(GUILayout.Button("Edit"))
        {
            DialogueSystemEditorWindow.Init(s);  
        }
    }
}
