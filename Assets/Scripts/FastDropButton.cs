using UnityEngine;
using UnityEngine.EventSystems;

public class FastDropButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Movement movement;

    public void OnPointerDown(PointerEventData eventData)
    {
        movement?.FastDropOn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movement?.FastDropOff();
    }
}
