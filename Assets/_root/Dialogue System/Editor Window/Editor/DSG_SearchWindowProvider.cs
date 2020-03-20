using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

public class DSG_SearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    public DSG_GraphView graphView;

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
        };

        tree.Add(new SearchTreeEntry(new GUIContent("TempTitle0", "Tooltip1")) { level = 1, userData = "0" });
        tree.Add(new SearchTreeEntry(new GUIContent("TempTitle1")) { level = 1, userData = "1" });
        tree.Add(new SearchTreeEntry(new GUIContent("TempTitle2")) { level = 1, userData = "2" });
        tree.Add(new SearchTreeEntry(new GUIContent("TempTitle3")) { level = 1, userData = "3" });

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    { 
        DSG_Node element = new DSG_Node("DSG_Node", graphView.connectorListener);

        //var windowMousePosition = graphView.GetRootVisualContainer().ChangeCoordinatesTo(_editorWindow.GetRootVisualContainer().parent, context.screenMousePosition - _editorWindow.position.position);

        var pos = graphView.WorldToLocal(context.screenMousePosition);

        //TODO: hacer otra cosa que no sea poner bien el pinchi nodo segun el mouse, igual otras cosas son mas importantes

        Debug.Log("Context pos: " + context.screenMousePosition);
        Debug.Log("Transformed: " + pos);

        element.SetPosition(new Rect(pos.x, pos.y, 0, 0));

        graphView.AddElement(element);
        graphView.AddNode(element);
        return true;
    }
}
