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
    public int Damage = 0;
    public float Heat = 0;
    public bool isOverheat = false;

    public List<Sprite> dangerDialSprites;
    public List<Sprite> detourDialSprites;
    public List<Sprite> randomDialSprites;

    public static readonly int MaxPower = 3;
    public static readonly int OverheatCooldown = 25;
    public static readonly float MaxHeat = 20;

    private List<List<Sprite>> sprites;
    private SpriteRenderer bossHologram;
    private GameObject[] gauges;
    private GameObject[] damageIcons;
    private float startTime;
    private DialogScript dialog;
    private bool isGameOver = false;
    private float finishTime = 0;

    private List<Action> actions = new List<Action>(){
        new Action() { type = ActionType.DangerZone, power = 1 },
        new Action() { type = ActionType.Detour, power = 0 },
        new Action() { type = ActionType.SecurityCheck, power = 0 },
    };

    private float incomeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        sprites = new List<List<Sprite>>()
        {
            dangerDialSprites,
            detourDialSprites,
            randomDialSprites,
        };

        bossHologram = GameObject.Find("BossHologram").GetComponent<SpriteRenderer>();
        gauges = GameObject.FindGameObjectsWithTag("Gauge");
        damageIcons = GameObject.FindGameObjectsWithTag("DamageIcon");
        dialog = GameObject.Find("DialogCanvas").GetComponent<DialogScript>();
        incomeTime = Time.time;
    }

    void FixedUpdate()
    {
        float delta = MaxHeat / ((float)OverheatCooldown / Time.fixedDeltaTime);
        if (isOverheat)
            delta *= 3;

        Heat = Math.Max(0, Heat - delta);
        bossHologram.enabled = isOverheat || ((Heat / MaxHeat) > 0.65);
        
        if (isOverheat && Heat == 0)
        {
            isOverheat = false;
        }
    }

    public void IncDamage()
    {
        Damage++;
        if (Damage >= damageIcons.Length)
        {
            isGameOver = true;
            GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
            finishTime = Time.time;
        }
    }

    float GetIncomeDelay()
    {
        // start with every 5s with max penalty of 10 
        float penalty = (Heat / MaxHeat) * 10;
        return 5 + penalty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - incomeTime > GetIncomeDelay())
        {
            Points++;
            incomeTime = Time.time;
        }

        GameObject.Find("PointsText").GetComponent<Text>().text = "Points: " + Points;
        TimeSpan timeSpan = TimeSpan.FromSeconds((isGameOver ? finishTime : Time.time) - startTime);
        string timeStr = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        GameObject.Find("Timer").GetComponent<Text>().text = timeStr;
        //Text heatText = GameObject.Find("HeatText").GetComponent<Text>();
        //heatText.text = "Heat: " + Heat;
        //heatText.color = isOverheat ? Color.red : Color.black;
        //Debug.Log("Heat: " + Heat);
        GameObject.Find("HeatBar").GetComponent<Warning_ctrl>().WarningLevel = (int)(((float)Heat / MaxHeat) * 8.0);

        int i = 0;
        foreach (Action action in actions)
        {
            // var mainGaugeImage = gauges[i].transform.Find("mainGauge").GetComponent<Image>();
            var mainGaugeImage = gauges[i].GetComponent<Image>();
            mainGaugeImage.sprite = sprites[i][action.power];

            var upgradeImage = gauges[i].transform.Find("Panel").GetComponent<Image>();
            upgradeImage.color = CanUpgrade(action) ? Color.green : Color.red;
            upgradeImage.enabled = action.power < MaxPower;

            var upgradeText = gauges[i].transform.Find("UpgradeText").GetComponent<Text>();
            int cost = GetUpgradeCost(action.type, action.power);
            upgradeText.text = cost > 0 ? cost.ToString() : "";

            i++;
        }

        for (int d = 1; d < damageIcons.Length + 1; d++)
        {
            damageIcons[d - 1].GetComponent<Image>().color = Damage >= d ? Color.red : new Color(255, 0, 0, 0.5f);
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

    public int OnActionPerform(ActionType type)
    {
        if (isOverheat) return 0;

        var action = actions.Find(a => a.type == type);
        float prevHeat = Heat;
        Heat = Math.Min(MaxHeat, Heat + GetActionHeat(action.type, action.power));
        if (Heat == MaxHeat)
        {
            isOverheat = true;
            dialog.showNewMessage(DialogSource.Commander, "Enough! Don't touch the system. Let's see what's going on!");
        }

        if (Heat > prevHeat && ((Heat / MaxHeat) > 0.65) && ((prevHeat / MaxHeat) < 0.65))
        {
            dialog.showNewMessage(DialogSource.Commander, "What's up with all the alerts? This is not normal...");
        }

        return GetActionDuration(type);
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
            case ActionType.DangerZone: return (new List<int>() { 3, 4, 6 })[power];
            case ActionType.SecurityCheck: return (new List<int>() { 3, 5, 6 })[power];
            case ActionType.Detour: return (new List<int>() { 3, 6, 7 })[power];
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

    public int GetActionDuration(ActionType type, int power)
    {
        if (power < 0 || power > MaxPower) return 0;

        switch (type)
        {
            case ActionType.DangerZone: return (new List<int>() { 8, 11, 14 })[power-1];
            case ActionType.SecurityCheck: return (new List<int>() { 8, 11, 14 })[power-1];
            case ActionType.Detour: return (new List<int>() { 8, 11, 14 })[power-1];
        }

        return 0;
    }

    public int GetActionDuration(ActionType type)
    {
        var action = actions.Find(a => a.type == type);
        if (action != null)
        {
            return GetActionDuration(type, action.power);
        }

        return 0;
    }

    public int GetActionCooldownDuration()
    {
        return 4;
    }
}
