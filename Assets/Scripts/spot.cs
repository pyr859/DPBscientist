using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spot : MonoBehaviour
{
    GameObject wound;
    GameObject blade;
    GameObject tick;
    private bool isWounded = false;
    private bool usingBlade = false;
    public int force = 1;

    // Start is called before the first frame update
    void Start()
    {
        wound = transform.Find("wound").gameObject;
        wound.SetActive(false);
        blade = GameObject.Find("blade");
        tick = GameObject.Find("/gauge/pointer/tick");
    }

    // Called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        if (!isWounded)
        {
            usingBlade = blade.GetComponent<blade>().isPicked;
            if (Input.GetMouseButton(0) && usingBlade)
            {
                force = IdentifyForce(tick);
                if (force >= 3) {
                    wound.SetActive(true);
                    isWounded = true;
                }
            }
        }
    }

    private int IdentifyForce(GameObject tick)
    {
        float tick_x = tick.transform.localPosition.x;
        if (tick_x <= 400) {
            return 1;
        }
        else if (tick_x <= 800) {
            return 2;
        }
        else if (tick_x <= 1200) {
            return 3;
        }
        else if (tick_x <= 1600) {
            return 4;
        }
        else {
            return 5;
        }
    }
}
