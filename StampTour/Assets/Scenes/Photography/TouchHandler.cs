using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // �̵� ���� �� �ʱ� ��ġ�� �����մϴ�.
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡���ϴ� ���� ������Ʈ�� �̵��մϴ�.
        transform.position = eventData.position;
    }
}
