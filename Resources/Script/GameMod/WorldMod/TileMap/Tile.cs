using System.Collections.Generic;
using UnityEngine;
using WorldMode;

public class Tile
{
    public float x, z;
    private string _type;
    private BasicAtackAction _occupation;

    public event EventHandler OnRadiusEnterEventHandler;
    public event EventHandler OnRadiusExitEventHandler;
    public delegate void EventHandler(BasicAtackAction basicAtackAction);

    private List<GameObject> inRadiusOfAttack;

    public Tile(float x, float z, string Type)
    {
        this.x = x;
        this.z = z;
        _type = Type;
        _occupation = null;
        inRadiusOfAttack = new List<GameObject>();
    }

    public BasicAtackAction GetOccupation() { return _occupation; }
    public string GetTileType() { return _type; }

    public void NullOccupation(BasicAtackAction basicAction)
    {
        _occupation = null;
        if (OnRadiusExitEventHandler != null)
            OnRadiusExitEventHandler.Invoke(basicAction);
    }

    public void SetOccupation(BasicAtackAction basicAction)
    {
        _occupation = basicAction;
        if (OnRadiusEnterEventHandler!=null)
        OnRadiusEnterEventHandler.Invoke(basicAction);
    }

    public bool CheckOccupation()
    {
        if (_occupation == null)
            return false;
        else
            return true;
    }

    public void AddInRadiusOf(EventHandler OnRadiusEnter, EventHandler OnRadiusExit)
    {
        OnRadiusEnterEventHandler -= OnRadiusEnter;
        OnRadiusExitEventHandler -= OnRadiusExit;
        OnRadiusEnterEventHandler += OnRadiusEnter;
        OnRadiusExitEventHandler += OnRadiusExit;
       // Debug.Log("Tile" + this.x + "_" + this.z + " in radius enter");
    }
    public void RemoveInRadiusOf(EventHandler OnRadiusEnter, EventHandler OnRadiusExit) 
    {

        OnRadiusEnterEventHandler -= OnRadiusEnter;
        OnRadiusExitEventHandler -= OnRadiusExit;
      //  Debug.Log("Tile" + this.x + "_" + this.z + " in radius exit");
    }
 
}


