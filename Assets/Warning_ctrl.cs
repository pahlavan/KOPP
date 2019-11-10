using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Warning_ctrl : MonoBehaviour
{
    public Sprite Warning0;
    public Sprite Warning1;
    public Sprite Warning2;
    public Sprite Warning3;
    public Sprite Warning4;
    public Sprite Warning5;
    public Sprite Warning6;
    public Sprite Warning7;
    public Sprite Warning8;

    public float WarningLevel;
    int WarningCase;

    // Start is called before the first frame update
    void Start()
    {
        WarningLevel = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = Warning0;
    }

    // Update is called once per frame
    void Update()
    {
        WarningCase = (int)WarningLevel;

        switch (WarningCase)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning0;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning1;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning2;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning3;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning4;
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning5;
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning6;
                break;
            case 7:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning7;
                break;
            case 8:
                gameObject.GetComponent<SpriteRenderer>().sprite = Warning8;
                break;
        }
    }
}
