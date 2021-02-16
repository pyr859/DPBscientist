using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spot : MonoBehaviour
{
    GameObject wound;
    GameObject blade;
    private bool isWounded = false;
    private bool usingBlade = false;


    // Start is called before the first frame update
    void Start()
    {
        wound = transform.Find("wound").gameObject;
        wound.SetActive(false);
        blade = GameObject.Find("blade");
    }

    // Called every frame while the mouse is over the Collider
    private void OnMouseOver() {
        if (!isWounded)
        {
            usingBlade = blade.GetComponent<blade>().isPicked;
            if (Input.GetMouseButton(0) && usingBlade)
            {
                wound.SetActive(true);
                isWounded = true;
            }
        }
    }
}
