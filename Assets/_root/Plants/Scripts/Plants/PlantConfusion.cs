using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantConfusion", menuName = "Plants/Confusion", order = 15)]

public class PlantConfusion : PlantSO
{
    public float effectDuration;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + effectDuration * _multiplier + " confusion time given to " + _target.name);
    }
}
