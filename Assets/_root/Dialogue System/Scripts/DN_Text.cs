using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextModifier
{
    public int characterIndexStart;
    public int characterIndexEnd;
    public float speedModifier;

    /* Posibilidades para despues:
     * cambio del font a italic o bold
     * efectos en las letras
     * cambio de font
     */

    public TextModifier(int characterIndexStart, int characterIndexEnd, float speedModifier)
    {
        this.characterIndexStart = characterIndexStart;
        this.characterIndexEnd = characterIndexEnd;
        this.speedModifier = speedModifier;
    }
}

[System.Serializable]
public class DN_Text : DialogueNode
{   
    DialogueDisplayer dialogueDisplayer; //TODO: cambiar esto por el que se vaya a usar, o ver si lo mando en el node enter o que rollo
    public DialogueConfig globalDialogueConfig;
    public DialogueConfig characterDialogueConfig;
    public string text;
    public bool overrideName = false;
    public string overridenName;       //TODO: checar la accesibilidad de estas variables

    private State state;
    private TextModifier currentModifier;
    private int currentCharacterIndex;
    private float textDisplayProgress; //va de 0 hasta tamaño del texto

    [SerializeField] List<TextModifier> textSpeedModifiers;

    #region Constructos and accesors
    public DN_Text(DialogueConfig globalDialogueConfig)
    {
        this.globalDialogueConfig = globalDialogueConfig;
    }

    public List<TextModifier> TextModifiers
    {
        get
        {
            return textSpeedModifiers;
        }
        private set { }
    }
    #endregion

    #region Abstract class implementation
    public override void NodeInterac(int _value)
    {
        switch (state)
        {
            case State.WRITING:
                SkipToFullText();
                break;
            case State.WAITING:
                break;
        }
    }

    public override void OnNodeEnter()
    {
        dialogueDisplayer.ShowDisplay();
        dialogueDisplayer.SetNameText(characterDialogueConfig.Name);
        state = State.WRITING;
    }

    public override void OnNodeStay(float _deltaTime)
    {
        switch (state)
        {
            case State.WRITING:
                UpdateTextDisplay(_deltaTime);
                break;
            case State.WAITING:
                break;
        }
    }

    public override void OnNodeExit()
    {
        state = State.WRITING;
        currentCharacterIndex = 0;
        textDisplayProgress = 0;
        /* Talvez hacer:
         * Que se agregue el texto a un backlog,
         */
    }
    #endregion

    void SkipToFullText()
    {
        textDisplayProgress = text.Length;
        currentCharacterIndex = text.Length;
        state = State.WAITING;

        /* Talvez hacer:
         * Que se espere un ratito antes de entrar al estado de waiting
         */
    }

    void GoToNextNode()
    {
        //TODO: hacer que se mueva al siguiente nodo
    }

    void UpdateTextDisplay(float _deltaTime)
    {
        TextModifier tempTm = GetModifierAt(currentCharacterIndex); //Obteniendo el modificador de este momento
        float speedMod = 1;
        if (tempTm != null)
        {
            speedMod = tempTm.speedModifier;
        }

        if (globalDialogueConfig) speedMod *= globalDialogueConfig.dialogueSpeed;
        if (characterDialogueConfig) speedMod *= characterDialogueConfig.dialogueSpeed;

        textDisplayProgress += speedMod * _deltaTime; //Aumentando el progreso segun todas las velocidades que lo afectan

        currentCharacterIndex = Mathf.FloorToInt(textDisplayProgress);

        string currentText = text.Substring(0, currentCharacterIndex); //Creo el string que se va a mandar al text displayer

        dialogueDisplayer.SetBodyText(currentText);

        if (currentCharacterIndex >= text.Length)
        {
            state = State.WAITING;
        }
    }

    #region Managing text modifiers
    public TextModifier GetModifierAt(int _characterIndex)
    {
        foreach(var tm in textSpeedModifiers)
        {
            if (tm.characterIndexStart < _characterIndex && tm.characterIndexEnd > _characterIndex)
                return tm;
        }
        return null;
    }

    public void RemoveModifierAt(int _i)
    {
        if (_i < textSpeedModifiers.Count)
            textSpeedModifiers.RemoveAt(_i);
    }

    public void RemoveModifier(int _characterIndex)
    {
        for (int i = textSpeedModifiers.Count - 1; i >= 0; i--)
        {
            if (textSpeedModifiers[i].characterIndexStart < _characterIndex && textSpeedModifiers[i].characterIndexEnd > _characterIndex)
            {
                textSpeedModifiers.RemoveAt(i);
            }
        }
    }
    #endregion

    private enum State
    {
        WRITING,
        WAITING
    }
}
