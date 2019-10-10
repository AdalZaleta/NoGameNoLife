using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantMenu_Item : MonoBehaviour
{
    private int invIndex;
    public Image plantImg;
    public TextMeshProUGUI plantName;

    public void SetPlantData(int _index, Sprite _spr, string _name)
    {
        invIndex = _index;
        plantImg.sprite = _spr;
        plantName.text = _name;
    }

    public void SetIndex(int _index)
    {
        invIndex = _index;
    }

    public void SetSprite(Sprite _spr)
    {
        plantImg.sprite = _spr;
    }

    public void SetName(string _name)
    {
        plantName.text = _name;
    }
}
