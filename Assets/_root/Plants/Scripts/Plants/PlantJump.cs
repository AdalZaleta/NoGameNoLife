using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Jump", order = 8)]

public class PlantJump : PlantSO
{
    public float jumpBoostStrength;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + jumpBoostStrength * _multiplier + " jump boost given to " + _target.name);
    }
}
