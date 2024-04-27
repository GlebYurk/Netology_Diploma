/*
using System;
using System.Collections;
using UnityEngine;
using WorldOrder.Generic;


    //Добавить интерфейс для действий и привести входные данные к единому стандарту
    public class BasicActionLegacy
    {
        //Заменить на корутин
        protected delegate bool _actionFunction();
        protected _actionFunction _action;

        protected IEnumerator _coroutine;

        //Происходит ли действие?
        protected bool inAction;

        protected Vector3 startRotation, startMove;

        protected GameObject _actionObject;

        //Конструктор для одного объекта
        public BasicActionLegacy(GameObject actionObject)
        {
            this._actionObject = actionObject;
            inAction = false;
            startRotation = _actionObject.transform.eulerAngles;
            startMove = _actionObject.transform.position;
        }


        // Логика вращения объекта
        protected bool rotateBase(float rotateX, float rotateY, float rotateZ)
        {
            Vector3 finalPoint = (startRotation + new Vector3(rotateX, rotateY, rotateZ));
            finalPoint.x %= 360;
            finalPoint.y %= 360;
            finalPoint.z %= 360;

            if (_actionObject.transform.eulerAngles.x == finalPoint.x && _actionObject.transform.eulerAngles.y == finalPoint.y && _actionObject.transform.eulerAngles.z == finalPoint.z)
            {
                return false;
            }
            else
            {
                Quaternion quaternionTo = Quaternion.Euler(startRotation + new Vector3(rotateX, rotateY, rotateZ));
                if (Math.Abs(Quaternion.Angle(quaternionTo, _actionObject.transform.rotation)) < 0.1f)
                {
                    _actionObject.transform.eulerAngles = finalPoint;
                    return false;
                }
                else
                {
                    _actionObject.transform.rotation = Quaternion.Slerp(_actionObject.transform.rotation, quaternionTo, WorldOrderConfig.speedRotate * Time.deltaTime);
                    return true;
                }
            }

        }


        // Логика перемещения объекта
        protected bool MoveBase(float X, float Y, float Z)
        {
            Vector3 vector3 = new Vector3(X, Y, Z);
            vector3 += startMove;
            if (_actionObject.transform.position == vector3)
            {
                _actionObject.transform.position = vector3;
                return false;
            }
            else
            {
                _actionObject.transform.position = Vector3.MoveTowards(_actionObject.transform.position, vector3, 0.02f);
                return true;
            }
        }



        // Поворот
        protected bool rotateLeft()
        {
            return rotateBase(0, -90, 0);
        }
        protected bool rotateRight()
        {
            return rotateBase(0, 90, 0);
        }

        // Передвижение 
        protected bool MoveBack()
        {
            return MoveBase(0, 0, -WorldOrderConfig.tileScale);
        }
        protected bool MoveForward()
        {
            return MoveBase(0, 0, WorldOrderConfig.tileScale);
        }
        protected bool MoveLeft()
        {
            return MoveBase(-WorldOrderConfig.tileScale, 0, 0);
        }
        protected bool MoveRight()
        {
            return MoveBase(WorldOrderConfig.tileScale, 0, 0);

        }

        public bool exitAction()
        {
            return false;
        }


    }

*/