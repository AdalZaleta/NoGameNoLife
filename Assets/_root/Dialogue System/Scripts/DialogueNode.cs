using UnityEngine;

[System.Serializable]
public class DialogueNode : ScriptableObject
{
    public DialogueNode nextNode;

    public virtual void OnNodeEnter() { }
    public virtual void OnNodeStay(float _deltaTime) { }
    public virtual void OnNodeExit() { }
    public virtual void NodeInteract(int _value) { }
}
