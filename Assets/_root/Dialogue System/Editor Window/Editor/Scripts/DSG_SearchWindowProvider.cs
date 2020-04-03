using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements.Experimental;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

namespace DSG
{
    public class DSG_SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public DSG_GraphView graphView;
        public EditorWindow editorWindow;

        public void Initialize(EditorWindow _editorWindow, DSG_GraphView _graphView)
        {
            editorWindow = _editorWindow;
            graphView = _graphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            { 
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            tree.Add(new SearchTreeEntry(new GUIContent("Dialogue")) { level = 1, userData = typeof(DSG_TextNode) });

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            System.Type type = (System.Type)searchTreeEntry.userData;
            DSG_Node element;

            element = graphView.MakeNode(type);

            var windowRoot = editorWindow.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - editorWindow.position.position);
            var pos = graphView.contentViewContainer.WorldToLocal(windowMousePosition);

            element.SetPosition(new Rect(pos.x, pos.y, 0, 0));

            graphView.AddElement(element);
            graphView.AddNode(element);
            return true;
        }

        private T ConvertNode<T>(object obj) where T : DSG_Node
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}