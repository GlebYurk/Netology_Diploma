using agent;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class Henkoten : Agent, IAgent
{

    // Start is called before the first frame update
    

    BasicAtackAction _action;

    float timer;
    private Renderer _renderer;
    public void Spell()
    {
        return;
    }

    public void SpellPassive(float t)
    {
        if (timer <= 6)
        {
            timer += t;
        }
        else
        {
            timer = timer - 6 + t;
            if (_renderer.material.color == Color.red)
                _renderer.material.color = Color.black;
            else
                _renderer.material.color = Color.red;

            _action.GetLiveForm().ChangeTypeAction();
            Debug.Log("Maria Spell");

        }
    }

    public GameObject GetAgentObject() { return this.gameObject; }

    public void SetAgentAction(TileController tileController) 
    {
        List<Point> radius = new List<Point>() { new Point(0,1) };
        _action = new BasicAtackAction(new GeneralLiveForm(10, 4, 4, 8, true, 2, this.gameObject), tileController, 2, radius);
        
        _renderer=_action.GetLiveForm().GetGameObject().transform.GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>();
        _renderer.material.color =Color.red;
        timer = 0;
    }
    public BasicAtackAction GetAgentAction()
    {
        return _action;
    }

    
}
