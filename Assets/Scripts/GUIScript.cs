using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum ActionType
{
    DangerZone,
    SecurityCheck,
    Detour,
}

public class Action
{
    public ActionType type;
    public int power;
}

public class GUIScript : MonoBehaviour
{
    public int Points = 0;
    public int Heat = 0;
    public bool isOverheat = false;

    public List<Sprite> dangerDialSprites;
    public List<Sprite> detourDialSprites;
    public List<Sprite> randomDialSprites;

    public static readonly int MaxPower = 3;
    public static readonly int MaxHeat = 10;
    public static readonly int OverheatCooldown = 5;

    private List<List<Sprite>> sprites;
    private int overheatCounter = 0;

    private List<Action> actions = new List<Action>(){
        new Action() { type = ActionType.DangerZone, power = 1 },
        new Action() { type = ActionType.Detour, power = 0 },
        new Action() { type = ActionType.SecurityCheck, power = 0 },
    };

    private int timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprites = new List<List<Sprite>>()
        {
            dangerDialSprites,
            detourDialSprites,
            randomDialSprites,
        };
    }

    void FixedUpdate()
    {
        if (isOverheat)
        {
            overheatCounter++;
            if (overheatCounter * Time.fixedDeltaTime >= OverheatCooldown)
            {
                isOverheat = false;
                Heat = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer++ % 100 == 0) Points++;

        GameObject.Find("PointsText").GetComponent<Text>().text = "Points: " + Points;
        Text heatText = GameObject.Find("HeatText").GetComponent<Text>();
        heatText.text = "Heat: " + Heat;
        heatText.color = isOverheat ? Color.red : Color.black;

        var gauges = GameObject.FindGameObjectsWithTag("Gauge");

        int i = 0;
        foreach (Action action in actions)
        {
            // var mainGaugeImage = gauges[i].transform.Find("mainGauge").GetComponent<Image>();
            var mainGaugeImage = gauges[i].GetComponent<Image>();
            mainGaugeImage.sprite = sprites[i][action.power];

            //var upgradeImage = gauges[i].transform.Find("upgrade").GetComponent<Image>();
            //upgradeImage.color = CanUpgrade(action) ? Color.green : Color.red;

            var upgradeText = gauges[i].transform.Find("UpgradeText").GetComponent<Text>();
            int cost = GetUpgradeCost(action.type, action.power);
            upgradeText.text = cost > 0 ? cost.ToString() : "";

            //for (int p = 1; p <= MaxPower; p++)
            //{
            //    var costText = gauges[i].transform.Find("cost" + p).GetComponent<Text>();
            //    costText.text = GetUpgradeCost(action.type, action.power).ToString();
            //}

            i++;
        }
    }

    bool CanUpgrade(Action action)
    {
        return action.power < MaxPower && Points >= GetUpgradeCost(action.type, action.power);
    }

    public void OnUpgradeClick(int index)
    {
        var action = actions[index];
        if (CanUpgrade(action))
        {
            Points -= GetUpgradeCost(action.type, action.power);
            action.power++;
        }
    }

    public void OnActionPerformed(ActionType type)
    {
        var action = actions.Find(a => a.type == type);
        Heat = Math.Min(MaxHeat, Heat + GetActionHeat(action.type, action.power));
        if (Heat == MaxHeat)
        {
            isOverheat = true;
            overheatCounter = 0;
        }
    }

    public List<ActionType> GetEnabledActions()
    {
        return actions.FindAll(a => a.power > 0).Select(a => a.type).ToList();
    }

    int GetUpgradeCost(ActionType type, int power)
    {
        if (power >= MaxPower) return 0;

        switch (type)
        {
            case ActionType.DangerZone: return (new List<int>() { 3, 4, 5 })[power];
            case ActionType.SecurityCheck: return (new List<int>() { 4, 5, 6 })[power];
            case ActionType.Detour: return (new List<int>() { 5, 6, 7 })[power];
        }

        return 0;
    }
    
    int GetActionHeat(ActionType type, int power)
    {
        if (power == 0) return 0;

        switch (type)
        {
            case ActionType.DangerZone: return (new List<int>() { 3, 4, 5 })[power - 1];
            case ActionType.SecurityCheck: return (new List<int>() { 4, 5, 6 })[power - 1];
            case ActionType.Detour: return (new List<int>() { 5, 6, 7 })[power - 1];
        }

        return 0;
    }
}
