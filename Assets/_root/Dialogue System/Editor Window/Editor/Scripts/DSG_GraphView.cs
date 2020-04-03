using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System.Reflection;

namespace DSG
{
    public class DSG_GraphView : GraphView
    {
        public DSG_ConnectorListener connectorListener;
        public DialogueTree tree;

        List<DSG_Node> dsgNodes = new List<DSG_Node>();

        public DSG_GraphView(DialogueTree tree)
        {
            SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            connectorListener = new DSG_ConnectorListener();
            this.tree = tree;
            if (tree == null) Debug.Log("GraphView constructor tree is null");
        }

        public List<DSG_Node> DSGNodes
        {
            get { return dsgNodes; }
            private set { }
        }

        public void AddNode(DSG_Node _node)
        {
            dsgNodes.Add(_node);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> result = new List<Port>();

            foreach (var n in dsgNodes)
            {
                if (n.inputPort == startPort || n.outputPort == startPort)
                    continue;

                Port subject = startPort.direction == Direction.Input ? n.outputPort : n.inputPort;

                if (startPort.portType == subject.portType)
                    result.Add(subject);
            }

            return result;
        }

        public void LoadNodes(DSG_TreeGraphData.GraphData graphData)
        {
            Debug.Log("GraphView Load data attempt");
            if (graphData == null) { Debug.Log("graph data was null"); return; }
            if (graphData.nodeTypes == null) { Debug.Log("Node types was null"); return; }
            if (graphData.nodePositions == null) { Debug.Log("Node positions was null"); return; }
            if (graphData.nodePositions.Count != tree.Nodes.Count || graphData.nodeTypes.Count != tree.Nodes.Count) { Debug.Log("GraphData nodes: " + graphData.nodePositions.Count + ", tree nodes: " + tree.Nodes.Count); return; }

            Debug.Log("Graph view loading data...");

            for(int i = 0; i < graphData.nodePositions.Count; i++)
            {
                DSG_Node node = MakeNode(graphData.nodeTypes[i]);
                node.SetPosition(graphData.nodePositions[i]);
                node.dialogueNode = tree.Nodes[i];

                AddElement(node);
                AddNode(node);
            }

            for(int i = 0; i < dsgNodes.Count; i++)
            {
                for(int j = 0; j < dsgNodes.Count; j++)
                {
                    if(dsgNodes[i].dialogueNode.nextNode == dsgNodes[i].dialogueNode)
                    {
                        Edge edge = new Edge();
                        dsgNodes[i].outputPort.Connect(edge);
                        dsgNodes[j].inputPort.Connect(edge);
                        AddElement(edge);
                        break;
                    }
                }
            }
        }

        public void SaveGraph(DSG_TreeGraphData graphData)
        {
            Debug.Log("GraphView saving graph");
            graphData.SaveGraph(this);

            foreach (var n in dsgNodes)
                n.SaveNode(tree);

            foreach (var n in dsgNodes)
                n.SaveNodeConnections();
        }

        public DSG_Node MakeNode(System.Type type)
        {
            if (!type.IsSubclassOf(typeof(DSG_Node))) return null;

            DSG_Node node;
            var createNodeMethod = typeof(DSG_Node)
                                 .GetMethod("CreateNode",
                                             BindingFlags.Static | BindingFlags.Public);

            var method = createNodeMethod.MakeGenericMethod(type);
            node = method.Invoke(null, new object[] { type.ToString(), connectorListener }) as DSG_Node;

            return node;
        }
    }

    public class DSG_Node : Node
    {
        public Port inputPort;
        public Port outputPort;

        public DialogueNode dialogueNode;

        IEdgeConnectorListener connectorListener;

