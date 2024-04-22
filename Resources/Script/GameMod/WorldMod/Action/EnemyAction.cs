using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using WorldOrder.Generic;

public class EnemyAction : BasicAtackAction
{
    int _indexOfAction;
    string _input;

    public EnemyAction(GeneralLiveForm _actionLiveForm,  TileController _tileController,float atackDelay, string input,List<Point> radius) : base(_actionLiveForm, _tileController, atackDelay,radius)
    {
        _indexOfAction = 0;
        _inAction = false;
        _startRotation = _actionLiveForm.GetGameObject().transform.eulerAngles;
        _startMove = _actionLiveForm.GetGameObject().transform.position;
        _input = input;

    }

    public void ActionFlow()
    {
        if (_input.Length > _indexOfAction)
        {
            if (_actionLiveForm != null)
            {
                if (!_coroutineRunning)
                {
                    if (!_inAction)
                    {
                        switch (_input[_indexOfAction])
                        {

                            case 'Q':
                                rotateLeft();
                                _inAction = true;
                                castRadiusExit();
                                break;

                            case 'E':
                                rotateRight();
                                _inAction = true;
                                castRadiusExit();
                                break;

                            case 'S':
                                moveDecisionMaker(0, 0, -WorldOrderConfig.tileScale);
                                break;

                            case 'W':
                                moveDecisionMaker(0, 0, WorldOrderConfig.tileScale);
                                break;

                            case 'A':
                                moveDecisionMaker(WorldOrderConfig.tileScale, 0, 0);
                                break;

                            case 'D':
                                moveDecisionMaker(-WorldOrderConfig.tileScale, 0, 0);
                                break;
                        }
                        return;
                    }
                    else
                    {
                        
                        _inAction = false;
                        _startRotation = _actionLiveForm.GetGameObject().transform.eulerAngles;
                        _startMove = _actionLiveForm.GetGameObject().transform.position;

                        castRadiusEnter();
                        _indexOfAction++;
                        return;
                        

                    }
                }
            }
        }
        return;
    }

    private void moveDecisionMaker(float X, float Y, float Z)
    {
        NormalizeToRotate(ref X, ref Z, _startRotation.y);

        Tile tile = _tileController.GetTile((int)((_startMove.x + X) / WorldOrderConfig.tileScale), (int)((_startMove.z + Z) / WorldOrderConfig.tileScale));

        if (GetTile().GetTileType() == "Objective")
        {
            LevelData levelData = new LevelData();
            levelData.OnTriger("EnemyOnObject");
            castRadiusExit();
            this.OnDead();
        }
        else
        if (tile.GetOccupation() != null)
        {
            if (tile.GetOccupation().GetLiveForm().GetGameObject()==_actionLiveForm.GetGameObject())
            {
                _startRotation = _actionLiveForm.GetGameObject().transform.eulerAngles;
                _startMove = _actionLiveForm.GetGameObject().transform.position;
            }
            _coroutineRunning = true;
            _coroutine.Add(Wait(1.5f));
            return; 
        }
        else
        {

                GetTile().NullOccupation(this);
                tile.SetOccupation(this);
                castRadiusExit();


                if (X == 0)
                {
                    if (Z < 0)
                        MoveBack();
                    else
                        MoveForward();
                }
                else
                if (X < 0)
                    MoveLeft();
                else
                    MoveRight();
                _inAction = true;
            
        }
    }

    private IEnumerator Wait(float i) { yield return new WaitForSeconds(i); _coroutineRunning = false; }
}
