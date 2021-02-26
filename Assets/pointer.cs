using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
    float speed = 200;
    bool forwards = true;
    GameObject triangle;
    GameObject graduation;

    // Start is called before the first frame update
    void Start()
    {
        triangle = transform.Find("triangle").gameObject;
        graduation = transform.Find("graduation").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(movePointer(triangle.transform,graduation.transform));
    }

    private IEnumerator movePointer(Transform triangle, Transform graduation)
    {
        float trianglePosition = triangle.localPosition.x;
        float currentSpeed = speed;
	if (!forwards) {
	    currentSpeed = -currentSpeed;
        }
        triangle.localPosition = new Vector3(triangle.localPosition.x + speed * Time.deltaTime, triangle.localPosition.y, triangle.localPosition.z);
            graduation.localPosition = new Vector3(graduation.localPosition.x + speed * Time.deltaTime, graduation.localPosition.y, graduation.localPosition.z);
        yield return null;
    }
}
