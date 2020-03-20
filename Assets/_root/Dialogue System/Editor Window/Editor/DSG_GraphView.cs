using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor.Graphing;
using UnityEditor.Graphing.Util;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using UnityEngine.Rendering;

using UnityEditor.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.UIElements;
using UnityEditor.VersionControl;
using UnityEditor.ShaderGraph.Drawing;

public class DSG_GraphView : GraphView
{
    public DSG_ConnectorListener connectorListener;

    List<DSG_Node> nodes = new List<DSG_Node>();

    public DSG_GraphView()
    {
        style.height = new Length(100, LengthUnit.Percent);
        style.width = new Length(100, LengthUnit.Percent);
        style.backgroundColor = new Color(0.6f, 0, 0.3f);

        SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ClickSelector());

        connectorListener = new DSG_ConnectorListener();
    }

    public void AddNode(DSG_Node _node)
    {
        nodes.Add(_node);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> result = new List<Port>();

        foreach(var n in nodes)
        {
            if (n.inputPort == startPort || n.outputPort == startPort)
                continue;

            Port subject = startPort.direction == Direction.Input ? n.outputPort : n.inputPort;

            if (startPort.portType == subject.portType)
                result.Add(subject);
        }

        return result;
    }
}

public class DSG_Node : Node
{
    public Port inputPort;
    public Port outputPort;

    IEdgeConnectorListener connectorListener;

    public DSG_Node(string title, IEdgeConnectorListener _connectorListener)
    {
        UseDefaultStyling();
        this.title = title;
        connectorListener = _connectorListener;

        extensionContainer.Add(new Label("Extension"));
        extensionContainer.style.backgroundColor = style.backgroundColor;


        inputPort = DSG_Port.Create(Direction.Input, _connectorListener);
        inputContainer.Add(inputPort);

        outputPort = DSG_Port.Create(Direction.Output, _connectorListener);
        outputContainer.Add(outputPort);
    }
}

public class DSG_Port : Port
{
    public DSG_Port(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
    {
        
    }

    public static Port Create(Direction direction, IEdgeConnectorListener connectorListener)
    {
        var port = new DSG_Port(Orientation.Horizontal, direction, Capacity.Single, typeof(int))
        {
            m_EdgeConnector = new EdgeConnector<Edge>(connectorListener),
        };

        port.AddManipulator(port.m_EdgeConnector);
        port.portName = "Port";

        return port;
    }
}

public class DSG_ConnectorListener : IEdgeConnectorListener
{
    public void OnDrop(GraphView _graphView, Edge edge)
    {
        edge.output.Connect(edge);
        edge.input.Connect(edge);
        _graphView.AddElement(edge);
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        Debug.Log("OnDropOutsidePort");


    }
}

/* TODOS:
 * Que se cree el nodo que quiero
 * Hacer mas streamlined el proceso de creación de nodos 
 * Guardar todos los nodos, edges etc en diferentes lados
 *      Que no se ponga el edge en donde mismo varias veces (estaba en alguna parte de shader graph eso)
 * Hacer la serialización de los nodos
 * Hacer la conversión de nodos a assets
 */
