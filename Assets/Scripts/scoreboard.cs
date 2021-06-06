using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreboard : MonoBehaviour
{
    int line_count;
    Text score_list;

    // Start is called before the first frame update
    void Start()
    {
        line_count = 1;
        score_list = this.GetComponent<Text>();
    }

    public void displayScore(int score){
        if (score == 100){
            score_list.text += "Good job! The efficiency of infiltration attempt #" + line_count + " is " + score + "\n";
            line_count += 1;
        }
        else {
            score_list.text += "The efficiency of infiltration attempt #" + line_count + " is " + score + "\n";
            line_count += 1;
        }
    }
}
