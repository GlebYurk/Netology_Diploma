using agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldOrder.Generic;

//Управление игроком
namespace WorldMode
{
    namespace Player
    {
        public class PlayerAction : BasicAction
        {
            // Режим камеры
            private bool _mapState;

            // Событие при переходе в режим карты
            public event EventHandler OnMapEventHandler;
            public delegate void EventHandler();
            GameObject _player;
            private IAgent _agent;

            //Конструктор для одного объекта
            public PlayerAction(GeneralLiveForm _actionLiveForm, TileController _tileController, IAgent _agent, GameObject player) : base(_actionLiveForm, _tileController)
            {
                _mapState = false;
                this._agent = _agent;
                _player = player;
            }

            public IAgent GetAgent() { return _agent; }

            public bool GetMapState() { return _mapState; }

            //Обработка пользовательского ввода
            public void ActionFlow(Queue<char> InputQ)
            {
                if (_actionLiveForm != null)
                {
                    if (!_inAction)
                    {
                        if (InputQ.Count != 0)
                        {
                            setAction(InputQ);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        if (!_coroutineRunning)
                        {
                            _inAction = false;
                            _startRotation = _player.transform.eulerAngles;
                            _startMove = _player.transform.position;
                            if (!_mapState)
                            {
                                _agent.GetAgentAction().SetStartPositin(_startRotation, _startMove);
                                _agent.GetAgentAction().castRadiusEnter();
                            }
                            if (InputQ.Count != 0)
                                InputQ.Dequeue();

                        }
                        return;
                    }
                }
                return;
            }

            //Установка действия на клавишы 
            void setAction(Queue<char> InputQ)
            {
                if (!_mapState)
                {
                    switch (InputQ.Peek())
                    {
                        case 'Q':
                            _agent.GetAgentAction().castRadiusExit();
                            rotateLeft();
                            break;

                        case 'E':
                            _agent.GetAgentAction().castRadiusExit();
                            rotateRight();
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

                        case 'F':
                            _agent.Spell();
                            break;
                        case 'M':
                            MapView();
                            break;

                        case '!':
                            MapView();
                            break;
                    }
                    _inAction = true;
                }
                else
                {
                    if(!_coroutineRunning)
                    switch (InputQ.Peek())
                    {
                        case 'S':
                            CameraMoveDown();
                            _inAction = true;
                            break;

                        case 'W':
                            CameraMoveUp();
                            _inAction = true;
                            break;

                        case 'A':
                            CameraMoveLeft();
                            _inAction = true;
                            break;

                        case 'D':
                            CameraMoveRight();
                            _inAction = true;
                            break;

                        default:
                            InputQ.Dequeue();
                            break;
                    }
                }
            }

            //Переход в режим сверху
            public void MapView()
            {
                if (_actionLiveForm.GetGameObject().transform.childCount > 1)
                {
                    _mapState = true;
                    _actionLiveForm.GetGameObject().transform.GetChild(1).transform.parent = null;
                    OnMapEventHandler?.Invoke();
                }
                _coroutine.Add(OnMapViewEnter());
            }

            private IEnumerator OnMapViewEnter()
            {
                yield return rotateBase(90, 0, _startRotation.y);
                yield return MoveBase(0, 8, 0, _actionLiveForm.GetSpeed());
                yield return null;
            }

            // Переход в режим от первого лица
            public void setAgent(IAgent agent)
            {
                _agent = agent;
                _actionLiveForm.GetGameObject().transform.position = agent.GetAgentObject().transform.position;
                _actionLiveForm.GetGameObject().transform.eulerAngles = agent.GetAgentObject().transform.eulerAngles;
                _startRotation = _actionLiveForm.GetGameObject().transform.eulerAngles;
                _startMove = _actionLiveForm.GetGameObject().transform.position;
                _endMove = _startMove;
                _mapState = false;
                agent.GetAgentObject().transform.parent = _actionLiveForm.GetGameObject().transform;
            }

            // Передвижение для камеры
            private void CameraMoveLeft()
            {
                _startMove = _player.transform.position;
                _coroutine.Add(MoveBase(WorldOrderConfig.tileScale, 0, 0, _actionLiveForm.GetSpeed()));
            }
            private void CameraMoveRight()
            {
                _startMove = _player.transform.position;
                _coroutine.Add(MoveBase(-WorldOrderConfig.tileScale, 0, 0, _actionLiveForm.GetSpeed()));
            }
            private void CameraMoveDown()
            {
                _startMove = _player.transform.position;
                _coroutine.Add(MoveBase(0, 0, -WorldOrderConfig.tileScale, _actionLiveForm.GetSpeed()));
            }
            private void CameraMoveUp()
            {
                _startMove = _player.transform.position;
                _coroutine.Add(MoveBase(0, 0, WorldOrderConfig.tileScale, _actionLiveForm.GetSpeed()));
            }

            // Определение логики взаимодействия с тайлом по тегу 
            private void moveDecisionMaker(float X, float Y, float Z)
            {
                NormalizeToRotate(ref X, ref Z, _startRotation.y);

                Tile tile = _tileController.GetTile((int)((_startMove.x + X) / WorldOrderConfig.tileScale), (int)((_startMove.z + Z) / WorldOrderConfig.tileScale));

                switch (tile.GetTileType())
                {
                    case "Wall":
                        _coroutineRunning = false; return;

                    case "Floor":
                        if (tile.GetOccupation() != null)
                        { _coroutineRunning = false; return; }
                        else
                        {

                            GetTile().NullOccupation(_agent.GetAgentAction());
                            _agent.GetAgentAction().castRadiusExit();
                            tile.SetOccupation(_agent.GetAgentAction());
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

                            break;
                        }

                    default:
                        _coroutineRunning = false; return;
                }
            }

        }
    }
}
    