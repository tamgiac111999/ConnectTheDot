using System.Collections;
using System.Collections.Generic;
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
        get { return _level; }
        set { this._level = value; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InvokeOnSelected();
    }
    
    public void InvokeOnSelected()
    {
        if (OnRightClick != null)
        {
            OnRightClick.Invoke(this.GetComponent<Umbrella>());
        }
    }
}
