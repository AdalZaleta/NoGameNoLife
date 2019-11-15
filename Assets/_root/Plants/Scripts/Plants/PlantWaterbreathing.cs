using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantWaterbreathing", menuName = "Plants/Waterbreathing", order = 10)]

public class PlantWaterbreathing : PlantSO
{
    public float effectDuration;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + effectDuration * _multiplier + " waterbreathing time given to " + _target.name);
    }
}
