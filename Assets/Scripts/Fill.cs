using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    public Image FillComponentImage => GetComponent<Image>();

    public Color FillColor
    {
        get => FillComponentImage.color;
        set => FillComponentImage.color = value;
    }

    public void SetFillColor(Color color)
    {
        FillComponentImage.color = color;
    }
}
