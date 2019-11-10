﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D;
using UnityEditorInternal;
using System;
using UnityEditor;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> OutgoingPlanets;
    public bool isSelected = false;
    public List<Button> UIButtons;
    public static IList<string> EnabledActions = new List<string>();
    public float menuAnimationDuration;

    private MenuState menuState;
    private int selectionTime = 0;
    private SpriteRenderer spriteRenderer;
    private int menuTransitionStep;
    private int menuTotalSteps;
    private float menuRadius;
    private int menuDegreeOffset;
    private List<Button> activeButtons;
    private float originalColliderRadius;
    private float menuColliderRadius;

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
        isSelected = true;
    }

    void MoveMenuButtons(int step)
    {
        if (step < 0 || step > menuTotalSteps) { throw new ArgumentException(nameof(step)); }

        int percent = step * 100 / menuTotalSteps;

        int i = 0;
        foreach (Button action in activeButtons)
        {
            var vec = Quaternion.Euler(0, 0, menuDegreeOffset * i++) * new Vector3(0, menuRadius * percent / 100, 0);
            action.transform.localPosition = vec;
        }
    }

    public void ShowActionMenu()
    {
        switch (EnabledActions.Count)
        {
            case 0:
                EnabledActions.Add(UIButtons[0].name);
                break;
            case 1:
                EnabledActions.Add(UIButtons[1].name);
                break;
            case 2:
                EnabledActions.Add(UIButtons[2].name);
                break;
            default:
                break;
        }

        gameObject.GetComponent<CircleCollider2D>().radius = menuColliderRadius;

        switch (menuState)
        {
            case MenuState.Collapsing:
                menuTransitionStep = menuTotalSteps - menuTransitionStep;
                menuState = MenuState.Respawning;
                break;
            case MenuState.Respawning:
                break;
            case MenuState.Active:
                break;
            case MenuState.Disable:
                menuTransitionStep = menuTotalSteps;
                menuState = MenuState.Respawning;
                break;
        }

        activeButtons = new List<Button>();
        foreach(var button in UIButtons)
        {
            if (EnabledActions.Contains(button.name))
            {
                activeButtons.Add(button);
            }
        }

        menuDegreeOffset = 360 / activeButtons.Count;
    }

    public void CollapseMenu()
    {
        menuState = MenuState.Collapsing;
        menuTransitionStep = menuTotalSteps;
        gameObject.GetComponent<CircleCollider2D>().radius = originalColliderRadius;
    }

    void CheckSelection()
    {
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

    void AnimateMenu()
    {
        if (menuTransitionStep > 0)
        {
            menuTransitionStep--;

            switch (menuState)
            {
                case MenuState.Respawning:
                    MoveMenuButtons(menuTotalSteps - menuTransitionStep);
                    break;
                case MenuState.Collapsing:
                    MoveMenuButtons(menuTransitionStep);
                    break;
            }

            if (menuTransitionStep <= 0)
            {
                switch (menuState)
                {
                    case MenuState.Respawning:
                        menuState = MenuState.Active;
                        break;
                    case MenuState.Collapsing:
                        menuState = MenuState.Disable;
                        break;
                }
            }
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
        menuTotalSteps = (int)(menuAnimationDuration / Time.fixedDeltaTime);
        menuTransitionStep = 0;
        menuRadius = spriteRenderer.bounds.size.x * 0.7f;
        menuState = MenuState.Disable;
        originalColliderRadius = gameObject.GetComponent<CircleCollider2D>().radius;
        menuColliderRadius = originalColliderRadius * 2.2f;
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
        if (menuState != MenuState.Active && menuState != MenuState.Disable)
        {
            AnimateMenu();
        }
    }

    public void OnMouseEnter()
    {
        ShowActionMenu();
    }

    public void DangerZoneAction()
    {
        Debug.Log("DangerZoneAction");
        CollapseMenu();
    }

    public void DetourAction()
    {
        Debug.Log("DetourAction");
        CollapseMenu();
    }

    public void SecurityCheckAction()
    {
        Debug.Log("SecurityCheckAction");
        CollapseMenu();
    }

    public void OnMouseExit()
    {
        CollapseMenu();
    }

    enum MenuState
    {
        Respawning,
        Active,
        Collapsing,
        Disable,
    }
}
