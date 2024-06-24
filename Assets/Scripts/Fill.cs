using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    public Image FillComponentImage
    {
        get
        {
            return this.gameObject.GetComponent<Image>();
        }
        set { }
    }

    public Color FillColor
    {
        get
        {
            return this.FillComponentImage.color;
        }
        set { }
    }
    
    public void SetFillColor(Color color)
    {
        FillComponentImage.color = color;
    }
}
