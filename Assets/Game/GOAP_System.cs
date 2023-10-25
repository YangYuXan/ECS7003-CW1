using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldState
{
    //public Dictionary<string, bool> _WorldStae = new Dictionary<string, bool>();
}

public class Goal
{
    //public string goalName;
    //public Dictionary<string, bool> goalCondition = new Dictionary<string, bool>();
}

public class Action
{
    public string actionName;
    public Dictionary<string, bool> preconditions = new Dictionary<string, bool>();
    public Dictionary<string, bool> effects = new Dictionary<string, bool>();
    public int cost;

    public void Execute()
    {
        Debug.Log("Executing Action: " + actionName);
    }
}

public class GOAPPlanner
{
    public List<Action> Plan(List<Action> availableActions, Goal goal, WorldState currentState)
    {
        // 实现GOAP算法，生成计划
        List<Action> plan = new List<Action>();

        // 示例：简单地选择第一个可用动作作为计划
        if (availableActions.Count > 0)
        {
            plan.Add(availableActions[0]);
        }

        return plan;
    }
}

public class GOAP_System : MonoBehaviour
{

    //World State
    public string[] States;
    public bool[] StateValue;
    Dictionary<string, bool> _WorldStae = new Dictionary<string, bool>();

    //Goal
    public string goalName;
    public string[] goalPreconditonName;
    public bool[] goalPrecondition;
    Dictionary<string, bool> _Goal = new Dictionary<string, bool>();

    //Action
    public string actionName;
    public 



    void Start()
    {
        for (int i = 0; i < States.Length; i++)
        {
            _WorldStae.Add(States[i], StateValue[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
