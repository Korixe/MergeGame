using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderView : MonoBehaviour
{
    public Button completeOrderButton;
    public TextMeshProUGUI orderNameText;
    public TextMeshProUGUI orderValueText;

    private ActiveOrder _activeOrder;

    public void SetActiveOrder(ActiveOrder activeOrder)
    {
        _activeOrder = activeOrder;
        orderNameText.text = _activeOrder.orderData.orderName;
        orderValueText.text = $"Value: {_activeOrder.orderData.orderValue}";

        completeOrderButton.onClick.RemoveAllListeners();
        completeOrderButton.onClick.AddListener(OnCompleteOrderButtonClicked);
    }

    private void OnCompleteOrderButtonClicked()
    {
        bool orderCompleted = OrderManager.Instance.CompleteOrder(_activeOrder);
        if (orderCompleted)
        {
            Debug.Log($"Order {_activeOrder.orderData.orderName} completed!");
            OrderManager.Instance.activeOrders.Remove(_activeOrder);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Order {_activeOrder.orderData.orderName} could not be completed");
        }
    }

}
