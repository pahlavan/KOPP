using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class content_mngr : MonoBehaviour
{
    public GameObject dialogA;
    public GameObject dialogB;
    public GameObject dialogC;
    bool TextAdone;
    bool TextBdone;
    // Start is called before the first frame update
    void Start()
    {
        dialogA.active = false;
        dialogB.active = false;

        TextAdone = false;
        TextBdone = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!TextAdone)
        {
            Invoke("TextA", 1.6f);
            TextAdone = true;
        }

        if (!TextBdone)
        {
            Invoke("TextB", 15f);
            TextBdone = true;
        }

    }

    void TextA()
    {
        dialogA.active = true;
        dialogB.active = false;
    }

    void TextB()
    {
        dialogB.active = true;
        dialogA.active = false;
    }

    void TextC()
    {

    }
}
