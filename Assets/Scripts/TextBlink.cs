using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    private int counter = 0;
    private GUIScript gui;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        gui = GameObject.Find("GUICanvas").GetComponent<GUIScript>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gui.isOverheat) text.enabled = false;
        else text.enabled = (counter++ % 50) > 25;
    }
}
