/*
using System.Collections.Generic;
using UnityEngine;
using WorldOrder.Generic;

namespace WorldMode
{
    namespace Player
    {
        public class PlayerActionLegacy : BasicActionLegacy
        {

            // Режим камеры
            private bool mapState;

            // Событие при переходе в режим карты
            public event EventHandler OnMapEventHandler;
            public delegate void EventHandler();


            //Ссылка на контроль тайлов (Возможно стоит заменить синглтоном?)
            private TileController _tileController;

            //Конструктор для одного объекта
            public PlayerActionLegacy(GameObject _actionObject, TileController tileController) : base(_actionObject)
            {
                mapState = false;
                _tileController = tileController;
            }

            //Обработка пользовательского ввода
            public void ActionFlow(Queue<char> InputQ)
            {
                if (_actionObject != null)
                {
                    if (!inAction)
                    {
                        if (InputQ.Count != 0)
                        {
                            setAction(InputQ);
                        }
                    }
                    else
                    {
                        if (!_action())
                        {
                            inAction = false;
                            startRotation = _actionObject.transform.eulerAngles;
                            startMove = _actionObject.transform.position;
                            if (InputQ.Count != 0)
                                InputQ.Dequeue();
                        }

                    }
                }
            }


            //Установка действия на клавишы 
            void setAction(Queue<char> InputQ)
            {
                if (!mapState)
                {
                    switch (InputQ.Peek())
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
                        case 'M':
                            _action = MapView;
                            break;
                        case '!':
                            _action = MapView;
                            break;
                    }

                    inAction = true;
                }
                else
                {
                    switch (InputQ.Peek())
                    {
                        case 'S':
                            _action = CameraMoveDown;
                            inAction = true;
                            break;
                        case 'W':
                            _action = CameraMoveUp;
                            inAction = true;
                            break;
                        case 'A':
                            _action = CameraMoveLeft;
                            inAction = true;
                            break;
                        case 'D':
                            _action = CameraMoveRight;
                            inAction = true;
                            break;
                        default:
                            InputQ.Dequeue();
                            break;
                    }


                }
            }
            //Переход в режим сверху
            private bool MapView()
            {
                if (_actionObject.transform.childCount > 1)
                {
                    mapState = true;
                    _actionObject.transform.GetChild(1).transform.parent = null;
                    OnMapEventHandler?.Invoke();
                }
                return ((rotateBase(90, 0, startRotation.y)) || MoveBase(0, 5, 0));
            }
            // Переход в режим от первого лица
            public void setAgent(GameObject agent)
            {
                _actionObject.transform.position = agent.transform.position;
                _actionObject.transform.eulerAngles = agent.transform.eulerAngles;
                startRotation = _actionObject.transform.eulerAngles;
                startMove = _actionObject.transform.position;
                mapState = false;
                agent.transform.parent = _actionObject.transform;
            }

            // Передвижение для камеры
            private bool CameraMoveLeft()
            {
                return MoveBase(WorldOrderConfig.tileScale, 0, 0);
            }
            private bool CameraMoveRight()
            {
                return MoveBase(-WorldOrderConfig.tileScale, 0, 0);
            }
            private bool CameraMoveDown()
            {
                return MoveBase(0, 0, -WorldOrderConfig.tileScale);
            }
            private bool CameraMoveUp()
            {
                return MoveBase(0, 0, WorldOrderConfig.tileScale);
            }

            // Определение логики взаимодействия с тайлом по тегу 
            private void moveDecisionMaker( float X, float Y, float Z)
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

                Tile tile = _tileController.GetTile((int) ((startMove.x+X) / WorldOrderConfig.tileScale), (int)((startMove.z+Z) / WorldOrderConfig.tileScale));
                Debug.Log(tile.x + " - " + tile.z);

                switch (tile.GetTileType())
                {
                    case "Wall":
                        _action = exitAction; return;
                    case "Floor":
                        if (tile.GetOccupation())
                        { _action = exitAction; return;}
                        else
                        {
                            _tileController.GetTile((int)(startMove.x / WorldOrderConfig.tileScale), (int)(startMove.z / WorldOrderConfig.tileScale)).NullOccupation();
                            tile.SetOccupation(_actionObject);

                            if(X==0)
                            {
                                if (Z<0)
                                    _action = MoveBack;
                                else
                                    _action = MoveForward;
                            }
                            else
                            if (X<0)
                                _action = MoveLeft;
                            else
                                _action = MoveRight;
                            break;
                        }
                        default:
                        _action = exitAction; return;
                }
            }
        }     
    }
}
*/