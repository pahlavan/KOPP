﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogSource
{
    AI,
    Commander,
    Humen,
}

public class DialogScript : MonoBehaviour
{
    private string message;
    private string messageState;
    private float startTime;
    
    public bool isOpen
    {
        get
        {
            return textbox.enabled;
        }
    }


    private Text textbox;
    private Image panel;
    private SpriteRenderer bossHologram;
    private SpriteRenderer humanHologram;

    public Color AIColor;
    public Color HumenColor;
    public Color CommanderColor;

    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.transform.Find("Panel").GetComponent<Image>();
        textbox = gameObject.transform.Find("Panel/Text").GetComponent<Text>();
        bossHologram = GameObject.Find("BossHologram").GetComponent<SpriteRenderer>();
        humanHologram = GameObject.Find("HumanHologram").GetComponent<SpriteRenderer>();

        showNewMessage(DialogSource.Humen, "We are inbound. Please help us with navigation...");
    }

    // Update is called once per frame
    void Update()
    {
        if (messageState.Length < message.Length)
        {
            int expectedCount = (int)((Time.time - startTime) / 0.05);
            if (messageState.Length < expectedCount)
            {
                messageState = message.Substring(0, expectedCount);
            }
        }

        if (Time.time - startTime > 7)
        {
            panel.enabled = false;
            textbox.enabled = false;
            humanHologram.enabled = false;
            bossHologram.enabled = false;
        }

        textbox.text = messageState;
    }

    public void showNewMessage(DialogSource source, string msg)
    {
        startTime = Time.time;
        message = msg;
        messageState = "";
        panel.enabled = true;
        textbox.enabled = true;

        switch (source)
        {
            case DialogSource.AI:
                panel.color = AIColor;
                break;
            
            case DialogSource.Commander:
                panel.color = CommanderColor;
                bossHologram.enabled = true;
                break;
            
            case DialogSource.Humen:
                panel.color = HumenColor;
                humanHologram.enabled = true;
                break;
        }
    }
}
