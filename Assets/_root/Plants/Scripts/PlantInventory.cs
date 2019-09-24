using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInventory : MonoBehaviour
{
    private List<PlantSO> plants = new List<PlantSO>();
    private List<PlantSO> plantCombo = new List<PlantSO>();

    public void AddPlant(PlantSO _plant)
    {
        plants.Add(_plant);
    }

    public void ComboPlant(PlantSO _plant)
    {
        plantCombo.Add(_plant);
    }

    private void ExecutePlantCombo()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
