using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTree))]
public class DialogueTreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueTree s = (DialogueTree)target;

        if(GUILayout.Button("Open in editor"))
        {
            DialogueSystemEditorWindow.Init(s);
        }
    }
}
