using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DSG
{
    public class DialogueSystemEditorWindow : EditorWindow
    {
        public DialogueTree currentTree;
        DSG_TreeGraphData graphData;
        DSG_GraphView graphView;
        VisualElement mainMenu;

        string graphDataPath = "Assets/_root/Dialogue System/Editor Window/Editor";
        string graphDataName = "AllGraphDatas";

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
            VisualElement root = rootVisualElement;

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_root/Dialogue System/Editor Window/Resources/Styles/DialogueSystemWindow.uss");
            root.styleSheets.Add(styleSheet);

            if (currentTree != null) //TODO: hacer el menu para crear nuevos arboles o abrir assets ya creados, primero tiene que estar la serializacion y deserializacion
            {
                OpenGraphView(root);
            }
            else
            {
                OpenMainMenu(root);
            }

            Debug.Log("Dialogue System On Enable");
        }

        void OpenGraphView(VisualElement root)
        {
            if (mainMenu != null)
            {
                root.Remove(mainMenu);
            }

            if (graphView != null)
            {
                root.Add(graphView);
            }
            else
            {
                Toolbar toolbar = new Toolbar();
                root.Add(toolbar);

                ToolbarButton loadButton = new ToolbarButton(Load) { text = "Load" };
                toolbar.Add(loadButton);

                ToolbarButton saveButton = new ToolbarButton(Save) { text = "Save" };
                toolbar.Add(saveButton);

                ToolbarSpacer spacer = new ToolbarSpacer { name = "flexSpacer", flex = true };
                toolbar.Add(spacer);

                ToolbarToggle autoCompile = new ToolbarToggle { text = "Autocompile" };
                autoCompile.labelElement.style.color = Color.black;
                toolbar.Add(autoCompile);

                graphView = new DSG_GraphView(currentTree);
                root.Add(graphView);

                DSG_SearchWindowProvider windowProvider = CreateInstance<DSG_SearchWindowProvider>();
                windowProvider.Initialize(this, graphView);

                graphView.nodeCreationRequest = (c) =>
                {
                    SearchWindow.Open(new SearchWindowContext(c.screenMousePosition, 0.0f, 0.0f), windowProvider);
                };
            }

            graphData = GetGraphData();
            graphView.LoadNodes(graphData.GetDataFor(currentTree));
        }

        void OpenMainMenu(VisualElement root)
        {
            if(graphView != null)
            {
                root.Remove(graphView);
            }
            if (mainMenu != null)
            {
                root.Add(mainMenu);
            }
            else
            {
                mainMenu = new VisualElement() { name = "MainMenu" };
                root.Add(mainMenu);

                Button load = new Button(Load)
                {
                    text = "Load",
                    style =
                {
                    width = new Length(200, LengthUnit.Pixel),
                    height = new Length(200, LengthUnit.Pixel),
                }
                };
                mainMenu.Add(load);

                Button create = new Button(CreateTree)
                {
                    text = "Create",
                    style =
                {
                    width = new Length(200, LengthUnit.Pixel),
                    height = new Length(200, LengthUnit.Pixel),
                }
                };
                mainMenu.Add(create);
            }
        }

        #region Button Actions
        void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Open Dialogue Tree", "", "asset");

            filePath = filePath.Replace(Application.dataPath, "Assets");
            DialogueTree tempTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(filePath);

            if(tempTree != null)
            {
                currentTree = tempTree;

                OpenGraphView(rootVisualElement);
            }
            else
            {
                Debug.Log("Tree was null");
            }
        }

        void Save()
        {
            graphView.SaveGraph(graphData);
        }

        void CreateTree()
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Create Dialogue Tree", "DialogueTree", "asset", "Create a dialogue tree");
            if (savePath != string.Empty)
            {
                Debug.Log("Save path is: " + savePath);

                DialogueTree tempTree = CreateInstance<DialogueTree>();
                AssetDatabase.CreateAsset(tempTree, savePath);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();

                Selection.activeObject = tempTree;

                currentTree = tempTree;
                OpenGraphView(rootVisualElement);
            }
        }

        void ToggleAutoCompile()
        {

        }

        #endregion

        DSG_TreeGraphData GetGraphData()
        {
            var treeGraphData = AssetDatabase.LoadAssetAtPath<DSG_TreeGraphData>(graphDataPath + "/" + graphDataName + ".asset");
            if (!treeGraphData)
            {
                treeGraphData = CreateInstance<DSG_TreeGraphData>();

                string[] folders = (graphDataPath).Split('/');
                string currentFolder = folders[0];

                for (int i = 1; i < folders.Length; i++)
                {
                    if (!AssetDatabase.IsValidFolder(currentFolder + "/" + folders[i]))
                    {
                        AssetDatabase.CreateFolder(currentFolder, folders[i]);
                    }
                    currentFolder += "/" + folders[i];
                }

                AssetDatabase.CreateAsset(treeGraphData, graphDataPath + "/" + graphDataName + ".asset");
                AssetDatabase.SaveAssets();
            }

            Debug.Log("Got graph data " + treeGraphData);

            return treeGraphData;
        }
    }
}
