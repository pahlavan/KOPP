using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> OutgoingPlanets;
    public Text text;
    public bool isSelected = false;

    private int selectionTime = 0;
    private SpriteRenderer spriteRenderer;

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
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
            selectionTime = 50;
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
