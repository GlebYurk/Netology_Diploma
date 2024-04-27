using System.Collections.Generic;
using UnityEngine;
using WorldOrder.Generic;
using agent;
using WorldMode.Player;
using Unity.VisualScripting;

//Генератор тайловой карты
public class LevelGenerator
{
    private GameObject _levelObject;

    private TileController _tileMap;

    private GameObject _floorTile, _wallTile, _spawnerTile, _objectiveTile, _playerObj;

    private List<IAgent> _agentList;
    private int _agentIndex = 0;
    private PlayerAction _playerAction;

    public LevelGenerator(string levelName, List<IAgent> agentList,GameObject player,TileController tileController)
    {
        _wallTile = Resources.Load<GameObject>("Tile/Wall");
        _floorTile = Resources.Load<GameObject>("Tile/Floor");
        _spawnerTile = Resources.Load<GameObject>("Tile/Spawner");
        _objectiveTile = Resources.Load<GameObject>("Tile/Objective");
        _playerObj = player;
        _agentList = agentList;
        _tileMap = tileController;

        _levelObject= new GameObject(levelName);
        tileController.addList();
        TextAsset LevelData;
        LevelData = Resources.Load<TextAsset>("LevelData/" + levelName);
        GenerateMap(LevelData.text);
    }

    public PlayerAction GetPlayerAction() { return _playerAction; }
    public GameObject GetLevelObject() { return _levelObject; }
    private void GenerateMap(string Data)
    {
        List<GameObject> SpawnObject = new List<GameObject>();

        int x=0, z=0;

        for (int i = 0; i < Data.Length; i++)
        {
            SpawnObject.Clear();
            switch (Data[i])
            {
                case '0':
                    SpawnObject.Add(_floorTile);
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;

                case '1':
                    SpawnObject.Add(_wallTile);
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;

                case '2':
                    SpawnObject.Add(_spawnerTile);
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;

                case '3':
                    SpawnObject.Add(_objectiveTile);
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;

                case '\r':
                    z++;
                    _tileMap.addList();
                    x = 0;
                    i++;
                    break;

                case 'A':
                    SpawnObject.Add(_floorTile);
                    if (_agentIndex < _agentList.Count)
                    {
                        string name = (_agentList[_agentIndex].GetAgentObject().name);
                        _agentList.RemoveAt(_agentIndex);
                        switch (name)
                        {
                            case "Qwerty":
                                _agentList.Insert(_agentIndex, (Resources.Load<Qwerty>("Pref/Agent/Qwerty")));
                                break;
                            case "SaintVictoria":
                                _agentList.Insert(_agentIndex, (Resources.Load<SaintVictoria>("Pref/Agent/SaintVictoria")));
                                break;
                            case "Henkoten":
                                _agentList.Insert(_agentIndex, (Resources.Load<Henkoten>("Pref/Agent/Henkoten")));
                                break;
                        }
                        SpawnObject.Add(_agentList[_agentIndex].GetAgentObject());

                    }
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;

                case 'P':
                    SpawnObject.Add(_floorTile);
                    if (_agentIndex < _agentList.Count)
                    {
                        string name = (_agentList[_agentIndex].GetAgentObject().name);
                        _agentList.RemoveAt(_agentIndex);
                        switch (name)
                        {
                            case "Qwerty":
                                _agentList.Insert(_agentIndex, (Resources.Load<Qwerty>("Pref/Agent/Qwerty")));
                                break;
                            case "SaintVictoria":
                                _agentList.Insert(_agentIndex, (Resources.Load<SaintVictoria>("Pref/Agent/SaintVictoria")));
                                break;
                            case "Henkoten":
                                _agentList.Insert(_agentIndex, (Resources.Load<Henkoten>("Pref/Agent/Henkoten")));
                                break;
                        }
                        SpawnObject.Add(_agentList[_agentIndex].GetAgentObject());
                    }
                    OnPlayerSpawn(SpawnObject, x, z);
                    x++;
                    break;

                default:
                    SpawnObject.Add(_wallTile);
                    OnTileSpawn(SpawnObject, x, z);
                    x++;
                    break;
            }
        }

        _tileMap.CastRadius();
    }

    private bool OnPlayerSpawn(List<GameObject> SpawnObject, int x, int z)
    {
        GameObject objOnTile = null;
        bool flag = true;

        foreach (GameObject obj in SpawnObject)
        {
            if (obj != null)
            {
                obj.transform.position = new Vector3(x * WorldOrderConfig.tileScale, obj.transform.position.y, z * WorldOrderConfig.tileScale);

                GameObject buff = Object.Instantiate(obj);
                buff.transform.parent = _levelObject.transform;
                if (obj.layer == 3)
                {

                    buff.name = x + "_" + z;
                    _tileMap.addInZ(z, obj);
                }
                else
                {

                    _agentList.Remove(obj.GetComponent<IAgent>());
                    objOnTile = buff;
                    objOnTile.transform.position=obj.transform.position;
                    _playerObj.transform.position= objOnTile.transform.position;
                    _playerObj.transform.eulerAngles = objOnTile.transform.eulerAngles;
                    objOnTile.transform.parent = _playerObj.transform;
                    objOnTile.name= obj.name;
                    _agentList.Insert(_agentIndex, objOnTile.GetComponent<IAgent>());
                    _playerAction = new PlayerAction(new GeneralLiveForm(1, 1, 1, 1, true, 0.02f, _playerObj), _tileMap, _agentList[_agentIndex],_playerObj);
                    _agentList[_agentIndex].SetAgentAction(_tileMap);
                    _agentIndex++;
                }

            }
            else { flag = false; break; }
        }

        if (objOnTile != null)
            _tileMap.OnTileOccupationSet(x, z, _agentList[_agentIndex - 1].GetAgentAction());


        return flag;
    }

    private bool OnTileSpawn(List<GameObject> SpawnObject, int x, int z)
    {
        GameObject objOnTile = null;
        bool flag = true;
        foreach (GameObject obj in SpawnObject)
        {
            if (obj != null)
            {
                obj.transform.position = new Vector3(x * WorldOrderConfig.tileScale, obj.transform.position.y, z * WorldOrderConfig.tileScale);
                GameObject buff = Object.Instantiate(obj);


                if (obj.layer == 3)
                {
                    buff.transform.parent = _levelObject.transform;
                    buff.name = x + "_" + z;
                    _tileMap.addInZ(z, obj);
                }
                else
                {
                    _agentList.Remove(obj.GetComponent<IAgent>());
                    objOnTile = buff;
                    objOnTile.name = obj.name;
                    _agentList.Insert(_agentIndex, objOnTile.GetComponent<IAgent>());
                    _agentList[_agentIndex].SetAgentAction(_tileMap);
                    _agentList[_agentIndex].GetAgentObject().transform.position = new Vector3(x * WorldOrderConfig.tileScale, obj.transform.position.y, z * WorldOrderConfig.tileScale);
                    _agentIndex++;
                }

            }
            else { flag = false; break; }
        }

        if (objOnTile != null)
            _tileMap.OnTileOccupationSet(x, z, _agentList[_agentIndex - 1].GetAgentAction());
        
        return flag;
    }

    public TileController GetTileMap() { return _tileMap; }

}


