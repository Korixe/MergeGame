using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public ActiveOrders activeOrders;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddActiveOrder(OrderData order)
    {
        if (activeOrders == null)
            activeOrders = new ActiveOrders();

        if (activeOrders.activeOrders == null)
            activeOrders.activeOrders = new List<OrderData>();

        if (activeOrders.requiredItems == null)
            activeOrders.requiredItems = new List<ItemData>();

        if (activeOrders.collectedItems == null)
            activeOrders.collectedItems = new List<ItemData>();

        activeOrders.activeOrders.Add(order);
        activeOrders.requiredItems.AddRange(order.requiredItems);
    }

    public bool CompleteOrder(OrderData order)
    {
        if (activeOrders == null || activeOrders.activeOrders == null)
            return false;

        if (activeOrders.activeOrders.Contains(order))
        {
            activeOrders.activeOrders.Remove(order);
            foreach (var item in order.requiredItems)
            {
                activeOrders.requiredItems.Remove(item);
                activeOrders.collectedItems.Remove(item);
            }
        }
        return true;
    }

    public bool IsOrderCompleted(OrderData order)
    {
        if (activeOrders == null || activeOrders.activeOrders == null)
            return false;

        return !activeOrders.activeOrders.Contains(order);
    }
}
