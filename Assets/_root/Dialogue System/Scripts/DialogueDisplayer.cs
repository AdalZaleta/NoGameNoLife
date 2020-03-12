using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject displayHolder;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private Button buttonPrefab;

    private List<Button> buttonOptions = new List<Button>();

    #region Text displayer
    public void ShowDisplay()
    {
        displayHolder.SetActive(true);
    }

    public void HideDisplay()
    {
        displayHolder.SetActive(false);
    }

    public void SetNameText(string _name)
    {
        if(nameText)
        {
            nameText.text = _name;
        }
    }

    public void SetBodyText(string _text)
    {
        if(bodyText)
        {
            bodyText.text = _text;
        }
    }
    #endregion

    #region Buttons
    public void RemoveAllButtons()
    {
        foreach (var b in buttonOptions)
            b.gameObject.SetActive(false); //Lo oculto para usar el pool
    }

    public void AddButtonOption(string _text, Action callback)
    {
        Button b;

        bool pooled = false;
        int id = 0;
        for(int i = 0; i < buttonOptions.Count; i++)
        {
            if(!buttonOptions[i].gameObject.activeSelf)
            {
                pooled = true;
                id = i;
                break;
            }
        }

        if (!pooled) //Si no se pudo usar el pool, instancio uno nuevo
        {
            b = Instantiate(buttonPrefab, buttonHolder.transform).GetComponent<Button>();
            buttonOptions.Add(b);
        }
        else
        {
            b = buttonOptions[id];   
        }

        b.gameObject.SetActive(true);
        b.onClick.AddListener(() => { callback?.Invoke(); });

        TextMeshProUGUI tmpro = b.GetComponentInChildren<TextMeshProUGUI>();  //Busco tanto un Text mesh pro como un Text normal
        if(tmpro)
        {
            tmpro.text = _text;
        }
        else
        {
            Text t = b.GetComponentInChildren<Text>();
            if(t)
            {
                t.text = _text;
            }
        }
    }
    #endregion
}
