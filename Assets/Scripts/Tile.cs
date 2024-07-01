using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int _cid = 0;
    private int _coefficient = 0;
    private bool _isSelected = false;

    [HideInInspector]
    public UnityEvent<Tile> onSelected;

    public Image ConnectionComponentImage => transform.Find("Connection/Pipe").GetComponent<Image>();

    public Image MarkComponentImage => transform.Find("Mark").GetComponent<Image>();

    public Color ConnectionColor => ConnectionComponentImage.color;

    public int cid
    {
        get => _cid;
        set => _cid = value;
    }

    public int coefficient
    {
        get => _coefficient;
        set => _coefficient = value;
    }

    public bool isSelected
    {
        get => _isSelected;
        set => _isSelected = value;
    }

    void Start()
    {
        if (_coefficient > 0 || _cid > 0)
        {
            SetConnectionColor(ConnectionComponentImage.color);
        }
        else
        {
            Destroy(MarkComponentImage.gameObject);
        }
    }

    public void SetConnectionColor(Color color) => ConnectionComponentImage.color = color;

    public void SetMarkColor(Color color) => MarkComponentImage.color = color;

    public void ResetConnection()
    {
        var connection = transform.Find("Connection").gameObject;
        connection.SetActive(false);
        connection.transform.eulerAngles = Vector3.zero;
    }

    public void SetMarkText(bool isActive) => transform.Find("Mark/Text").gameObject.SetActive(isActive);

    public void ConnectionToSide(bool top, bool right, bool bottom, bool left)
    {
        var connection = transform.Find("Connection").gameObject;
        connection.SetActive(true);
        connection.transform.eulerAngles = new Vector3(0, 0, right ? -90 : bottom ? -180 : left ? -270 : 0);
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

    private void InvokeOnSelected() => onSelected?.Invoke(this);
}
