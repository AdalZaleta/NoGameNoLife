using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantRevive", menuName = "Plants/Strength", order = 4)]

public class PlantStrength : PlantSO
{
    public float strAmount;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + strAmount * _multiplier + " strength given to " + _target.name);
    }
}
