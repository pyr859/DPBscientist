using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syringe : MonoBehaviour
{
    public bool isPicked = false;
    public bool onSpot = false;
    float pushPlunger_speed = 200;
    Vector3 startPos;
    Vector3 mousePos;
    Vector3 plunger_startPos;
    GameObject spot;
    GameObject plunger;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.localPosition;
        spot = GameObject.Find("spot");
        plunger = transform.Find("plunger").gameObject;
        plunger_startPos = plunger.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (isPicked)
        {
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
            if (Input.GetMouseButton(1))
            {
                this.gameObject.transform.Rotate(0, 0, 40);
                this.gameObject.transform.localPosition = startPos;
                plunger.transform.localPosition = plunger_startPos;
                isPicked = false;
            }
            if (onSpot && Input.GetMouseButton(0))
            {
                StartCoroutine(pushPlunger(plunger.transform));
            }
        }
    }

    // Called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        if (!isPicked && Input.GetMouseButton(0))
        {
            isPicked = true;
            this.gameObject.transform.Rotate(0, 0, -40);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "spot")
        {
            onSpot = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "spot")
        {
            onSpot = false;
        }
    }

    private IEnumerator pushPlunger(Transform pos)
    {
        pos.localPosition = new Vector3(pos.localPosition.x, pos.localPosition.y - pushPlunger_speed * Time.deltaTime, pos.localPosition.z);
        yield return null;
    }
}
