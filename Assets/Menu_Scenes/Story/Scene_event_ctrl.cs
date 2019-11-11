using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_event_ctrl : MonoBehaviour
{
    public float EventTimer1;
    public float EventTimer2;
    public float EventTimer3;

    public GameObject textBox;
    bool textBoxON;
    public GameObject CapitanHolo;
    public GameObject CheifHolo;
    public GameObject AiHolo;

    public GameObject HoloRays;
    bool CapitanDone = false;

    // Start is called before the first frame update
    void Start()
    {
        textBoxON = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!textBoxON)
        {
            textBoxON = true;
            Invoke("TextBox", EventTimer1);
        }

        if (CapitanDone)
        {
            CapitanDone = false;
            Invoke("CheifTalkA", EventTimer2);
        }

        Invoke("nextScene", EventTimer3);
    }

    void TextBox()
    {
        HoloRays.GetComponent<Animator>().Play("Holo_start");
        CapitanHolo.GetComponent<Animator>().Play("Capitan_start");
        textBox.GetComponent<Animator>().Play("textBox_on");
        CapitanHolo.GetComponent<AudioSource>().Play();
        CapitanDone = true;
    }

    void CapTalkA()
    {

    }

    void CheifTalkA()
    {
        CapitanHolo.GetComponent<Animator>().Play("Capitan_end");
        HoloRays.GetComponent<Animator>().Play("Holo_start");
        CheifHolo.GetComponent<Animator>().Play("Chief_start");
        CheifHolo.GetComponent<AudioSource>().Play();

    }

    void nextScene()
    {
        SceneManager.LoadScene("Story2");
    }
}
