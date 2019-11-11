using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour
{
    public float letterPause = 0.2f;
   // public GameObject Speaker;
    string message;
    Text textComp;
    public bool TextActive;
    bool TextisActive = false;
    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<Text>();
        message = textComp.text;
        textComp.text = "";
    }

    void Update()
    {
        if (gameObject.active && !TextisActive)
        {
            TextisActive = true;
            StartCoroutine(TypeText());
        }

    }

    IEnumerator TypeText()
    {
            foreach (char letter in message.ToCharArray())
            {
                textComp.text += letter;

                yield return 0;
                yield return new WaitForSeconds(letterPause);
            }
    }
}
