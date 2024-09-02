using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private GameObject currentDragTarget;

    /// <summary>
    /// ��̬�ж���Ʒ���е���Ʒ�Ƿ����
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool Is_Null(GameObject target)
    {
        if (target.name == "Item")
        {
            return target.transform.GetComponentInChildren<Image>().color.a == 0;
        }
        else if (target.name == "Num")
        {
            return (target.transform.GetComponentInChildren<TextMeshProUGUI>().text == "0" ||
                    target.transform.GetComponentInChildren<TextMeshProUGUI>().text == null);
        }
        else
        {
            return (target.transform.GetComponentInChildren<Image>().color.a == 0 ||
                    target.transform.GetComponentInChildren<TextMeshProUGUI>().text == "0" ||
                    target.transform.GetComponentInChildren<TextMeshProUGUI>().text == null);
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentDragTarget = eventData.pointerCurrentRaycast.gameObject;
        if (Is_Null(currentDragTarget))
        {
            Debug.Log("Ϊ�ղ����϶�");
            currentDragTarget = null;
            return;
        }

        rectTransform = currentDragTarget.transform.parent.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            originalPosition = rectTransform.anchoredPosition;
        }
        else
        {
            Debug.LogWarning("�޷���ȡ RectTransform");
        }

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDragTarget == null) return;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDragTarget == null) return;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(target);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (target == null)
        {
            Debug.LogError("�����϶�ʱ�·�����Ϊ��");
            rectTransform.anchoredPosition = originalPosition;
            return;
        }

        if (IsOverUseBar(target)) // �ڱ������ƶ�
        {
            RectTransform targetTransform = target.transform.parent.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = targetTransform.anchoredPosition;
            target.transform.parent.GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
        else if (!IsOverUseBar(target)) // ��������϶�ʱ�����κα���������
        {
            Debug.Log("δ���ڱ���������" + originalPosition);
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private bool IsOverUseBar(GameObject eventData)
    {
        return eventData.CompareTag("BagGrid");
    }
}


