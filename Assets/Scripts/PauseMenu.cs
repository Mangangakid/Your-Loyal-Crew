using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    
    public Toggle perspectiveToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Perspective"))
        {
            bool i = false;
            int j = PlayerPrefs.GetInt("Perspective");
            if (j == 1)
            {
                i = true;
            }
            PerspectiveChange(i);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    private void PerspectiveChange(bool value)
    {
        perspectiveToggle.isOn = value;
        if (value)
        {
            PlayerPrefs.SetInt("Perspective", 1);
            Camera.main.orthographic =!value;
        }
        else
        {
            PlayerPrefs.SetInt("Perspective", 0);
        }
        Camera.main.orthographic = !value;
    }

    public void PerspectiveToggle()
    {
        PerspectiveChange(perspectiveToggle.isOn);
    }
}
