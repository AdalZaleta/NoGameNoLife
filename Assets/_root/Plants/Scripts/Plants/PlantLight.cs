using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantLight", menuName = "Plants/Light", order = 11)]

public class PlantLight : PlantSO
{
    public float effectDuration;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + effectDuration * _multiplier + " light time given to " + _target.name);
    }
}
