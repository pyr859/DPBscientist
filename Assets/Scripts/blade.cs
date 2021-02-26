using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blade : MonoBehaviour
{
    public bool isPicked = false;
    bool usingSyringe;
    Vector3 startPos;
    Vector3 mousePos;
    GameObject syringe;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.localPosition;
        syringe = GameObject.Find("syringe");
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
                this.gameObject.transform.Rotate(0, 0, -35);
                this.gameObject.transform.localPosition = startPos;
                isPicked = false;
            }
        }
    }

    // Called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        usingSyringe = syringe.GetComponent<syringe>().isPicked;
        if (!isPicked && !usingSyringe && Input.GetMouseButton(0))
        {
            isPicked = true;
            this.gameObject.transform.Rotate(0, 0, 35);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
}
