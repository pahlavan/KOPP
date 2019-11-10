using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.UI;

public class SpaceshipScript : MonoBehaviour
{
    public GameObject InitialPlanet;
    public GameObject FuelBar;
    public int MaxFuel = 100;
    public float ShipVelocity;
    public GameObject ShipBody;

    private bool inFlight = true;
    private bool isInitialized = false;
    private float fuel = 0;

    private GameObject nextHop;
    private GameObject lastHop;
    private GUIScript gui;

    void SelectNextHop()
    {
        lastHop = nextHop;
        var nextHopScript = nextHop.GetComponent<PlanetScript>();
        var forwardOption = nextHopScript.OutgoingPlanets;
        var detour = new List<GameObject>();
        detour.AddRange(nextHopScript.incomingPlanets);
        detour.AddRange(forwardOption);

        int cnt = forwardOption.Count;

        if (cnt == 0)
        {
            inFlight = false;
            gui.IncDamage();
        }
        else
        {
            foreach(var planet in detour)
            {
                if (planet.GetComponent<PlanetScript>().planetState == PlanetState.Detour)
                {
                    nextHop = planet;
                    return;
                }
            }

            nextHop = forwardOption[UnityEngine.Random.Range(0, cnt)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fuel = MaxFuel;
        gui = GameObject.Find("GUICanvas").GetComponent<GUIScript>();
    }

    void AdjustShipRotation()
    {
        ShipBody.transform.rotation = Quaternion.identity;
        int directionMult = transform.position.x < nextHop.transform.position.x ? -1 : +1;
        ShipBody.transform.Rotate(0, 0, directionMult * Vector3.Angle(new Vector3(0, 1, 0), nextHop.transform.position - transform.position));
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

            transform.position = Vector3.MoveTowards(transform.position, nextHop.transform.position, ShipVelocity / 10);
            fuel -= ShipVelocity / 4;

            FuelBar.transform.localScale = new Vector3((float)(3.424 * fuel / 100), 0.639f, 1);

            if (transform.position == nextHop.transform.position)
            {
                fuel = Math.Min(MaxFuel, fuel + 100);

                if (nextHop.GetComponent<PlanetScript>().planetState != PlanetState.SecurityCheck)
                {
                    SelectNextHop();
                }
            }
        }

        AdjustShipRotation();

        if (fuel <= 0 || !inFlight)
        {
            Destroy(gameObject);
        }
    }
}
