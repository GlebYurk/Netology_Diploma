using System.Collections.Generic;
using UnityEngine;


public class EnemyData 
{
    private GameObject _pref;
    private List<float> _timer;

    public EnemyData(GameObject pref)
    {
        _timer = new List<float>();
        _pref = pref;
    }

    public GameObject GetEnemy() { return _pref; }

    public void AddEnemy(float timer)
    {
        
        for (int i=0;i<_timer.Count-1;i++) 
        {
            if (timer < _timer[i])
            {
                _timer.Insert(i, timer);
                return;
            }
        }
        _timer.Add(timer);
    }

    public bool ReadyToSpawn(float timer)
    {
        if (_timer.Count !=0)
        {
            if (_timer[0] < timer)
            {
                _timer.RemoveAt(0);
                return true;
            }
            else
                return false;
        }
        return false;
    }

    public bool CheckEnemyType(GameObject enemy) 
    { 
    if (_pref==enemy)
            return true;
        else
            return false;
    }
}
