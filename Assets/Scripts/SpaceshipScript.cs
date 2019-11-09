using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.UI;

public class SpaceshipScript : MonoBehaviour
{
    public GameObject InitialPlanet;
    public float Speed = (float)0.1;
    public Text text;
    public GameObject FuelBar;

    private bool inFlight = true;
    private bool isInitialized = false;
    private int distance = 0;

    private GameObject nextHop;
    private GameObject lastHop;

    //void DrawQuad(Rect position, Color color)
    //{
    //    Texture2D texture = new Texture2D(1, 1);
    //    texture.SetPixel(0, 0, color);
    //    texture.Apply();
    //    GUI.skin.box.normal.background = texture;
    //    GUI.Box(position, GUIContent.none);
    //}

    //void OnGUI()
    //{
    //    DrawQuad(new Rect(transform.position.x, transform.position.y + 50, 100, 10), Color.white);
    //}

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
            nextHop = options[Random.Range(0, cnt)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //var fuelBar = new GameObject();
        //var shape = fuelBar.AddComponent<SpriteShapeRenderer>();
        //shape.bounds = new Bounds(transform.position, GetComponent<SpriteRenderer>().bounds.size);
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
            if (nextHop.GetComponent<PlanetScript>().isSelected)
            {
                nextHop = lastHop;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextHop.transform.position, Speed * 5);
            distance++;
            text.text = distance.ToString();

            FuelBar.transform.localScale = new Vector3((float)(3.424 * (110 - distance) / 110.0), 0.639f, 1);

            if (transform.position == nextHop.transform.position)
            {
                if (!nextHop.GetComponent<PlanetScript>().isSelected)
                {
                    distance = 0;
                }
                SelectNextHop();
            }
        }

        if (distance > 110 || !inFlight)
        {
            Destroy(gameObject);
        }

    }
}
