using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    public GameObject InitialPlanet;
    public float Speed = (float)0.1;

    private bool inFlight = true;

    private GameObject nextHop;

    void SelectNextHop()
    {
        var options = nextHop.GetComponent<PlanetScript>().OutgoingPlanets;
        int cnt = options.Count;

        if (cnt == 0)
        {
            inFlight = false;
        }
        else
        {
            nextHop = options[Random.Range(0, cnt)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = InitialPlanet.transform.position;
        nextHop = InitialPlanet;
        SelectNextHop();
    }

    // Update is called once per frame
    void Update()
    {
        if (inFlight)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextHop.transform.position, Speed * 5);
            if (transform.position == nextHop.transform.position)
            {
                SelectNextHop();
            }
        }
    }
}
