using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantMenu_Item : MonoBehaviour
{
    private int invIndex;
    private PlantMenu plantMenu;
    private Button btn;
    private bool buffered = false;
    public Image plantImg;
    public TextMeshProUGUI plantName;
    public TextMeshProUGUI indexTxt;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ProcessSelection);
    }

    public void SetPlantData(int _index, Sprite _spr, string _name, PlantMenu _menu)
    {
        invIndex = _index;
        plantImg.sprite = _spr;
        plantName.text = _name;
        plantMenu = _menu;
        indexTxt.text = invIndex.ToString();
    }

    public void SetIndex(int _index)
    {
        invIndex = _index;
        indexTxt.text = invIndex.ToString();
    }

    public void SetSprite(Sprite _spr)
    {
        plantImg.sprite = _spr;
    }

    public void SetName(string _name)
    {
        plantName.text = _name;
    }

    public void ProcessSelection()
    {
        if (buffered)
            Debuffer();
        else
            if (plantMenu.inventory.plantBuffer.Count < 3)
                Buffer();
    }

    public void Buffer()
    {
        plantMenu.inventory.BufferPlant(invIndex);
        plantMenu.BufferPlant(this);
        buffered = true;
    }

    public void Debuffer()
    {
        plantMenu.inventory.DebufferPlant(invIndex);
        plantMenu.DebufferPlant(this);
        buffered = false;
    }

}
