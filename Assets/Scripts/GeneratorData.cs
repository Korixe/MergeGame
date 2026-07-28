using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorData", menuName = "Scriptable Objects/GeneratorData")]
public class GeneratorData : ItemData
{
    public ItemData[] possibleItems;
    public int maxSpawns;
    public float cooldownTime;
}
