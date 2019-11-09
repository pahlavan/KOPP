using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public GameObject spaceshipPrefab;
    public GameObject initialPlanet;
    public Text text;

    private int timer = 0;

    void createNewShip()
    {
        var newShip = Instantiate(spaceshipPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var shipScript = newShip.GetComponent<SpaceshipScript>();
        shipScript.InitialPlanet = initialPlanet;
        shipScript.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        // createNewShip();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer++ % 60 == 0)
        {
            createNewShip();
        }
    }
}
