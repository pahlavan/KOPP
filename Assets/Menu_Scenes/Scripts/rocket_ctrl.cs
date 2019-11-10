using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket_ctrl : MonoBehaviour
{
    public Vector3 destination;
    public float VSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(destination.x, destination.y -8, -.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position != destination)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, VSpeed);
        }
    }
}
