using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldOrder.Generic;

namespace WorldMode
{
    //Добавить интерфейс для действий и привести входные данные к единому стандарту
    //Возможно стоит переформировать класс в конструктор и отделить GeneralLiveForm
    public class BasicAction
    {
        protected List<IEnumerator> _coroutine;
        protected List<IEnumerator> _runningCoroutine;
        protected bool _coroutineRunning;

        protected bool _inAction;

        protected Vector3 _startRotation, _startMove;
        protected Vector3 _endMove, _endRotate;
        protected GeneralLiveForm _actionLiveForm;

        protected TileController _tileController;
        
        protected bool _live;
        //Конструктор для одного объекта
        public BasicAction(GeneralLiveForm actionObject, TileController tileController)
        {

            _coroutine = new List<IEnumerator>();
            _runningCoroutine = new List<IEnumerator>();
            this._actionLiveForm = actionObject;
            _inAction = false;
            _coroutineRunning = false;
            _live = true;
            _startRotation = _actionLiveForm.GetGameObject().transform.eulerAngles;
            _startMove = _actionLiveForm.GetGameObject().transform.position;
            _endMove = _startMove;
            _endRotate = _startRotation;
            _tileController = tileController;
        }

        public GeneralLiveForm GetLiveForm() { return _actionLiveForm; }

        public bool IsLive() { return _live; } 

        public void StartCourutine(MonoBehaviour monoBehaviour)
        {
            foreach (IEnumerator action in _coroutine)
            {
                _runningCoroutine.Add(action);
                monoBehaviour.StartCoroutine(action);
            }
            _coroutine.Clear();
        }

        public void StopCourutine(MonoBehaviour monoBehaviour)
        {
            foreach (IEnumerator action in _runningCoroutine)
            {
                monoBehaviour.StopCoroutine(action);
            }
        _runningCoroutine.Clear();
        }

        // Логика вращения объекта
        protected IEnumerator rotateBase(float rotateX, float rotateY, float rotateZ)
        {
            _coroutineRunning = true;
            _endRotate = (_startRotation + new Vector3(rotateX, rotateY, rotateZ));
            _endRotate.x %= 360;
            _endRotate.y %= 360;
            _endRotate.z %= 360;
            Quaternion quaternionTo = Quaternion.Euler(_startRotation + new Vector3(rotateX, rotateY, rotateZ));
            while (Math.Abs(Quaternion.Angle(quaternionTo, _actionLiveForm.GetGameObject().transform.rotation)) > WorldOrderConfig.speedRotate)
            {
                if (_live)
                    _actionLiveForm.GetGameObject().transform.rotation = Quaternion.Slerp(_actionLiveForm.GetGameObject().transform.rotation, quaternionTo, WorldOrderConfig.speedRotate * Time.deltaTime);
                    yield return null;
            }
            _actionLiveForm.GetGameObject().transform.eulerAngles = _endRotate;
            _coroutineRunning =false;
            yield return null;
        }


        // Логика перемещения объекта
        protected IEnumerator MoveBase(float X, float Y, float Z, float step)
        {
            _coroutineRunning = true;
            _endMove= _startMove + new Vector3(X, Y, Z);

            while (_actionLiveForm.GetGameObject().transform.position != _endMove)
            {
                if (_live)
                    _actionLiveForm.GetGameObject().transform.position = Vector3.MoveTowards(_actionLiveForm.GetGameObject().transform.position, _endMove, step);
                yield return null;
            }
            _actionLiveForm.GetGameObject().transform.position = _endMove;
            _coroutineRunning = false;
            yield return null;
        }

        // Поворот
        protected void rotateLeft()
        {
            _coroutine.Add(rotateBase(0, -90, 0));
        }
        protected void rotateRight()
        {
            _coroutine.Add(rotateBase(0, 90, 0));
        }

        // Передвижение 
        protected void MoveBack()
        {
            _coroutine.Add( MoveBase(0, 0, -WorldOrderConfig.tileScale,_actionLiveForm.GetSpeed()));
        }
        protected void MoveForward()
        {
            _coroutine.Add( MoveBase(0, 0, WorldOrderConfig.tileScale, _actionLiveForm.GetSpeed()));
        }
        protected void MoveLeft()
        {
            _coroutine.Add(MoveBase(-WorldOrderConfig.tileScale, 0, 0, _actionLiveForm.GetSpeed()));
        }
        protected void MoveRight()
        {
            _coroutine.Add(MoveBase(WorldOrderConfig.tileScale, 0, 0, _actionLiveForm.GetSpeed()));

        }

        public void NormalizeToRotate(ref float x, ref float z,float rotationY) 
        {
            float buff;
            switch (rotationY)
            {
                case 90:
                    buff = x;
                    x = z; z = -buff;
                    break;

                case 180:
                    z *= -1;
                    x *= -1;
                    break;

                case 270:
                    buff = x;
                    x = -z; z = buff;
                    break;
                default: break;
            }
        }

        public Tile GetTile() { return _tileController.GetTile((int)(_endMove.x / WorldOrderConfig.tileScale), (int)(_endMove.z / WorldOrderConfig.tileScale)); }
        public Tile GetTileStart() { return _tileController.GetTile((int)(_startMove.x / WorldOrderConfig.tileScale), (int)(_startMove.z / WorldOrderConfig.tileScale)); }

    }

}