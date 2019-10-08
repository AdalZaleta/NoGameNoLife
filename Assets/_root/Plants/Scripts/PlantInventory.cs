using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInventory : MonoBehaviour
{
    public List<PlantSO> plants = new List<PlantSO>();
    public List<PlantSO> plantBuffer = new List<PlantSO>();

    // Testing Only
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UsePlants(plantBuffer);
        }
    }
    // ~Testing Only

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

    public void UsePlants(List<PlantSO> _plants)
    {
        if (_plants.Count > 0)
        {
            bool canCombo = true;

            System.Type plantType = _plants[0].GetType();

            for (int i = 0; i < _plants.Count; i++)
            {
                if (_plants[i].GetType() != plantType)
                {
                    canCombo = false;
                    Debug.Log("Can't combine different types of plants");
                    break;
                }
            }

            if (canCombo)
            {
                Debug.Log("Using Plant Combo");
                _plants[0].Use(this.gameObject, _plants.Count);
                Debug.Log("Clearing Buffer");
                plantBuffer.Clear();
            }

        } else
        {
            Debug.Log("No Plants Buffered");
        }
    }
}
