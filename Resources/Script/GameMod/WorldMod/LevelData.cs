using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class LevelData
{
    private static string _objectOnLevel;
    private static int _objectData;
    private static int _subDefeatState;

    public bool CheckLevelState()
    {
        if (_objectData == 0)
            return true;
        else 
            return false;
    }

    public bool CheckSubDefeat()
    {
        
        if (_subDefeatState == 0)
            return true;
        else
            return false;
    }

    public void OnTriger(string Trigger)
    {
        switch (_objectOnLevel)
        {
            case "Defend":
                if (Trigger == "DeadEnemy")
                {
                    _objectData--;
                    Debug.Log("_objectData - " + _objectData);
                }
                if (Trigger == "EnemyOnObject")
                {
                    _subDefeatState--;
                    Debug.Log("subDefeat - " + _subDefeatState);
                }
                break;
            case "Kill":
                if (Trigger == "DeadEnemy")
                {
                    _objectData--;
                    Debug.Log("_objectData - " + _objectData);
                }
                break;

        }

    }
   
    public void OnLevelSet(string objectOnLevel, int objectData, int subDefeatState)
    {
        _objectData = objectData;
        _objectOnLevel = objectOnLevel;
        _subDefeatState = subDefeatState;
    }

}
