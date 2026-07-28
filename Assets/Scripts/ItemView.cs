using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    private Image _image;
    private GridManager _gridManager;
    private CellView _cellView;
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
        CellView CellView = _originalParentCell.GetComponent<CellView>();
        GridCell Cell = GridManager.Instance.GetCell(CellView.row, CellView.column);
        CellView targetCellView = GetCellUnderItem(eventData);

        if (targetCellView == null || targetCellView == CellView)
        {
            ReturnToOriginalCell();
            return;
        }

        GridCell targetCell = GridManager.Instance.GetCell(targetCellView.row, targetCellView.column);

        if (!targetCell.isTaken)
            MoveToCell(Cell, targetCell, targetCellView);
        else
        {
            if (targetCell.itemData.level == itemData.level && targetCell.itemData.type == itemData.type && itemData.isMergeable)
            {
                // test
                Destroy(gameObject);
                Destroy(targetCell.itemView.gameObject);
            }
            else
                ReturnToOriginalCell();
        }
    }

    private CellView GetCellUnderItem(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            CellView cellView = result.gameObject.GetComponent<CellView>();
            if (cellView != null)
                return cellView;
        }
        return null;
    }

    private void MoveToCell(GridCell originCell, GridCell targetCell, CellView targetCellView)
    {
        originCell.isTaken = false;
        originCell.itemData = null;
        originCell.itemView = null;

        targetCell.isTaken = true;
        targetCell.itemData = itemData;
        targetCell.itemView = this;

        transform.SetParent(targetCellView.transform, false);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    private void ReturnToOriginalCell()
    {
        transform.SetParent(_originalParentCell, false);
        _rectTransform.anchoredPosition = _originalPosition;
    }
}
