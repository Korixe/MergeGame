using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> ItemData;

    public ItemData GetItemByID(string id)
    {
        return ItemData.Find(item => item.itemID == id);
    }
}