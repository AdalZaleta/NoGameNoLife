using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantMenu_Item : MonoBehaviour
{
    public List<int> invIndex = new List<int>();
    private PlantMenu plantMenu;
    private Button btn;
    private bool buffered = false;
    public Image plantImg;
    public TextMeshProUGUI plantName;
    private int ammount;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ProcessSelection);
    }

    public void SetPlantData(int _index, Sprite _spr, string _name, PlantMenu _menu)
    {
        invIndex.Clear();
        invIndex[0] = _index;
        plantImg.sprite = _spr;
        plantName.text = _name;
        plantMenu = _menu;
        ammount = invIndex.Count;
    }

    public void SetIndex(int _index, int _i)
    {
        if (_i < invIndex.Count)
            invIndex[_i] = _index;
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
        // TODO: Change Inventory to work with plant amounts instead of individually
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
