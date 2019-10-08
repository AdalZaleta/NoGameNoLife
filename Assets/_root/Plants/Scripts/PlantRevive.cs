using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantRevive", menuName = "Plants/Revive", order = 3)]

public class PlantRevive : PlantSO
{
    public int charges;

    public override void Use(GameObject _target, int multiplier)
    {
        if (charges > 0)
        {
            Debug.Log("Reviving " + _target.name);
        }
    }
}
