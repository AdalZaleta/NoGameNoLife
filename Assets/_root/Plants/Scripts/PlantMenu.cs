using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMenu : MonoBehaviour
{
    public PlantInventory inventory;
    public Transform plantPanel;
    public Transform plantBufferPanel;
    public GameObject plantItemPfb;

    [SerializeField]
    public List<PlantMenu_Item> plantItems = new List<PlantMenu_Item>();
    [SerializeField]
    public List<PlantMenu_Item> plantItemsBuffered = new List<PlantMenu_Item>();

    public void FillPlantMenuItems()
    {
        for (int i = 0; i < inventory.plants.Count; i++)
        {
            GameObject plant = GameObject.Instantiate(plantItemPfb, plantPanel);
            plant.GetComponent<PlantMenu_Item>().SetPlantData(i, inventory.plants[i].plantSpr, inventory.plants[i].plantName, this);
            plantItems.Add(plant.GetComponent<PlantMenu_Item>());
        }
    }

    public void BufferPlant(PlantMenu_Item _plant)
    {
        plantItemsBuffered.Add(_plant);
        plantItems.Remove(_plant);
        MovePlantToPanel(_plant.gameObject, plantBufferPanel);
        UpdatePlantsIndex();
    }

    public void DebufferPlant(PlantMenu_Item _plant)
    {
        plantItems.Add(_plant);
        plantItemsBuffered.Remove(_plant);
        MovePlantToPanel(_plant.gameObject, plantPanel);
        UpdatePlantsIndex();
    }

    public void MovePlantToPanel(GameObject _plant, Transform _panel)
    {
        _plant.transform.SetParent(_panel, false);
    }

    public void UpdatePlantsIndex()
    {
        for (int i = 0; i < inventory.plants.Count; i++)
        {
            plantItems[i].SetIndex(i);
        }
        for (int i = 0; i < inventory.plantBuffer.Count; i++)
        {
            plantItemsBuffered[i].SetIndex(i);
        }
    }

    public void UsePlants()
    {
        if (inventory.UsePlants())
        {
            for (int i = 0; i < plantItemsBuffered.Count; i++)
            {
                Destroy(plantItemsBuffered[i].gameObject);
            }
            plantItemsBuffered.Clear();
        }
    }

    private void Awake()
    {
        FillPlantMenuItems();
    }
}
