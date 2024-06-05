using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Canvas canvas;
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
        Debug.Log("OnDrag");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, Camera.main, out Vector2 pos);
        transform.position = canvas.transform.TransformPoint(pos);


        // newPosition.z = initialPosition.z; // z �� �� ����
        // transform.position = newPosition;
    }
}
