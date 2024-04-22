using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

//Управление дорогой для монстров
public class PatchHolder
{
    public string path;
    public Tile spawner;

    private List<EnemyData> enemies;

    public PatchHolder(string path, Tile spawner)
    {
        this.path = path;
        this.spawner = spawner;
        enemies = new List<EnemyData>();
    }

    public void addEnemyType(GameObject enemy) 
    {
        enemies.Add(new EnemyData(enemy));
    }
    public void addEnemy(GameObject enemy, float timer) 
    {
        foreach (EnemyData enemyData in enemies)
        {
            if (enemyData.CheckEnemyType(enemy))
            {
                enemyData.AddEnemy(timer);
                return;
            }
        }
    }

    public List<EnemyAction> SpawnCheker(float timer,TileController tileController)
    {
        List<EnemyAction> list = new List<EnemyAction>(); 
        for(int i=0;i<enemies.Count;)
        {
            if (enemies[i].ReadyToSpawn(timer))
            {
                //Debug.Log(enemies[i] + " to spawn at "+ timer);
                List<Point> radius;
                switch (enemies[i].GetEnemy().name)
                {
                    case "Spider":
                        GeneralLiveForm Spider = new GeneralLiveForm(5, 3, 3, 6, false, 0.003f, (tileController.SpawnOnTile(enemies[i].GetEnemy(), (int)spawner.x, (int)spawner.z)));
                        radius = new List<Point>() { new Point(0, 1) };
                        list.Add(new EnemyAction(Spider, tileController, 5, path, radius));
                        tileController.OnTileOccupationSet((int)spawner.x, (int)spawner.z, list[list.Count - 1]);
                        list[list.Count - 1].castRadiusEnter();
                        break;

                    case "SpiderMag":
                        GeneralLiveForm SpiderMag = new GeneralLiveForm(4, 2, 4, 6, true, 0.003f, (tileController.SpawnOnTile(enemies[i].GetEnemy(), (int)spawner.x, (int)spawner.z)));
                        radius = new List<Point>() { new Point(0, 1) };
                        list.Add(new EnemyAction(SpiderMag, tileController, 5, path, radius));
                        tileController.OnTileOccupationSet((int)spawner.x, (int)spawner.z, list[list.Count - 1]);
                        list[list.Count - 1].castRadiusEnter();
                        break;

                    case "SpiderSpeed":
                        GeneralLiveForm SpiderSpeed = new GeneralLiveForm(4, 1, 1, 4, false, 0.005f, (tileController.SpawnOnTile(enemies[i].GetEnemy(), (int)spawner.x, (int)spawner.z)));
                        radius = new List<Point>() { new Point(0, 1) };
                        list.Add(new EnemyAction(SpiderSpeed, tileController, 5, path, radius));
                        tileController.OnTileOccupationSet((int)spawner.x, (int)spawner.z, list[list.Count - 1]);
                        list[list.Count - 1].castRadiusEnter();
                        break;

                    case "Slime":
                        GeneralLiveForm Slime = new GeneralLiveForm(10, 8, 3, 5, false, 0.001f, (tileController.SpawnOnTile(enemies[i].GetEnemy(), (int)spawner.x, (int)spawner.z)));
                        radius = new List<Point>() { new Point(0, 1) };
                        list.Add(new EnemyAction(Slime, tileController, 5, path, radius));
                        tileController.OnTileOccupationSet((int)spawner.x, (int)spawner.z, list[list.Count - 1]);
                        list[list.Count - 1].castRadiusEnter();
                        break;

                    case "SlimeMag":
                        GeneralLiveForm SlimeMag = new GeneralLiveForm(10, 3, 8, 5, true, 0.001f, (tileController.SpawnOnTile(enemies[i].GetEnemy(), (int)spawner.x, (int)spawner.z)));
                        radius = new List<Point>() { new Point(0, 1) };
                        list.Add(new EnemyAction(SlimeMag, tileController, 5, path, radius));
                        tileController.OnTileOccupationSet((int)spawner.x, (int)spawner.z, list[list.Count - 1]);
                        list[list.Count - 1].castRadiusEnter();
                        break;
                       
                }
                i++;
            }
            else
                i++;
        }
        return list;
    }
}
