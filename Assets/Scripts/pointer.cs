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
    float speed = 1800;
    float tick_x;
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
            StartCoroutine(movePointer("blade"));
        }

        // if the syringe is picked during a frame but is inactive, move the gauge pointer to the left toward the origin at a constant speed
        else if (syringeIsPicked && !Input.GetMouseButton(0)){
            StartCoroutine(movePointer("syringe_idle"));
        }

        // if no tool is picked during a frame, set the gauge pointer to its origin
        else if (!bladeIsPicked && !syringeIsPicked) {
            triangle.transform.localPosition = triangle_origin;
            tick.transform.localPosition = tick_origin;
        }
    }

    public IEnumerator movePointer(string mode)
    {
        if (mode == "blade"){
            if (tick_x <= 0) {
                direction = 1;
            }
            else if (tick_x >= 2000) {
                direction = -1;
            }
        }
        // if the tick is inside the appropriate range (x <= 2000), move the pointer to the right at a constant speed
        // if the tick is beyond the appropriate range, remain in place
        else if (mode == "syringe_push"){
            if (tick_x <= 2000){
                direction = 1;
            }
            else if (tick_x > 2000){
                direction = 0;
            }
        }
        // if the tick is inside the appropriate range (x >= 0), move the pointer to the left at a constant speed
        // if the tick is beyond the appropriate range, remain in place
        else if (mode == "syringe_idle"){
            if (tick_x > 0) {
                direction = -1;
            }
            else if (tick_x <= 0) {
                direction = 0;
            }
        }
        else {
            Debug.Log("Invalid command");
        }

        Transform tri = triangle.transform;
        Transform tic = tick.transform;
        // move the triangle
        tri.localPosition = new Vector3(tri.localPosition.x + speed * Time.deltaTime * direction, tri.localPosition.y, tri.localPosition.z);
        // move the tick
        tic.localPosition = new Vector3(tic.localPosition.x + speed * Time.deltaTime * direction, tic.localPosition.y, tic.localPosition.z);
        yield return null;
    }
}
