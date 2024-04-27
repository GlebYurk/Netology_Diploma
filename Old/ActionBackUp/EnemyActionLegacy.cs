/*
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using WorldMode;
using WorldOrder.Generic;

public class EnemyActionLegacy : BasicActionLegacy
{
    int indexOfAction;
    string input;
    private TileController _tileController;

    public EnemyActionLegacy(GameObject _actionObject, float atackDelay, string input, TileController tileController) : base(_actionObject)
    {
        int indexOfAction = 0;
        this._actionObject = _actionObject;
        this._tileController = tileController;
        inAction = false;
        startRotation = _actionObject.transform.eulerAngles;
        startMove = _actionObject.transform.position;
        this.input = input;

    }

    public void ActionFlow()
    {
        while (input.Length > indexOfAction)
        {
            if (_actionObject != null)
            {
                if (!inAction)
                {
                    switch (input[indexOfAction])
                    {
                        case 'Q':
                            _action = rotateLeft;
                            break;

                        case 'E':
                            _action = rotateRight;
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
                    inAction = true;

                }
                else
                {
                    if (!_action())
                    {
                        inAction = false;
                        startRotation = _actionObject.transform.eulerAngles;
                        startMove = _actionObject.transform.position;
                        indexOfAction++;
                    }

                }
            }
        }
    }
    private void moveDecisionMaker(float X, float Y, float Z)
    {
        float buff;
        switch (startRotation.y)
        {
            case 90:
                buff = X;
                X = Z; Z = -buff;
                break;
            case 180:
                Z *= -1;
                X *= -1;
                break;
            case 270:
                buff = X;
                X = -Z; Z = buff;
                break;
            default: break;
        }

        Tile tile = _tileController.GetTile((int)((startMove.x + X) / WorldOrderConfig.tileScale), (int)((startMove.z + Z) / WorldOrderConfig.tileScale));
        Debug.Log(tile.x + " - " + tile.z);

        if (tile.GetOccupation())
        { _action = exitAction; return; }
        else
        {
            _tileController.GetTile((int)(startMove.x / WorldOrderConfig.tileScale), (int)(startMove.z / WorldOrderConfig.tileScale)).NullOccupation();
            tile.SetOccupation(_actionObject);

            if (X == 0)
            {
                if (Z < 0)
                    _action = MoveBack;
                else
                    _action = MoveForward;
            }
            else
            if (X < 0)
                _action = MoveLeft;
            else
                _action = MoveRight;

        }
    }
}
*/