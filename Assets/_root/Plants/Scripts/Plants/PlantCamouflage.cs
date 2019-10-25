using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Camouflage", order = 11)]

public class PlantCamouflage : PlantSO
{
    public float effectDuration;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + effectDuration * _multiplier + " active camo time given to " + _target.name);
    }
}
