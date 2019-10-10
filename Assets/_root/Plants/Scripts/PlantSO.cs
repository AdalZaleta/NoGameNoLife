using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Plants/MainPlant", order = 1)]

public class PlantSO : ScriptableObject
{
    [Header("Inventory")]
    public string plantName;
    public int rarity;

    [Header("Visuals")]
    public Sprite plantSpr;

    public virtual void Use(GameObject _target, int _multiplier) { }
}
