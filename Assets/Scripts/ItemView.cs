using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public ItemData itemData;
    private Image _image;
    private Vector2 _originalPosition;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _originalParentCell;
    private int _itemUsed = 0;
    private bool _isOnCooldown = false;



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
        CellView prevCellView = _originalParentCell.GetComponent<CellView>();
        GridCell cell = GridManager.Instance.GetCell(prevCellView.row, prevCellView.column);
        CellView targetCellView = GetCellUnderItem(eventData);

        if (targetCellView == null || targetCellView == prevCellView)
        {
            ReturnToOriginalCell();
            return;
        }

        GridCell targetCell = GridManager.Instance.GetCell(targetCellView.row, targetCellView.column);

        if (!targetCell.isTaken)
            MoveToCell(cell, targetCell, targetCellView);
        else
        {
            if (targetCell.itemData.level == itemData.level && targetCell.itemData.itemName == itemData.itemName && itemData.isMergeable)
            {
                ItemData nextItem = itemData.nextLevelItemData;
                cell.isTaken = false;
                cell.itemData = null;
                cell.itemView = null;

                Destroy(gameObject);
                Destroy(targetCell.itemView.gameObject);

                targetCell.isTaken = false;
                targetCell.itemData = null;
                targetCell.itemView = null;

                if (nextItem != null)
                {
                    GridManager.Instance.SpawnItemInCell(targetCell, targetCellView, nextItem);
                }
            }
            else
                SwapItems(cell, targetCell, prevCellView, targetCellView);
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

    private void SwapItems(GridCell originCell, GridCell targetCell, CellView originCellView, CellView targetCellView)
    {
        ItemView targetItemView = targetCell.itemView;
        ItemData targetItemData = targetCell.itemData;

        originCell.itemData = targetItemData;
        originCell.itemView = targetItemView;

        targetCell.itemData = itemData;
        targetCell.itemView = this;

        targetItemView.transform.SetParent(originCellView.transform, false);
        targetItemView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        transform.SetParent(targetCellView.transform, false);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;

        if (itemData is GeneratorData generatorData)
        {
            if (generatorData.possibleItems.Length > 0 && !_isOnCooldown)
            {
                int randIndex = Random.Range(0, generatorData.possibleItems.Length);
                ItemData randomItem = generatorData.possibleItems[randIndex];

                GridCell targetCell = null;
                for (int i = 0; i < GridManager.Instance.rows; i++)
                {
                    for (int j = 0; j < GridManager.Instance.columns; j++)
                    {
                        if (GridManager.Instance.IsCellFree(i, j))
                        {
                            targetCell = GridManager.Instance.GetCell(i, j);
                            break;
                        }
                    }
                    if (targetCell != null)
                        break;
                }

                if (targetCell != null)
                {
                    GridManager.Instance.SpawnItemInCell(targetCell, targetCell.cellView, randomItem);
                    _itemUsed++;

                    if (_itemUsed >= generatorData.maxSpawns)
                    {
                        _isOnCooldown = true;
                        StartCoroutine(StartCooldown(generatorData.cooldownTime));
                    }
                }
                else
                    Debug.Log("Can't spawn an item, no free cells available");
            }
        }
    }

    IEnumerator StartCooldown(float cooldownDuration)
    {
        float timer = 0f;
        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _itemUsed = 0;
        _isOnCooldown = false;
    }
}