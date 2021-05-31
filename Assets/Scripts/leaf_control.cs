using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaf_control : MonoBehaviour
{
    GameObject lefthalf;
    GameObject righthalf;
    GameObject leftflip;
    GameObject rightflip;
    public GameObject fliptop;
    bool usingBlade;
    bool usingSyringe;
    public bool flipped;

    // Start is called before the first frame update
    void Start()
    {
        lefthalf = transform.Find("left/default").gameObject;
        righthalf = transform.Find("right/default").gameObject;
        leftflip = transform.Find("left/flip").gameObject;
        rightflip = transform.Find("right/flip").gameObject;
        flipped = false;
    }

    // Update is called once per frame
    void Update(){}

    // OnMouseOver is called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {       
        // when left click
        if (Input.GetMouseButtonDown(0)){
            usingBlade = GameObject.Find("blade").GetComponent<blade>().isPicked;
            usingSyringe = GameObject.Find("syringe").GetComponent<syringe>().isPicked;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            // if either the blade or the syringe is in use, terminate this execution loop
            // TBD: IMPLEMENT THE HINT MESSAGES
            if (usingBlade || usingSyringe){
                if (usingBlade){
                    print("You are using the blade.");
                }
                if (usingSyringe){
                    print("You are using the syringe.");
                }
            }
            // flipped = false, if the leaf is front side up => flipLeaf() executes a flipforth action series
            // flipped = true, if the leaf is back side up => flipLeaf() executes a flipback action series
            else {
                if (!flipped){
                    fliptop = hit.collider.transform.parent.Find("flip").gameObject;
                }
                else if (flipped){
                    fliptop = hit.collider.gameObject;
                }
                flipped = flipLeaf(flipped, fliptop);
            }
        }
    }

    public bool flipLeaf(bool flipback, GameObject flipgroup){
        // the flipforth action series: both front sides become inactive, and the flipped conformation becomes active
        if (!flipback){
            lefthalf.SetActive(false);
            righthalf.SetActive(false);
            flipgroup.SetActive(true);
            return true;
        }
        // the flipback action series: both front sides become active, and the flipped conformation becomes inactive
        else if (flipback){
            lefthalf.SetActive(true);
            righthalf.SetActive(true);
            flipgroup.SetActive(false);
            return false;
        }
        // the following return is never actually called, but Unity requires all code paths to return a value
        return flipback;
    }
}