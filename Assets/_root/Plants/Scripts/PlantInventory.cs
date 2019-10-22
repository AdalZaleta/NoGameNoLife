using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInventory : MonoBehaviour
{
    public List<PlantSO> plants = new List<PlantSO>();
    public List<PlantSO> plantBuffer = new List<PlantSO>();

    public void AddPlant(PlantSO _plant)
    {
        plants.Add(_plant);
    }

    public void BufferPlant(PlantSO _plant)
    {
        if (plantBuffer.Count < 3)
        {
            plantBuffer.Add(_plant);
            plants.Remove(_plant);
        }
    }

    public void DebufferPlant(PlantSO _plant)
    {
        plantBuffer.Remove(_plant);
        plants.Add(_plant);
    }

    public void BufferPlant(int _plantIndex)
    {
        if (plantBuffer.Count < 3)
        {
            plantBuffer.Add(plants[_plantIndex]);
            plants.RemoveAt(_plantIndex);
        }
    }

    public void DebufferPlant(int _plantIndex)
    {
        plants.Add(plantBuffer[_plantIndex]);
        plantBuffer.Remove(plants[_plantIndex]);
    }

    public void DebufferPlant()
    {
        plants.Add(plantBuffer[plantBuffer.Count - 1]);
        plantBuffer.RemoveAt(plantBuffer.Count - 1);
    }

    public bool UsePlants()
    {
        if (plantBuffer.Count > 0)
        {
            bool canCombo = true;

            System.Type plantType = plantBuffer[0].GetType();   

            for (int i = 0; i < plantBuffer.Count; i++)
            {
                if (plantBuffer[i].GetType() != plantType)
                {
                    canCombo = false;
                    Debug.Log("Can't combine different types of plants");
                    break;
                }
            }

            if (canCombo)
            {
                Debug.Log("Using Plant Combo");
                plantBuffer[0].Use(this.gameObject, plantBuffer.Count);
                Debug.Log("Clearing Buffer");
                plantBuffer.Clear();
                return true;
            }

        } else
        {
            Debug.Log("No Plants Buffered");
            return false;
        }
        return false;
    }
}
