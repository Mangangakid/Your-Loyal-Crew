using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarTwoColors : MonoBehaviour
{
    public Scrollbar myScroll;
    public RectTransform completedArea;

    private void Start()
    {

    }

    public void ChangeLength()
    {
        if (completedArea != null)
        {
            completedArea.localScale = new Vector3(myScroll.value*1f, 1f, 1f);
        }
        else
        {
            Debug.Log("Che, lo que no anda es ésto: falta el area de lo completo de " + this.gameObject.name);
        }
    }
}
