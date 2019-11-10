using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.UI;

public class SpaceshipScript : MonoBehaviour
{
    public GameObject InitialPlanet;
    public float Speed = (float)0.1;
    public GameObject FuelBar;
    public int MaxFuel;

    private bool inFlight = true;
    private bool isInitialized = false;
    private int fuel = 0;

    private GameObject nextHop;
    private GameObject lastHop;

    void SelectNextHop()
    {
        lastHop = nextHop;
        var options = nextHop.GetComponent<PlanetScript>().OutgoingPlanets;
        int cnt = options.Count;

        if (cnt == 0)
        {
            inFlight = false;
        }
        else
        {
            foreach(var planet in options)
            {
                if (planet.GetComponent<PlanetScript>().planetState == PlanetState.Detour)
                {
                    nextHop = planet;
                    return;
                }
            }

            nextHop = options[Random.Range(0, cnt)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fuel = MaxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            transform.position = InitialPlanet.transform.position;
            nextHop = InitialPlanet;
            SelectNextHop();
            isInitialized = true;
        }

        if (inFlight)
        {
            if (nextHop.GetComponent<PlanetScript>().planetState == PlanetState.DangerZone)
            {
                nextHop = lastHop;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextHop.transform.position, Speed);
            fuel--;

            FuelBar.transform.localScale = new Vector3((float)(3.424 * fuel / MaxFuel), 0.639f, 1);

            if (transform.position == nextHop.transform.position)
            {
                fuel = MaxFuel;

                if (nextHop.GetComponent<PlanetScript>().planetState != PlanetState.SecurityCheck)
                {
                    SelectNextHop();
                }
            }
        }

        if (fuel <= 0 || !inFlight)
        {
            Destroy(gameObject);
        }

    }
}
