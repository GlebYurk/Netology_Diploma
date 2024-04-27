/*using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace ActionType
{
    public class ActionTP
    {
        //Размер тайла
        public const float tileScale = 1;


        //Обшие модификаторы скорости

        private const float _speedRotate = 15;
        private const float _speedMove = 4f;


        //Объект, который совершает действия

        private GameObject _actionObject;


        //Ссылка на метод описывающий действие

        private delegate bool _actionFunction();
        private _actionFunction _action;


        //Находится ли объект в действии 

        bool inAction;


        // Начальная позиция движния

        Vector3 startRotation, startMove;

        // Маяк для режима камеры 
        private Vector3 _mapView;
        



        //Конструктор для одного объекта
        public ActionTP(GameObject _actionObject)
        {
            this._actionObject = _actionObject;
            inAction = false;
            startRotation = _actionObject.transform.eulerAngles;
            startMove = _actionObject.transform.position;
        }


        //Обработка пользовательского ввода
        public void ActionFlow(Queue<char> InputQ)
        {
            if (!inAction)
            {
                if (InputQ.Count != 0)
                {
                    setAction(InputQ.Peek());
                    inAction = true;

                }
            }
            else
            {
                if (!(inAction = _action()))
                {
                    startRotation = _actionObject.transform.eulerAngles;
                    startMove = _actionObject.transform.position;
                    InputQ.Dequeue();

                }

            }
        }


        //Установка действия
        void setAction(char InputQ)
        {
            switch (InputQ)
            {
                case 'Q':
                    _action = rotateLeft;
                    break;

                case 'E':
                    _action = rotateRight;
                    break;

                case 'S':
                    _action = MoveBack;
                    break;

                case 'W':
                    _action = MoveForward;
                    break;

                case 'A':
                    _action = MoveLeft;
                    break;

                case 'D':
                    _action = MoveRight;
                    break;
                case 'M':
                    _action = MapView;
                    break;
            }
        }

        public void setMapView(Vector3 mapView)
        { 
        _mapView = mapView;
        }



        // Поворот
        public bool rotateLeft()
        {
            return rotateBase(0, -90, 0);
        }

        bool rotateRight()
        {
            return rotateBase(0, 90, 0);
        }


        // Передвижение 
        bool MoveBack()
        {
            //  moveDecisionMaker(TileCheker(x, z),x, y, z, distance);
            return moveDecisionMaker(TileCheker(0, -tileScale), 0, 0, -tileScale, tileScale);
        }

        bool MoveForward()
        {
            return moveDecisionMaker(TileCheker(0, tileScale), 0, 0, tileScale, tileScale);
        }

        bool MoveLeft()
        {
            return moveDecisionMaker(TileCheker(tileScale, 0), tileScale, 0, 0, tileScale);
        }

        bool MoveRight()
        {
            return moveDecisionMaker(TileCheker(-tileScale, 0), -tileScale, 0, 0, tileScale);
        }

        bool MapView()
        {
            if (_actionObject.transform.childCount > 1)
                if(_actionObject.transform.GetChild(1) != null)
            _actionObject.transform.GetChild(1).transform.parent = null;
            if (!MoveBase(_mapView.x, _mapView.y, _mapView.z, 15))
                return rotateBase(90, 0, 0);
            else
                return true;


        }

        // Логика вращения объекта
        bool rotateBase(float rotateX, float rotateY, float rotateZ)
        {
            Quaternion quaternionTo = Quaternion.Euler(startRotation + new Vector3(rotateX, rotateY, rotateZ));

            if (Math.Abs(Quaternion.Angle(quaternionTo, _actionObject.transform.rotation)) < 0.09f)
            {
                Vector3 finalPoint = (startRotation + new Vector3(rotateX, rotateY, rotateZ));

                finalPoint.x %= 360;
                finalPoint.y %= 360;
                finalPoint.z %= 360;
                _actionObject.transform.eulerAngles = finalPoint;

                return false;
            }
            else
            {
                _actionObject.transform.rotation = Quaternion.Slerp(_actionObject.transform.rotation, quaternionTo, _speedRotate * Time.deltaTime);

                return true;
            }
        }


        // Логика перемещения объекта
        bool MoveBase(float X, float Y, float Z, float distance)
        {

            if (Vector3.Distance(_actionObject.transform.position, startMove) > distance)
            {
                _actionObject.transform.position = new Vector3(Mathf.Round(_actionObject.transform.position.x), Mathf.Round(_actionObject.transform.position.y), Mathf.Round(_actionObject.transform.position.z));
                return false;
            }
            else
            {
                _actionObject.transform.Translate(X * Time.deltaTime * _speedMove, Y * Time.deltaTime * _speedMove, Z * Time.deltaTime * _speedMove);
                return true;
            }
        }


        // Классификация тайла
        public string TileCheker(float x, float z)
        {
            GameObject cheking;

            // Поправка на поворот 
            switch (startRotation.y)
            {
                case 0:
                    cheking = GameObject.Find((startMove.x + x) + "_" + (startMove.z + z));
                    break;

                case 90:
                    cheking = GameObject.Find((startMove.x + z) + "_" + (startMove.z - x));
                    break;

                case 180:
                    cheking = GameObject.Find((startMove.x - x) + "_" + (startMove.z - z));
                    break;

                case 270:
                    cheking = GameObject.Find((startMove.x - z) + "_" + (startMove.z + x));
                    break;

                default:
                    cheking = GameObject.Find(x + "_" + z);
                    break;
            }

            Debug.Log(cheking.name + " ---- " + startRotation.y);


            // Возврат тега 
            if (cheking.CompareTag("Wall"))
            {
                return "Wall";
            }
            else if (cheking.CompareTag("Floor"))
            {
                return "Floor";
            }
            else if (cheking.CompareTag("Occupied"))
            {
                return "Occupied";
            }
            else if (cheking.CompareTag("Event"))
            {
                return "Event";
            }
            return "defult";
        }

        bool eventHandler()
        {

            return false;
        }


        // Определение логики взаимодействия с тайлом по тегу 
        bool moveDecisionMaker(string tagObgect, float X, float Y, float Z, float distance)
        {
            switch (tagObgect)
            {
                case "Wall":
                    break;

                case "Floor":
                    return MoveBase(X, Y, Z, distance);
            }

            return false;
        }
    }
}*/