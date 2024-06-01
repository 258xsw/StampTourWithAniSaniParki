using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 initialPosition;
    private Camera mainCamera; // ī�޶� ���� �߰�

    private void Awake()
    {
        initialPosition = transform.position;
        mainCamera = Camera.main; // ���� ī�޶� �Ҵ�
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // �̵� ���� �� �ʱ� ��ġ�� �����մϴ�.
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��ġ ��ǥ�� ���� ��ǥ�� ��ȯ�Ͽ� ������Ʈ�� �̵��մϴ�.
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        newPosition.z = initialPosition.z; // z �� �� ����
        transform.position = newPosition;
    }
}
