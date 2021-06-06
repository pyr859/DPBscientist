using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syringe : MonoBehaviour
{
    public bool isPicked;
    bool usingBlade;
    float spd_base = 100;
    float spd_multiplier;
    public float vol;
    Vector3 startPos;
    Vector3 mousePos;
    Vector3 plunger_startPos;
    Vector3 buffer_startPos;
    GameObject plunger;
    GameObject buffer;
    GameObject blade;
    GameObject tick;

    // Start is called before the first frame update
    void Start()
    {
        isPicked = false;
        startPos = this.gameObject.transform.localPosition;
        plunger = transform.Find("plunger").gameObject;
        buffer = transform.Find("mask-liquid").gameObject;
        plunger_startPos = plunger.transform.localPosition;
        buffer_startPos = buffer.transform.localPosition;
        blade = GameObject.Find("blade");
        tick = GameObject.Find("gauge/pointer/tick");
        vol = 300;
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
                buffer.transform.localPosition = buffer_startPos;
                isPicked = false;
                vol = 300;
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
        float tick_x = tick.transform.localPosition.x;
        if (tick_x <= 666) {
            spd_multiplier = 0.5f;
        }
        else if (tick_x <= 1332) {
            spd_multiplier = 1;
        }
        else {
            spd_multiplier = 2;
        }
        
        if (vol > 0){
            vol -= spd_base * spd_multiplier * Time.deltaTime;
            plunger.transform.localPosition = new Vector3(0.7f, vol + 450f, 0);
            buffer.transform.localPosition = new Vector3(2.8f, vol - 247.5f, 0);
            yield return null;
        }
    }
}
