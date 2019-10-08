using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Plants/MainPlant", order = 1)]

public class PlantSO : ScriptableObject
{
    public string plantName;
    public int rarity;

    public virtual void Use(GameObject _target, int _multiplier) { }
}
