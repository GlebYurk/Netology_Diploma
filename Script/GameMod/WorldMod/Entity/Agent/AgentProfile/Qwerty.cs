using agent;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class Qwerty : Agent, IAgent
{    

    BasicAtackAction _action;

    Tile TP;
    public void Spell()
    {
        if (TP==null)
        {
            TP=_action.GetTileStart(); 
        }
        else
        {
            if (TP.GetOccupation() == null)
            {
                _action.TeleportToTile(TP);
                TP = null;
            }
            else
                TP = null;
        }
    }
    public void SpellPassive(float t) { return; }
    public GameObject GetAgentObject() { return this.gameObject; }

    public void SetAgentAction(TileController tileController) 
    {
        List<Point> radius = new List<Point>() { new Point(0,1) };
        _action = new BasicAtackAction(new GeneralLiveForm(18, 3, 4, 4, false , 2, this.gameObject), tileController, 2, radius);
       
        TP= null;
    }
    public BasicAtackAction GetAgentAction()
    {
        return _action;
    }

    public override string ToString()
    {
        GeneralLiveForm generalLiveForm = _action.GetLiveForm();
        return (generalLiveForm.ToString() + (TP == null ? " Нет маяка":" Маяк готов")) ;
    }

}
