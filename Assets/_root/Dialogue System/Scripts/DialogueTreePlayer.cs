using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTreePlayer : MonoBehaviour
{
    public DialogueTree tree;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            tree?.Start();
        }

        tree.UpdateDialogue();
    }
}
