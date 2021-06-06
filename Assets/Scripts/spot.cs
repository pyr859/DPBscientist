using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    float wound_multiplier = 0;
    float speed_base = 1.3f;
    float push_multiplier;
    public int score;

    // parent and sibling references
    GameObject petiole;
    GameObject fliptop;
    GameObject front;
    GameObject front_cover;
    GameObject front_mask;

    // global references
    GameObject blade;
    GameObject syringe;
    GameObject pointer;
    GameObject tick;
    public GameObject scoreboard;
    bool usingBlade = false;
    bool usingSyringe = false;
    int force;
    float syringe_vol;

    // Start is called before the first frame update
    void Start()
    {
        // cache childs
        ring = transform.Find("ring").gameObject;
        cover = transform.Find("cover").gameObject;
        wound = transform.Find("wound").gameObject;
        infiltrate = transform.Find("mask-water").gameObject;
        
        // cache parents and siblings
        petiole = transform.parent.parent.parent.gameObject;
        fliptop = transform.parent.gameObject;
        front = transform.parent.parent.gameObject.transform.Find("default").gameObject;
        front_cover = front.transform.Find(this.name + "/cover").gameObject; 
        front_mask = front.transform.Find(this.name + "/mask-water").gameObject;
        
        // cache global objects
        blade = GameObject.Find("blade");
        syringe = GameObject.Find("syringe");
        pointer = GameObject.Find("gauge/pointer");
        tick = pointer.transform.Find("tick").gameObject;
        scoreboard = GameObject.Find("Canvas/scores").gameObject;
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
        // if the primary mouse button is cliked while hovering over a spot
        if (Input.GetMouseButton(0)){
            usingBlade = blade.GetComponent<blade>().isPicked;
            usingSyringe = syringe.GetComponent<syringe>().isPicked;
            // if using the blade
            if (usingBlade && !isWounded){
                force = IdentifyForce(tick);
                // if the incision force is 3, inactivate the cover to create a hole
                if (force == 3){
                    cover.SetActive(false);
                    isWounded = true;
                    wound_multiplier = 0.5f;
                    
                    // inactivate the corresponding cover on the front view
                    front_cover.SetActive(false);
                }
                // if the incision force is 2, activate the wound sprite
                else if (force == 2){
                    wound.SetActive(true);
                    // darken the cover color to the same brightness (HSV) as the watermark
                    front_cover.GetComponent<Renderer>().material.color = Color.HSVToRGB(0, 0, 0.7f);
                    isWounded = true;
                    wound_multiplier = 1;
                }
            }
            // if using the syringe
            else if (usingSyringe && isWounded){
                StartCoroutine(pointer.GetComponent<pointer>().movePointer("syringe_push"));
                force = IdentifyForce(tick);
                syringe_vol = syringe.GetComponent<syringe>().vol;
                // If the syringe volume is greater than 0, initialize the infiltrate watermark when force is >= 2
                if (syringe_vol > 0 && !infiltrate.activeSelf && force >= 2){
                    infiltrate.SetActive(true);
                    front_mask.SetActive(true);
                    front.transform.Find("watermark1").gameObject.SetActive(true);
                }
                // update the speed multiplier for watermark expansion based on gauge force
                if (infiltrate.activeSelf){
                    if (force == 1){
                        push_multiplier = 0.25f;
                    }
                    else if (force == 2){
                        push_multiplier = 1;
                    }
                    else if (force == 3){
                        push_multiplier = 0.5F;
                    }
                }
                StartCoroutine(syringe.GetComponent<syringe>().pushPlunger());
                StartCoroutine(infiltration());
            }
            // the spot collider masks the leaf collider due to the Z transform ordering
            // patch: the following (else if) chunk executes leaf flip when the mouse click falls on the spot collider
            else if (!usingBlade && !usingSyringe){
                petiole.GetComponent<leaf_control>().flipped = petiole.GetComponent<leaf_control>().flipLeaf(true, fliptop);
            }
        }
    }
    // Called when the mouse is not any longer over the Collider
    // trigger result calculation if mask-water is active
    private void OnMouseExit() {
        if (infiltrate.activeSelf && ring.activeSelf){
            ring.SetActive(false);
            score = Mathf.RoundToInt(Mathf.Min(100, 100 * infiltrate.transform.localScale.x / 4));
            scoreboard.GetComponent<scoreboard>().displayScore(score);
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

    private int IdentifyForce(GameObject tick)
    {
        float tick_x = tick.transform.localPosition.x;
        if (tick_x <= 666) {
            return 1;
        }
        else if (tick_x <= 1332) {
            return 2;
        }
        else {
            return 3;
        }
    }

    private IEnumerator infiltration()
    {
        // check (1) the activity status of the spot-ring, (2) the remaining volume in the syringe, and (3) the watermark area on the leaf
        // infiltration only proceeds if the ring is still active, plus both values have not exceeded their respective threshold
        Transform area = infiltrate.transform;
        Transform counterpart = front_mask.transform;
        if (ring.activeSelf && syringe_vol > 0 && area.localScale.x < 4){
            area.localScale += new Vector3(speed_base * wound_multiplier * push_multiplier * Time.deltaTime, speed_base * wound_multiplier * push_multiplier * Time.deltaTime * 1.25f, 0);
            
            // simultaneously resize the watermark in the front view
            counterpart.localScale = area.localScale;
            yield return null;
        }
    }
}
