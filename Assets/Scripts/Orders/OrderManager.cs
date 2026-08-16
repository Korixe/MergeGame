using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public OrderData[] possibleOrders;
    public int maxActiveOrders = 10;
    public List<ActiveOrder> activeOrders = new List<ActiveOrder>();

    public GameObject orderViewPrefab;
    public Transform ordersContainer;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FillEmptyOrderSlots();
        RefreshOrderViews();
    }

    private OrderData AddActiveOrder(OrderData order)
    {
        if (possibleOrders.Length == 0)
            return null;

        int randIndex = Random.Range(0, possibleOrders.Length);
        return possibleOrders[randIndex];
    }

    private void FillEmptyOrderSlots()
    {
        while (activeOrders.Count < 1) // Change this to maxActiveOrders later
        {
            OrderData newOrder = AddActiveOrder(null);
            if (newOrder != null)
                activeOrders.Add(new ActiveOrder(newOrder));
        }
    }

    public bool CompleteOrder(ActiveOrder order)
    {
        Dictionary<ItemData, List<GridCell>> foundCells = new Dictionary<ItemData, List<GridCell>>();

        foreach(OrderRequirements requirement in order.orderData.requirements)
        {
            List<GridCell> cellsWithItem = GridManager.Instance.GetCellsWithItem(requirement.itemData, requirement.requiredAmount);

            if (cellsWithItem == null)
                return false;

            foundCells[requirement.itemData] = cellsWithItem;
        }
        
        foreach (var item in foundCells)
        {
            foreach (GridCell cell in item.Value)
                GridManager.Instance.RemoveItemFromCell(cell);
        }

        CurrencyManager.Instance.AddCurrency(order.orderData.orderValue);

        activeOrders.Remove(order);
        FillEmptyOrderSlots();
        RefreshOrderViews();

        return true;
    }

    private void RefreshOrderViews()
    {
        foreach (Transform child in ordersContainer)
            Destroy(child.gameObject);

        foreach (ActiveOrder order in activeOrders)
        {
            GameObject spawnedView = Instantiate(orderViewPrefab, ordersContainer);
            OrderView orderView = spawnedView.GetComponent<OrderView>();
            orderView.SetActiveOrder(order);
        }
    }
}
