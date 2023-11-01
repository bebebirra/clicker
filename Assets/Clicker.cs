using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject ExternalHandlerGObject;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        RectTransform rectTransform = transform as RectTransform;

        Vector2 correction = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.y);

        Vector3 pointClicked = transform.InverseTransformPoint(pointerEventData.position);
                
        Vector3 correctedPointClicked = transform.InverseTransformPoint(pointerEventData.position);
        correctedPointClicked.x = (int)correctedPointClicked.x - correction.x;
        correctedPointClicked.y = (int)correctedPointClicked.y - correction.y;

        correctedPointClicked = new Vector2((int)correctedPointClicked.x, (int)correctedPointClicked.y);

        Debug.Log($"{name} Game Object Clicked @ {pointClicked.x}, {pointClicked.y}({correctedPointClicked.x}, {correctedPointClicked.y})");

        if (ExternalHandlerGObject != null)
        {
            ExternalHandlerGObject.GetComponent<IExternalClickerHandler>()?
                .HandleOnPointerClick(transform, pointerEventData, correctedPointClicked);
        }
    }
}