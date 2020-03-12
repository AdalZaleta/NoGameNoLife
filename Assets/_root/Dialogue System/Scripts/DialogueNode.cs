using UnityEngine;

[System.Serializable]
public abstract class DialogueNode : ScriptableObject
{
    public DialogueNode nextNode;

    public abstract void OnNodeEnter();
    public abstract void OnNodeStay(float _deltaTime);
    public abstract void OnNodeExit();
    public abstract void NodeInterac(int _value);
}
