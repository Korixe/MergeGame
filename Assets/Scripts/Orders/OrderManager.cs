using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public OrderData[] possibleOrders;
    public int maxActiveOrders = 10;
    public List<ActiveOrder> activeOrders = new List<ActiveOrder>();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // fill empty slots with random orders
    }

    public OrderData AddActiveOrder(OrderData order)
    {
        if (possibleOrders.Length == 0)
            return null;

        int randIndex = Random.Range(0, possibleOrders.Length);
        return possibleOrders[randIndex];
    }

    public bool CompleteOrder(ActiveOrder order)
    {
        Dictionary<ItemData, List<GridCell>> foundCells = new Dictionary<ItemData, List<GridCell>>();

        foreach(OrderRequirements requirement in order.orderData.requirements)
        {
            // get cell with item

            if (cellsWithItem.Count < requirement.requiredAmount)
                return false;

            foundCells[requirement.itemData] = cellsWithItem;
        }
        
        foreach (var item in foundCells)
        {
            foreach (GridCell cell in item.Value)
            {
                // remove item from cell
            }
        }
        return true;
    }
}
