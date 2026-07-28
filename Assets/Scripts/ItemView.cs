using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    private Image _image;
    private Vector2 _originalPosition;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _originalParentCell;



    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetItemData(ItemData data)
    {
        itemData = data;
        _image.sprite = itemData.sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParentCell = transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }

}
