using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSG {
    public class DSG_TreeGraphData : ScriptableObject
    {
        [System.Serializable]
        public class GraphData
        {
            public DialogueTree tree;
            public List<System.Type> nodeTypes;
            public List<Rect> nodePositions;

            public bool typesAreNull;

            public GraphData(DialogueTree tree, List<Rect> nodePositions, List<System.Type> nodeTypes)
            {
                this.tree = tree;
                this.nodePositions = nodePositions;
                this.nodeTypes = nodeTypes;
                Debug.Log("GraphData constructor");
                if (this.nodeTypes == null) Debug.Log("Types are null");
                else Debug.Log("Types aren't null");
            }
        }

        public List<GraphData> data;

        public DSG_TreeGraphData()
        {
            data = new List<GraphData>();
        }

        public void SaveGraph(DSG_GraphView graphView)
        {
            Debug.Log("Graph Data saving graph");
            List<Rect> positions = new List<Rect>();
            List<System.Type> types = new List<System.Type>();
            for(int i = 0; i < graphView.DSGNodes.Count; i++)
            {
                positions.Add(graphView.DSGNodes[i].GetPosition());
                types.Add(graphView.DSGNodes[i].GetNodeType());
            }

            bool saved = false;

            for(int i = 0; i < data.Count; i++)
            {
                if(data[i].tree == graphView.tree)
                {
                    if(types == null)
                    {
                        Debug.Log("Types is null");
                    }
                    else
                    {
                        Debug.Log("Types is not null");
                    }
                    data[i] = new GraphData(graphView.tree, positions, types);
                    saved = true;
                }
            }

            if(!saved)
                data.Add(new GraphData(graphView.tree, positions, types));

            Cleanup();
        }

        public GraphData GetDataFor(DialogueTree tree)
        {
            foreach(var d in data)
            {
                if (d.tree == tree)
                    return d;
            }

            return null;
        }

        private void Cleanup()
        {
            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (data[i].tree == null)
                    data.RemoveAt(i);
            }
        }

        private void OnValidate()
        {
            foreach(var d in data)
            {
                d.typesAreNull = d.nodeTypes == null;
            }
        }
    }
}