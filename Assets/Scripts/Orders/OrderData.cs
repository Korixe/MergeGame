using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "Scriptable Objects/OrderData")]
public class OrderData : ScriptableObject
{
    public string orderID;
    public string orderName;
    public int orderValue;
    public List<OrderRequirements> requirements;
    
}
