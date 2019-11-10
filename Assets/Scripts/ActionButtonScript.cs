using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void OnMouseEnteExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
