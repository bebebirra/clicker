using UnityEngine;
using UnityEngine.EventSystems;

public interface IExternalClickerHandler
{
    public void HandleOnPointerClick(Transform sender, PointerEventData pointerEventData, Vector2 correctedPoint);
}
