using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scroll_sprite : MonoBehaviour
{
    public float ScrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(0, Time.time * ScrollSpeed);

        //gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset = offset;
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
