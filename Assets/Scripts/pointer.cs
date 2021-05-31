using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
    // child GameObjects and their initial position vector
    GameObject triangle;
    Vector3 triangle_origin;
    GameObject tick;
    Vector3 tick_origin;

    // key variables for the pointer
    public float speed = 800;
    float tick_x = 0;
    int direction = 1;

    // crosstalk with other GameObjects
    GameObject blade;
    bool bladeIsPicked = false;
    GameObject syringe;
    bool syringeIsPicked = false;

    // Start is called before the first frame update
    void Start()
    {
        // For performance reasons, it is recommended to not use the "Find" function every frame. Instead, cache the result in a member variable at startup
        triangle = transform.Find("triangle").gameObject;
        tick = transform.Find("tick").gameObject;
        blade = GameObject.Find("blade");
        syringe = GameObject.Find("syringe");
        
        // save the starting position of the pointer, so to allow its return to the origin when the tool is dropped
        triangle_origin = triangle.transform.localPosition;
        tick_origin = tick.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // check whether one of the tools is picked up in every frame call
        bladeIsPicked = blade.GetComponent<blade>().isPicked;
        syringeIsPicked = syringe.GetComponent<syringe>().isPicked;
        
        // update tick_x every frame since it may have moved
        tick_x = tick.transform.localPosition.x;

        // if the blade is picked during a frame, move the gauge pointer along the x-axis with a defined speed
        if (bladeIsPicked) {    
            if (tick_x <= 0) {
                direction = 1;
            }
            else if (tick_x >= 2000) {
                direction = -1;
            }
            StartCoroutine(movePointer(triangle.transform, tick.transform));
        }
        // if the syringe is picked during a frame
        else if (syringeIsPicked) {
            // when the left mouse button is pressed
            if (Input.GetMouseButton(0)) {
                // if the tick is inside the appropriate range (i.e. x <= 2000), move the pointer to the right at a constant speed
                if (tick_x <= 2000) {
                    direction = 1;
                    StartCoroutine(movePointer(triangle.transform, tick.transform));
                }
                // if the tick is beyond the appropriate range, do nothing
            }
            // when the left mouse button is not pressed
            else {
                // as long as the tick is inside the appropriate range (i.e. x >= 0), move the pointer to the left at a constant speed
                if (tick_x >= 0) {
                    direction = -1;
                    StartCoroutine(movePointer(triangle.transform, tick.transform));
                }
            }
        }
        // if no tool is picked during a frame, set the gauge pointer to its origin
        else if (!bladeIsPicked && !syringeIsPicked) {
            triangle.transform.localPosition = triangle_origin;
            tick.transform.localPosition = tick_origin;
        }
    }

    private IEnumerator movePointer(Transform triangle, Transform tick)
    {
        triangle.localPosition = new Vector3(triangle.localPosition.x + speed * Time.deltaTime * direction, triangle.localPosition.y, triangle.localPosition.z);
        tick.localPosition = new Vector3(tick.localPosition.x + speed * Time.deltaTime * direction, tick.localPosition.y, tick.localPosition.z);
        yield return null;
    }
}
