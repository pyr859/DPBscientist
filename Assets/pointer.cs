using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
    float speed = 500;
    bool forwards = true;
    GameObject triangle;
    GameObject graduation;
    bool bladeIsPicked;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        triangle = transform.Find("triangle").gameObject;
        graduation = transform.Find("graduation").gameObject;
        bladeIsPicked = GameObject.Find("blade").GetComponent<blade>().isPicked;
    }

    // Update is called once per frame
    void Update()
    {
        bladeIsPicked = GameObject.Find("blade").GetComponent<blade>().isPicked;
        
        if (bladeIsPicked) {    
            if (graduation.transform.localPosition.x <= 0) {
                direction = 1;
            } else if (graduation.transform.localPosition.x >= 2000) {
                direction = -1;
            }        
            StartCoroutine(movePointer(triangle.transform,graduation.transform));
        }
    }

    private IEnumerator movePointer(Transform triangle, Transform graduation)
    {
        float trianglePosition = triangle.localPosition.x;
        float currentSpeed = speed;
	if (!forwards) {
	    currentSpeed = -currentSpeed;
        }
        triangle.localPosition = new Vector3(triangle.localPosition.x + speed * Time.deltaTime * direction, triangle.localPosition.y, triangle.localPosition.z);
            graduation.localPosition = new Vector3(graduation.localPosition.x + speed * Time.deltaTime * direction, graduation.localPosition.y, graduation.localPosition.z);
        yield return null;
    }
}
