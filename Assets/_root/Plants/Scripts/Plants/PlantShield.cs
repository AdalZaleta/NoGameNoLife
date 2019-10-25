using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Shield", order = 13)]

public class PlantShield : PlantSO
{
    public float effectDuration;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + effectDuration * _multiplier + " shield time given to " + _target.name);
    }
}
