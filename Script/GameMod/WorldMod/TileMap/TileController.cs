using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WorldMode;
using WorldOrder.Generic;

//Контроллер карты
public class TileController
{
    //Карта
    private List<List<Tile>> _tileMap;
    
    public TileController(List<List<Tile>> tileMap) { _tileMap = tileMap; }

    public Tile GetTile(int x, int z) { return _tileMap[z][x]; }

    public void addList()
    => _tileMap.Add(new List<Tile>());

    public void addInZ(int z, GameObject obj)
        => _tileMap[z].Add(new Tile(obj.transform.position.x, obj.transform.position.z, obj.name));

    public void SetTileMap(List<List<Tile>> tileMap) 
        => _tileMap = tileMap;    
   
    public void OnTileOccupationSet(int x,int z,BasicAtackAction basicAction) 
        => _tileMap[z][x].SetOccupation(basicAction);
    
    public void OnTileChange(int x,int z, Tile tile) 
        => _tileMap[z][x] = tile;

    //Поиск всех тайлов по типу 
    public List<Tile> GetTilesByType(string type)
    {
        List<Tile> FindTarget = new List<Tile>();
        foreach (List<Tile> tileList in _tileMap)
        {
            foreach (Tile tile in tileList)
            {
                if (tile.GetTileType() == type)
                    FindTarget.Add(tile);
            }
        }
        return FindTarget;
    }



    // Спавн объектов на тайле
    public GameObject SpawnOnTile(GameObject pref, int x, int z) 
    { 
        pref.transform.position = new Vector3(x * WorldOrderConfig.tileScale, pref.transform.position.y, z * WorldOrderConfig.tileScale);
        GameObject buff = Object.Instantiate(pref);
        return buff;
    }

    public void CastRadius()
    {
        foreach (List<Tile> tileList in _tileMap)
        {
            foreach (Tile tile in tileList)
            {
                if (tile.GetOccupation()!=null)
                {
                    tile.GetOccupation().castRadiusEnter();
                }
            }
        }
    }
}
