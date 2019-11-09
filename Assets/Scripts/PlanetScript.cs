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
    public GameObject actionMenuHandler;
    public List<GameObject> Actions;
    
    private GameObject actionMenu;
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
        isSelected = true;
    }

    private void OnMouseDown()
    {
        SelectPlanet();
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

    private void OnMouseEnter()
    {
        ShowActionMenu();
    }

    public void ShowActionMenu()
    {
        switch (ActionMenuScript.enabledActions.Count)
        {
            case 0:
                ActionMenuScript.enabledActions.Add(Actions[0]);
                break;
            case 1:
                ActionMenuScript.enabledActions.Add(Actions[1]);
                break;
            case 2:
                ActionMenuScript.enabledActions.Add(Actions[2]);
                break;
            default:
                break;
        }

        actionMenu = Instantiate(actionMenuHandler);
        actionMenu.GetComponent<ActionMenuScript>().Respwan(gameObject);
    }

    private void OnMouseExit()
    {
        HideActionMenu();
    }

    public void HideActionMenu()
    {
        actionMenu.GetComponent<ActionMenuScript>().Collapse();
    }
}
