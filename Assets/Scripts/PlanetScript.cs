using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> OutgoingPlanets;
    public Text text;

    private int selectionTime = 0;
    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void SelectPlanet()
    {
        //selectionTime = 100000;
        isSelected = !isSelected;
    }

    void CheckSelection()
    {
        //text.text = selectionTime.ToString();
        if(isSelected)
        {
            selectionTime--;
            if (selectionTime <= 0)
            {
                isSelected = false;
            }
        }
        else
        {
            selectionTime = 100;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var planet in OutgoingPlanets)
        {
            DrawLine(transform.position, planet.transform.position, Color.green);
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        CheckSelection();

        if (isSelected)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    isSelected = true;
        //}
    }

    void OnMouseDown()
    {
        isSelected = !isSelected;
    }
}
