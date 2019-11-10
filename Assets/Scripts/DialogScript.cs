using System.Collections;
using System.Collections.Generic;
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

    private Text textbox;
    private Image panel;

    public Color AIColor;
    public Color HumenColor;
    public Color CommanderColor;

    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.transform.Find("Panel").GetComponent<Image>();
        textbox = gameObject.transform.Find("Panel/Text").GetComponent<Text>();
        showNewMessage(DialogSource.Humen, "We are inbound. Please help us with navigation throught the galaxy...");
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

        textbox.text = messageState;
    }

    public void showNewMessage(DialogSource source, string msg)
    {
        startTime = Time.time;
        message = msg;
        messageState = "";

        switch (source)
        {
            case DialogSource.AI: panel.color = AIColor; break;
            case DialogSource.Commander: panel.color = CommanderColor; break;
            case DialogSource.Humen: panel.color = HumenColor; break;
        }
    }
}
