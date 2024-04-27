/*
using System.Collections.Generic;
using UnityEngine;
using ActionType;
using JetBrains.Annotations;
using agent;

public class Player : MonoBehaviour
{
    private byte _playerMode;

    Vector3 _mapView1 = new Vector3(0.23f, 13f, 2f);

    List<Agent> agentList;
    //Класс системы передвижения
    private ActionTP _playerType;

    //Ввод
    Queue<char> _inputQueue;

    void Awake()
    {
        agentList= new List<Agent>();
        
        _playerMode = 0;
        _inputQueue = new(4);
        _playerType = new(this.gameObject);
        _playerType.setMapView(_mapView1);

    }

    void Update()
    {
        if (_playerMode == 0)
        {
            InputS();
            _playerType.ActionFlow(_inputQueue);

        }
    }





    //Обработка ввода 
    void InputS()
    {

        if (_inputQueue.TryPeek(out char cType))
        {

            if ((Input.GetKeyDown(KeyCode.W)))
            {
                if (cType.Equals('S'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('W');

            }
            if ((Input.GetKeyDown(KeyCode.S)))
            {
                if (cType.Equals('W'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('S');

            }
            if ((Input.GetKeyDown(KeyCode.Q)))
            {
                if (cType.Equals('E'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('Q');

            }

            if ((Input.GetKeyDown(KeyCode.E)))
            {

                if (cType.Equals('Q'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('E');
            }

            if ((Input.GetKeyDown(KeyCode.A)))
            {

                if (cType.Equals('A'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('D');
            }

            if ((Input.GetKeyDown(KeyCode.D)))
            {

                if (cType.Equals('D'))
                    _inputQueue.Enqueue('C');
                else
                    _inputQueue.Enqueue('A');
            }

            if ((Input.GetKeyDown(KeyCode.M)))
            {
                _inputQueue.Enqueue('M');
            }

        }
    }

    public ActionTP GetAction()
    {
        return _playerType;
    }

    public void SetPlayerMode(byte mode)
    {
        _playerMode = mode;
    }
}
*/