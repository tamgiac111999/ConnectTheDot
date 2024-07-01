using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Umbrella : MonoBehaviour, IPointerClickHandler
{
    private int _level;

    [HideInInspector]
    public UnityEvent<Umbrella> OnRightClick;
    
    public int level
    {
        get => _level;
        set => _level = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InvokeOnSelected();
    }
    
    private void InvokeOnSelected()
    {
        OnRightClick?.Invoke(this);
    }
}
