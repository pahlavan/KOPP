using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenuScript : MonoBehaviour
{
    public static IList<GameObject> enabledActions = new List<GameObject>();

    private Vector3 location;
    private GameObject planet;
    private float radius;
    private int degreeOffset;
    private IList<GameObject> activeActions;

    public void Initialize(GameObject planet)
    {
        this.planet = planet;
        this.location = planet.transform.position;
        if (enabledActions.Count <= 1)
        {
            radius = 0;
            degreeOffset = 0;
        }
        else
        {
            radius = planet.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            degreeOffset = 360 / enabledActions.Count;
        }

        activeActions = new List<GameObject>();
    }

    public void Respwan(GameObject planet)
    {
        Initialize(planet);
        Debug.Log("Respawn");

        int i = 0;
        foreach(GameObject action in enabledActions)
        {
            var vec = Quaternion.Euler(0, 0, degreeOffset * i) * new Vector3(0, radius, 0);
            i++;

            Debug.Log(vec);
            var instance = Instantiate(action, location + vec, Quaternion.identity);
            activeActions.Add(instance);
        }
    }

    public void Collapse()
    {
        foreach (GameObject instance in activeActions)
        {
            Destroy(instance);
        }

        Destroy(this);
    }

    // Start is called before the first frame update
}
