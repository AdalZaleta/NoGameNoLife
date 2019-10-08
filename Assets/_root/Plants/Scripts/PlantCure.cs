using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Cure", order = 2)]

public class PlantCure : PlantSO
{
    public int curePercent;

    public override void Use(GameObject _target, int multiplier)
    {
        Debug.Log("Curing " + _target.name + " " + curePercent * multiplier + "%");
    }
}
