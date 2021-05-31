using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spot : MonoBehaviour
{
    // child references
    GameObject ring;
    GameObject cover;
    GameObject wound;
    GameObject infiltrate;
    int ring_dir;
    float ring_speed = 0.004f;
    bool isWounded = false;

    // parent references
    GameObject petiole;
    GameObject fliptop;

    // global references
    GameObject blade;
    GameObject syringe;
    GameObject tick;
    bool usingBlade = false;
    bool usingSyringe = false;
    public int force = 1;

    // Start is called before the first frame update
    void Start()
    {
        // cache childs
        ring = transform.Find("ring").gameObject;
        cover = transform.Find("cover").gameObject;
        wound = transform.Find("wound").gameObject;
        infiltrate = transform.Find("mask-water").gameObject;
        
        // cache parents
        petiole = transform.parent.parent.parent.gameObject;
        fliptop = transform.parent.gameObject;
        
        // cache global objects
        blade = GameObject.Find("blade");
        syringe = GameObject.Find("syringe");
        tick = GameObject.Find("/gauge/pointer/tick");
    }

    // Update is called once per frame
    void Update()
    {
        if (ring.transform.localScale.x <= 1){
            ring_dir = 1;
        }
        else if (ring.transform.localScale.x >= 1.4){
            ring_dir = -1;
        }
        StartCoroutine(ring_animation(ring.transform));
    }

    // OnMouseOver is called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        //
        if (Input.GetMouseButton(0)){
            usingBlade = blade.GetComponent<blade>().isPicked;
            usingSyringe = syringe.GetComponent<syringe>().isPicked;
            // if using the blade and the incision force is 5, inactivate the cover to create a hole
            // if the incision force is 3 or 4, activate the wound sprite
            if (usingBlade && !isWounded){
                force = IdentifyForce(tick);
                if (force == 5){
                    cover.SetActive(false);
                    isWounded = true;
                }
                else if (force >= 3){
                    wound.SetActive(true);
                    isWounded = true;
                }
            }
            // if using the syringe, initialize the infiltrate watermark
            // push the plunger and expand the watermark area
            else if (usingSyringe && isWounded){
                if (!infiltrate.activeSelf){
                    infiltrate.SetActive(true);
                }
                StartCoroutine(syringe.GetComponent<syringe>().pushPlunger());
                StartCoroutine(infiltration(infiltrate.transform));
            }
            // the spot collider masks the leaf collider due to the Z transform ordering
            // patch: the following (else if) chunk executes leaf flip when the mouse click falls on the spot collider
            else if (!usingBlade && !usingSyringe){
                petiole.GetComponent<leaf_control>().flipped = petiole.GetComponent<leaf_control>().flipLeaf(true, fliptop);
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

    private IEnumerator ring_animation(Transform ring)
    {
        if (ring_dir == 1){
            ring.localScale += new Vector3(ring_speed, ring_speed, 1);
            yield return null;
        }
        else {
            ring.localScale -= new Vector3(ring_speed, ring_speed, 1);
            yield return null;
        }
    }

    private IEnumerator infiltration(Transform area)
    {
        if (area.localScale.x < 3){
            area.localScale += new Vector3(0.01f, 0.02f, 0);
            yield return null;
        }
    }
}
