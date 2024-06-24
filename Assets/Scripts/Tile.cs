using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isSelected = false;
    private bool _isPlayable = false;
    public int cid = 0;
    public int coefficient = 0;
    public static int UNPLAYABLE_INDEX = 0;
    public static string NAME_MARK = "Mark";
    public static string NAME_CONNECTION = "Connection";

    [HideInInspector]
    public UnityEvent<Tile> onSelected;

    public Image ConnectionComponentImage
    {
        get
        {
            return this.transform.Find(NAME_CONNECTION).gameObject.transform.Find("Pipe").gameObject.GetComponent<Image>();
        }
        set { }
    }

    public Image MarkComponentImage
    {
        get
        {
            return this.transform.Find(NAME_MARK).gameObject.GetComponent<Image>();
        }
        set { }
    }

    public bool isSelected
    {
        get { return _isSelected; }
        set { this._isSelected = value; }
    }

    public bool isPlayable
    {
        get { return this._isPlayable; }
        set { this._isPlayable = value; }
    }

    public Color ConnectionColor
    {
        get
        {
            return this.ConnectionComponentImage.color;
        }
        set { }
    }

    void Start()
    {
        _isPlayable = coefficient > UNPLAYABLE_INDEX || cid > UNPLAYABLE_INDEX;

        if (_isPlayable)
        {
            SetConnectionColor(ConnectionComponentImage.color);
        }
        else
        {
            Destroy(MarkComponentImage.gameObject);
        }
    }

    public void SetConnectionColor(Color color)
    {
        ConnectionComponentImage.color = color;
    }

    public void SetMarkColor(Color color)
    {
        MarkComponentImage.color = color;
    }

    public void ResetConnection()
    {
        var connection = this.transform.Find(NAME_CONNECTION).gameObject;
        connection.SetActive(false);
        connection.transform.eulerAngles = Vector3.zero;
    }

    public void SetMarkText()
    {
        var markTextObject = this.transform.Find(NAME_MARK).gameObject.transform.Find("Text").gameObject;
        markTextObject.SetActive(true);
    }
    public void ResetMarkText()
    {
        var markTextObject = this.transform.Find(NAME_MARK).gameObject.transform.Find("Text").gameObject;
        markTextObject.SetActive(false);
    }

    public void ConnectionToSide(bool top, bool right, bool bottom, bool left)
    {
        var connection = this.transform.Find(NAME_CONNECTION).gameObject;
        connection.SetActive(true);
        connection.transform.eulerAngles = Vector3.zero;
        int angle = right ? -90 : bottom ? -180 : left ? -270 : 0;
        this.transform.Find(NAME_CONNECTION).gameObject.transform.Rotate(new Vector3(0, 0, angle));
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        InvokeOnSelected();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _isSelected = false;
        InvokeOnSelected();
    }
    public void InvokeOnSelected()
    {
        if (onSelected != null)
        {
            onSelected.Invoke(this.GetComponent<Tile>());
        }
    }
}
