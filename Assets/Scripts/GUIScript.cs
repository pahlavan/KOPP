using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Block,
    Delay,
    Suggest,
}

public class Action
{
    public ActionType type;
    public int power;
}

public class GUIScript : MonoBehaviour
{
    public int Points = 0;
    public List<Sprite> dangerDialSprites;
    public List<Sprite> detourDialSprites;
    public List<Sprite> randomDialSprites;

    public static int MaxPower = 3;
    private List<List<Sprite>> sprites;

    private List<Action> actions = new List<Action>(){
        new Action() { type = ActionType.Block, power = 1 },
        new Action() { type = ActionType.Delay, power = 0 },
        new Action() { type = ActionType.Suggest, power = 0 },
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

    // Update is called once per frame
    void Update()
    {
        if (timer++ % 100 == 0) Points++;

        GameObject.Find("PointsText").GetComponent<Text>().text = "Points: " + Points;

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
        if (CanUpgrade(action)) {
            Points -= GetUpgradeCost(action.type, action.power);
            action.power++;
        }
    }

    int GetUpgradeCost(ActionType type, int power)
    {
        if (power >= MaxPower) return 0;

        switch (type)
        {
            case ActionType.Block: return (new List<int>() { 3, 4, 5 })[power];
            case ActionType.Delay: return (new List<int>() { 4, 5, 6 })[power];
            case ActionType.Suggest: return (new List<int>() { 5, 6, 7 })[power];
        }

        return 0;
    }
}
