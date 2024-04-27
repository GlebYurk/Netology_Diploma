using agent;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using WorldMode;
using WorldOrder.Generic;

public class BasicAtackAction : BasicAction, IRadiusAction
{

    protected float _atackDelay;
    protected List<BasicAtackAction> _targetLiveForm;
    protected IEnumerator _atackCoroutine;
    protected bool _atackRunning;
    protected List<Point> _radius;

    public event AtackEventHandler OnAtackEventHandler;
    public delegate void AtackEventHandler(GeneralLiveForm generalLiveForm);

    protected AudioSource _audioSource;
    protected Animator _animator;
    public BasicAtackAction(GeneralLiveForm actionLiveForm, TileController tileController, float atackDelay, List<Point> radius) : base(actionLiveForm, tileController)
    {
        _atackDelay = atackDelay;
        _actionLiveForm = actionLiveForm;
        _targetLiveForm = new List<BasicAtackAction>();
        _atackRunning = false;
        _radius = radius;

        _audioSource = actionLiveForm.GetGameObject().GetComponent<AudioSource>();
        _animator = actionLiveForm.GetGameObject().GetComponent<Animator>();
    }


    protected void OnDead()
    {
        LevelData levelData = new LevelData();
        levelData.OnTriger("Dead" + _actionLiveForm.GetGameObject().tag);
        castRadiusExit();
        GetTile().NullOccupation(this);

        _live = false;
    }

    public  List<BasicAtackAction> GetTargetList(){ return _targetLiveForm; }


    public void OnRadiusEnterAction(BasicAtackAction liveForm)
    {
        if (liveForm.IsLive())
        {
            if (liveForm._actionLiveForm.GetGameObject().tag != this._actionLiveForm.GetGameObject().tag)
            {
                _targetLiveForm.Add(liveForm);
                //Debug.Log(_actionLiveForm.GetGameObject().name + " eat " + liveForm.GetLiveForm().GetGameObject().name);
                if (!_atackRunning && _actionLiveForm.GetGameObject() != liveForm.GetLiveForm().GetGameObject())
                {
                    _atackRunning = true;
                    _coroutine.Add(Atack());
                }
            }
        }


    }
    public void OnRadiusExitAction(BasicAtackAction liveForm)
    {
        if(liveForm.IsLive())
        if (liveForm._actionLiveForm.GetGameObject().tag != this._actionLiveForm.GetGameObject().tag)
        {
            _targetLiveForm.Remove(liveForm);
           // Debug.Log(_actionLiveForm.GetGameObject().name + " lost " + liveForm.GetLiveForm().GetGameObject().name);
        }

    }
    IEnumerator Atack()
    {

        while (_targetLiveForm.Count != 0)
        {
          //  Debug.Log("Atack");
            for (int i = 0; i < _targetLiveForm.Count ; i++)
            {
                if (_targetLiveForm[i].GetLiveForm().GetGameObject() != null)
                {
                    if (_targetLiveForm[i] != null)
                    {
                        if (_targetLiveForm[i].IsLive())
                        {
                            _audioSource.Play();
                            _animator.SetTrigger("TrAtack");
                            if (_targetLiveForm[0].GetLiveForm().GetDamage(_actionLiveForm.GetAtackType(), _actionLiveForm.GetAtack(), 0))
                            {
                                _targetLiveForm[0].OnDead();
                            }
                            else
                            if(_targetLiveForm[0].OnAtackEventHandler!=null && _targetLiveForm[0]!=null)
                            _targetLiveForm[0].OnAtackEventHandler.Invoke(_targetLiveForm[0].GetLiveForm());
                            yield return new WaitForSeconds(_atackDelay);
                            break;
                        }
                    }
                    if (_targetLiveForm.Count == i - 1)
                    { yield break; }
                }
                else 
                { 
                _targetLiveForm.RemoveAt(i);
                    i--;
                }
            }
        }
        //Debug.Log("Retreat");
        _atackRunning = false;
      yield return this;  
    }


    public void castRadiusEnter()
    {
        foreach (Point tileCoordinate in _radius)
        {
            float x = (float)tileCoordinate.X;
            float z = (float)tileCoordinate.Y;

            NormalizeToRotate(ref x, ref z, _actionLiveForm.GetGameObject().transform.eulerAngles.y);
            
            Tile tile = _tileController.GetTile((int)((_startMove.x + x) / WorldOrderConfig.tileScale), (int)((_startMove.z + z) / WorldOrderConfig.tileScale));
            if (tile.GetOccupation() != null)
                OnRadiusEnterAction(tile.GetOccupation());
            tile.AddInRadiusOf(OnRadiusEnterAction, OnRadiusExitAction);
        }
    }


    public void castRadiusExit() 
    {
        foreach (Point tileCoordinate in _radius)
        {
            float x = (float)tileCoordinate.X;
            float z = (float)tileCoordinate.Y;

            NormalizeToRotate(ref x, ref z, _actionLiveForm.GetGameObject().transform.eulerAngles.y);

            Tile tile = _tileController.GetTile((int)((_startMove.x + x) / WorldOrderConfig.tileScale), (int)((_startMove.z + z) / WorldOrderConfig.tileScale));
            if (tile.GetOccupation() != null)
                OnRadiusExitAction(tile.GetOccupation());
            tile.RemoveInRadiusOf(OnRadiusEnterAction, OnRadiusExitAction);
        }
    }


    public void SetStartPositin(Vector3 startRotation, Vector3 startMove)
    {
        _startMove = startMove;
        _startRotation = startRotation;
    }


    public IEnumerator GetEnumerator() { return _atackCoroutine; }
    public bool CheckLiveStatus() { return _live; }

    public void TeleportToTile(Tile td)
    {
        GetTile().NullOccupation(this);
        castRadiusExit();
        Vector3 vector3 = new Vector3(td.x,_startMove.y,td.z);
        _startMove = vector3;
        _endMove = _startMove;
        if (_actionLiveForm.GetGameObject().transform.parent.gameObject.name == "Player")
            _actionLiveForm.GetGameObject().transform.parent.position = vector3;
        else
            _actionLiveForm.GetGameObject().transform.position = vector3;
        castRadiusEnter();
    }

}
