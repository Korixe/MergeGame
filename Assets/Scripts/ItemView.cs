using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    public ItemData itemData;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetItemData(ItemData data)
    {
        itemData = data;
        _image.sprite = itemData.prefab;
    }

}
