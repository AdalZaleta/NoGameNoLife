using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

namespace DSG
{
    public class DSG_TextNode : DSG_Node
    {
        TextField textField;

        public DSG_TextNode(string title, IEdgeConnectorListener _connectorListener) : base(title, _connectorListener) { }

        protected override void MakeSettings(VisualElement parentContainer)
        {
            VisualElement container = new VisualElement() { name = "settings" };

            container.AddToClassList("node-settings-container");

            textField = new TextField("Text:", 600, true, false, 'f');
            textField.AddToClassList("text-box");

            container.Add(textField);

            parentContainer.Add(container);
        }

        protected override DialogueNode CreateDialogueNodeInstance()
        {
            return ScriptableObject.CreateInstance<DN_Text>();
        }

        protected override void SaveDialogueNode(DialogueNode dn)
        {
            DN_Text tempNode = (DN_Text)dn;

            tempNode.text = textField.text;
        }

        public override Type GetNodeType()
        {
            return typeof(DSG_TextNode);
        }
    }
}