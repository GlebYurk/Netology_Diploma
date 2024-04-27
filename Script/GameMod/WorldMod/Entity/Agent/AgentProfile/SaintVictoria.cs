using agent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;
public class SaintVictoria : Agent, IAgent
{


    BasicAtackAction _action;

    float timer;
    public void Spell()
    {
        return;
    }

    public void SpellPassive(float t)
    {
        if (timer<=3)
        {
            timer+=t;
        }
        else
        {
            timer= timer-3+t;

            _action.GetLiveForm().HPRegen(2);
            Debug.Log("Maria Spell");

        }
    }

    public GameObject GetAgentObject() { return this.gameObject; }

    public void SetAgentAction(TileController tileController) 
    {
        List<Point> radius = new List<Point>() { new Point(0,1), new Point (0,2)};
        _action = new BasicAtackAction(new GeneralLiveForm(15, 3, 2, 3, true, 2, this.gameObject), tileController, 2, radius);
        
        timer = 0;
    }
    public BasicAtackAction GetAgentAction()
    {
        return _action;
    }

    public override string ToString()
    {
        GeneralLiveForm generalLiveForm = _action.GetLiveForm();
        return (generalLiveForm.ToString() + " Couldown: " + Math.Round(3 - timer));
    }
}
