using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    private Queue<char> _inputQueue;

    public Queue<char> GetInput() => _inputQueue;
    public void ClearInput()=>_inputQueue.Clear();

    public event EventHandler PauseEventHandler;
    public delegate void EventHandler();


    public InputController()
    {
        _inputQueue = new(4);
    }

    public InputController(int queueSize)
    {
        _inputQueue = new(queueSize);
    }

    public void InputWorldMode()
    {

        _inputQueue.TryPeek(out char cType);
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
       
        if ((Input.GetKeyDown(KeyCode.F)))
        {
            _inputQueue.Enqueue('F');
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseEventHandler.Invoke();
        }
    }
}