        public DSG_Node(string title, IEdgeConnectorListener _connectorListener)
        {
            this.title = title;
            connectorListener = _connectorListener;

            inputPort = DSG_Port.Create(Direction.Input, Port.Capacity.Multi, _connectorListener);
            inputContainer.Add(inputPort);

            outputPort = DSG_Port.Create(Direction.Output, Port.Capacity.Single, _connectorListener);
            outputContainer.Add(outputPort);

            MakeSettings(extensionContainer);

            RefreshExpandedState();
            UseDefaultStyling();
        }

        public static T CreateNode<T>(string title, IEdgeConnectorListener _connectorListener) where T : DSG_Node
        {
            return Activator.CreateInstance(typeof(T), title, _connectorListener) as T;
        }

        protected virtual void MakeSettings(VisualElement parentContainer) { }

        public void SaveNode(DialogueTree tree)
        {
            if (dialogueNode == null)
            {
                DialogueNode tempNode = CreateDialogueNodeInstance();
                SaveDialogueNode(tempNode);
                dialogueNode = tempNode;
                if (tree == null) Debug.Log("Tree is null");
                if (dialogueNode == null) Debug.Log("DialogueNode is null");
                tree.AddNode(dialogueNode);
                AssetDatabase.AddObjectToAsset(dialogueNode, tree);
            }
            else
            {
                SaveDialogueNode(dialogueNode);
            }
        }

        protected virtual DialogueNode CreateDialogueNodeInstance()
        {
            return ScriptableObject.CreateInstance<DialogueNode>();
        }

        protected virtual void SaveDialogueNode(DialogueNode dn) { }

        public virtual void SaveNodeConnections()
        {
            //Haciendo conecciones, deberian de conectarse despues de que se hayan creado todos
            if (outputPort.connected)
            {
                foreach (var c in outputPort.connections)
                {
                    Node tempNode = c.input.node;
                    if (tempNode.GetType().IsSubclassOf(typeof(DSG_Node)))
                    {
                        dialogueNode.nextNode = ((DSG_Node)tempNode).dialogueNode;
                    }
                }
            }
        }

        public virtual System.Type GetNodeType()
        {
            return typeof(DSG_Node);
        }

        protected override void OnPortRemoved(Port port)
        {
            base.OnPortRemoved(port);
        }
    }

    public class DSG_Port : Port
    {
        List<Edge> edges;

        public DSG_Port(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) { }

        public static Port Create(Direction direction, Capacity _capacity, IEdgeConnectorListener connectorListener)
        {
            var port = new DSG_Port(Orientation.Horizontal, direction, _capacity, typeof(int))
            {
                m_EdgeConnector = new EdgeConnector<Edge>(connectorListener),
            };

            port.AddManipulator(port.m_EdgeConnector);
            port.portName = "Port";

            port.edges = new List<Edge>();

            return port;
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);

            edges.Add(edge);
        }

        public override void DisconnectAll()
        {
            base.DisconnectAll();
            foreach (var e in edges)
            {
                e.output?.Disconnect(e);
                e.input?.Disconnect(e);
                e.RemoveFromHierarchy();
            }
        }
    }

    public class DSG_ConnectorListener : IEdgeConnectorListener
    {
        public void OnDrop(GraphView _graphView, Edge edge)
        {
            if (edge.input.capacity == Port.Capacity.Single)
            {
                edge.input.DisconnectAll();
            }

            if (edge.output.capacity == Port.Capacity.Single)
            {
                edge.output.DisconnectAll();
            }

            edge.output.Connect(edge);
            edge.input.Connect(edge);
            _graphView.AddElement(edge);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            Debug.Log("OnDropOutsidePort");
        }
    }

    /* TODO:
     * Que se carguen las conexiones entre nodos y los datos especificos, como el texto
     * Que se borren los nodods de los assets y del arbol al ser borrado de el graph
     * Hacer los diferentes tipos de nodos
     * Que se pueda guardar el arbol
     * Que se pueda cargar el arbol
     * Que se pueda editar los settings de dialogo
     * Que se pueda probar en runtime y talvez en editor
     * Que se guarde el ultimo folder en donde guardaste el dialogue tree para la ventana editor
     */
}