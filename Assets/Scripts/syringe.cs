using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syringe : MonoBehaviour
{
    public bool isPicked = false;
    bool usingBlade;
    float push_speed = 300;
    Vector3 startPos;
    Vector3 mousePos;
    Vector3 plunger_startPos;
    GameObject blade;
    GameObject plunger;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.localPosition;
        blade = GameObject.Find("blade");
        plunger = transform.Find("plunger").gameObject;
        plunger_startPos = plunger.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (isPicked)
        {
            // udpate the syringe position based on the mouse position in every frame
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);

            // drop the syringe upon right click
            if (Input.GetMouseButton(1))
            {
                this.gameObject.transform.Rotate(0, 0, 40);
                this.gameObject.transform.localPosition = startPos;
                plunger.transform.localPosition = plunger_startPos;
                isPicked = false;
            }
        }
    }

    // OnMouseOVer is called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        usingBlade = blade.GetComponent<blade>().isPicked;
        if (!isPicked && !usingBlade && Input.GetMouseButton(0))
        {
            isPicked = true;
            this.gameObject.transform.Rotate(0, 0, -40);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    public IEnumerator pushPlunger()
    {
        Transform pos = plunger.transform;
        if (pos.localPosition.y > 450){
            pos.localPosition = new Vector3(pos.localPosition.x, pos.localPosition.y - push_speed * Time.deltaTime, pos.localPosition.z);
            yield return null;
        }
    }
}
