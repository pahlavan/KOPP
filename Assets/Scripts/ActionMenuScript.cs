using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ActionMenuScript : MonoBehaviour
{
    public static IList<GameObject> enabledActions = new List<GameObject>();

    public float duration;

    private Vector3 location;
    private GameObject planet;
    private float radius;
    private int degreeOffset;
    private IList<GameObject> activeActions;
    private MenuState state;
    private int transitionStep;
    private int totalSteps;

    public void Initialize(GameObject planet)
    {
        state = MenuState.Idle;
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
        foreach (GameObject action in enabledActions)
        {
            var instance = Instantiate(action, location, Quaternion.identity);
            activeActions.Add(instance);
        }

        duration = 0.3f;
        transitionStep = 0;
    }

    void MoveActions(int percent)
    {
        if (percent < 0 || percent > 100) { throw new ArgumentException(nameof(percent)); }

        int i = 0;
        foreach (GameObject action in activeActions)
        {
            var vec = Quaternion.Euler(0, 0, degreeOffset * i++) * new Vector3(0, radius * percent / 100, 0);
            action.transform.position = location + vec;
        }
    }

    public void Respwan(GameObject planet)
    {
        Initialize(planet);
        state = MenuState.Respawning;
        if (activeActions.Count > 1)
        {
            planet.GetComponent<CircleCollider2D>().radius *= 2;
        }
        totalSteps = transitionStep = CalculateDurationInStep(duration);
    }

    public void Collapse()
    {
        totalSteps = transitionStep = CalculateDurationInStep(duration);
        state = MenuState.Collapsing;
        if (activeActions.Count > 1)
        {
            planet.GetComponent<CircleCollider2D>().radius /= 2;
        }
    }

    int CalculateDurationInStep(float duration)
    {
        return (int)(duration / Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if (transitionStep > 0)
        {
            transitionStep--;

            switch (state)
            {
                case MenuState.Respawning:
                    MoveActions(100 - transitionStep * 100 / totalSteps);
                    break;
                case MenuState.Collapsing:
                    MoveActions(transitionStep * 100 / totalSteps);
                    break;
            }

            if (transitionStep <= 0)
            {
                switch (state)
                {
                    case MenuState.Respawning:
                        state = MenuState.Idle;
                        break;
                    case MenuState.Collapsing:
                        foreach (GameObject instance in activeActions)
                        {
                            Destroy(instance);
                        }

                        Destroy(this);
                        break;
                }
            }
        }
    }

    enum MenuState
    {
        Respawning,
        Idle,
        Collapsing,
    }
}
