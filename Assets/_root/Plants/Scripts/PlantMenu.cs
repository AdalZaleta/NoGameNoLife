using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMenu : MonoBehaviour
{
    public PlantInventory inventory;
    public Transform plantPanel;
    public GameObject plantItemPfb;

    private List<PlantMenu_Item> plantItems = new List<PlantMenu_Item>();

    public void FillPlantMenuItems()
    {
        for (int i = 0; i < inventory.plants.Count; i++)
        {
            GameObject plant = GameObject.Instantiate(plantItemPfb, plantPanel);
            plant.GetComponent<PlantMenu_Item>().SetPlantData(i, inventory.plants[i].plantSpr, inventory.plants[i].plantName);
        }
    }

}
