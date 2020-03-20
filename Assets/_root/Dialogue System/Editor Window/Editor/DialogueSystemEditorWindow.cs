using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogueSystemEditorWindow : EditorWindow
{
    DialogueTree currentTree;

    DSG_GraphView graphView;

    [MenuItem("Fluorescente/DIalogue System/Dialogue System Editor Window")]
    static void Init()
    {
        DialogueSystemEditorWindow wnd = GetWindow<DialogueSystemEditorWindow>();
        wnd.titleContent = new GUIContent("Dialogue System");
        wnd.minSize = new Vector2(750, 500);
    }

    public static void Init(DialogueTree tree)
    {
        DialogueSystemEditorWindow wnd = GetWindow<DialogueSystemEditorWindow>();
        wnd.titleContent = new GUIContent("Dialogue System");
        wnd.currentTree = tree;
        wnd.minSize = new Vector2(750, 500);
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.

        // Import UXML
        /*var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_root/Dialogue System/Editor Window/Editor/DialogueSystemEditorWindow.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);*/

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_root/Dialogue System/Editor Window/Editor/DialogueSystemEditorWindow.uss");
        /*VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);*/


        Toolbar toolbar = new Toolbar();
        root.Add(toolbar);

        /*Box mainBox = new Box()
        {
            style = {
                height= new Length(100, LengthUnit.Percent),
                width = new Length(100, LengthUnit.Percent),
                backgroundColor=new Color(0.6f,0,0.3f)
            }
        };
        root.Add(mainBox);

        mainBox.RegisterCallback<MouseUpEvent>(MainBoxMouseUp);*/

        graphView = new DSG_GraphView();
        root.Add(graphView);

        DSG_SearchWindowProvider windowProvider = CreateInstance<DSG_SearchWindowProvider>();
        windowProvider.graphView = graphView;

        graphView.nodeCreationRequest = (c) =>
        {
            SearchWindow.Open(new SearchWindowContext(c.screenMousePosition, 0.0f, 0.0f), windowProvider);
        };

        //graphView.RegisterCallback<KeyDownEvent>(SpaceDown);
    }

    void SpaceDown(KeyDownEvent evt)
    {
        if(evt.keyCode == KeyCode.Space)
        {
            DSG_Node element = new DSG_Node("DSG_Node", graphView.connectorListener);
            graphView.AddElement(element);

            DSG_Node element2 = new DSG_Node("DSG_Node", graphView.connectorListener);
            graphView.AddElement(element2);

            var con = new EdgeConnector<Edge>(graphView.connectorListener);

            
        }
    }

    public class TextNodeElement : VisualElement
    {
        DN_Text node;

        public TextNodeElement()
        {
            //node = new DN_Text(null);

            Box box = new Box()
            {
                style =
                {
                    position=Position.Relative,
                    flexGrow=1,
                    backgroundColor=Color.blue
                }
            };

            this.name = "text-node-element";
            m_Header = new Label("Text Node");
            m_Header.name = "header";
            m_Header.style.unityTextAlign = TextAnchor.MiddleCenter;

            box.Add(m_Header);
            Add(box);
        }

        Label m_Header;
        
    }

    class SquareDragger : MouseManipulator
    {
        private Vector2 m_Start;
        protected bool m_Active;

        public SquareDragger()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_Active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Start = e.localMousePosition;

                m_Active = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        protected void OnMouseMove(MouseMoveEvent e)
        {
            if (!m_Active || !target.HasMouseCapture())
                return;

            Vector2 diff = e.localMousePosition - m_Start;

            target.style.top = target.layout.y + diff.y;
            target.style.left = target.layout.x + diff.x;

            e.StopPropagation();
        }

        protected void OnMouseUp(MouseUpEvent e)
        {
            if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
                return;

            m_Active = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }
}