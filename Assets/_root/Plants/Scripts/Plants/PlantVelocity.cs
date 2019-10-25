using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Velocity", order = 6)]

public class PlantVelocity : PlantSO
{
    public float velocityAmount;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + velocityAmount * _multiplier + " velocity given to " + _target.name);
    }
}
