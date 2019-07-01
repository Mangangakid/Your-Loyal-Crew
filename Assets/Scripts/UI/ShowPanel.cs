using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void ShowAnimation(bool show)
    {
        if (show)
        {
            anim.SetTrigger("Show");
        }
        else
        {
            anim.SetTrigger("Hide");
        }
    }
}
