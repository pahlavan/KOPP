using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D;
using UnityEditorInternal;
using System;
using UnityEditor;
using System.Linq;
using UnityEditor.Experimental.U2D;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> OutgoingPlanets;
    public List<Button> UIButtons;
    public float menuAnimationDuration;
    public PlanetState planetState;
    public GameObject CooldownAnimation;
    public Material pathMaterial;
    public Color pathColor;

    private MenuState menuState;
    private SpriteRenderer spriteRenderer;
    private int menuTransitionStep;
    private int menuTotalSteps;
    private float menuRadius;
    private int menuDegreeOffset;
    private List<Button> activeButtons;
    private float originalColliderRadius;
    private float menuColliderRadius;
    private GUIScript guiScript;

    private Animator planetAnimator;
    private float planetStateTransitionTime;
    private string distinguishedMenuButton;
    private bool includeDistinguiedButton;
    private string activeAction;

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.material = pathMaterial;
        lr.startColor = pathColor;
        lr.endColor = pathColor;
        lr.startWidth = 4.0f;
        lr.endWidth = 4.0f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void MoveMenuButtons(int step)
    {
        if (step < 0 || step > menuTotalSteps) { throw new ArgumentException(nameof(step)); }

        int percent = step * 100 / menuTotalSteps;

        int i = 0;
        foreach (Button action in activeButtons)
        {
            if (distinguishedMenuButton == null || (includeDistinguiedButton && action.name == distinguishedMenuButton)
               || (!includeDistinguiedButton && action.name != distinguishedMenuButton))
            {
                var vec = Quaternion.Euler(0, 0, menuDegreeOffset * i) * new Vector3(0, menuRadius * percent / 100, 0);
                action.transform.localPosition = vec;
            }

            i++;
        }
    }

    public void ShowActionMenu()
    {
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
            if (guiScript.GetEnabledActions().Select(t => t.ToString()).Contains(button.name))
            {
                button.gameObject.SetActive(true);
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

    void EnableMenuActions(bool state)
    {
        foreach(var button in UIButtons)
        {
            button.interactable = state || button.name == activeAction;
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

                distinguishedMenuButton = null;
            }
        }
    }

    void AnimatePlanet()
    {
        if (Time.time >= planetStateTransitionTime)
        {
            planetState = PlanetState.Available;
            CooldownAnimation.SetActive(false);
            distinguishedMenuButton = activeAction;
            includeDistinguiedButton = true;
            CollapseMenu();
            activeAction = null;
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
        menuTotalSteps = CalculateStepCount(menuAnimationDuration);
        menuTransitionStep = 0;
        menuRadius = spriteRenderer.bounds.size.x * 0.7f;
        menuState = MenuState.Disable;
        originalColliderRadius = gameObject.GetComponent<CircleCollider2D>().radius;
        menuColliderRadius = originalColliderRadius * 2.2f;
        guiScript = GameObject.Find("GUICanvas").GetComponent<GUIScript>();

        planetState = PlanetState.Available;
        planetAnimator = CooldownAnimation.GetComponent<Animator>();
        distinguishedMenuButton = null;
        activeAction = null;
    }
    
    void Update()
    {
        if (planetState != PlanetState.Available || guiScript.isOverheat)
        {
            EnableMenuActions(false);
        }
        else
        {
            EnableMenuActions(true);
        }
    }

    int CalculateStepCount(float duration)
    {
        return (int)(duration / Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (menuState != MenuState.Active && menuState != MenuState.Disable)
        {
            AnimateMenu();
        }

        if (planetState != PlanetState.Available)
        {
            AnimatePlanet();
        }
    }

    public void OnMouseEnter()
    {
        if (activeAction != null || distinguishedMenuButton != null) return;

        if (OutgoingPlanets.Count == 0)
        {
            EnableMenuActions(false);
        }
        else
        {
            ShowActionMenu();
        }
    }

    void TriggerTimerAnimation(float duration)
    {
        CooldownAnimation.SetActive(true);
        planetAnimator.speed = 1f / duration;
        planetAnimator.Play("Cooldown");
    }

    void PrepareActionExecution(float duration, string action)
    {
        activeAction = action;
        distinguishedMenuButton = activeAction;
        includeDistinguiedButton = false;
        CollapseMenu();
        TriggerTimerAnimation(duration);
        planetStateTransitionTime = Time.time + duration;
    }

    public void DangerZoneAction()
    {
        if (activeAction != null) return;

        planetState = PlanetState.DangerZone;
        int duration = guiScript.OnActionPerform(ActionType.DangerZone);
        PrepareActionExecution(duration, "DangerZone");
    }

    public void DetourAction()
    {
        if (activeAction != null) return;
        
        planetState = PlanetState.Detour;
        int duration = guiScript.OnActionPerform(ActionType.Detour);
        PrepareActionExecution(duration, "Detour");
    }

    public void SecurityCheckAction()
    {
        if (activeAction != null) return;
        
        planetState = PlanetState.SecurityCheck;
        int duration = guiScript.OnActionPerform(ActionType.SecurityCheck);
        PrepareActionExecution(duration, "SecurityCheck");
    }

    public void OnMouseExit()
    {
        if (activeAction != null || distinguishedMenuButton != null) return;

        if (OutgoingPlanets.Count != 0)
        {
            CollapseMenu();
        }
    }

    enum MenuState
    {
        Respawning,
        Active,
        Collapsing,
        Disable,
    }
}

public enum PlanetState
{
    Available,
    DangerZone,
    Detour,
    SecurityCheck,
}
