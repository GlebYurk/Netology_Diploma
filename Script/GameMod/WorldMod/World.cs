using agent;
using System.Collections.Generic;
using UnityEngine;
using WorldOrder.GameMode;
using WorldMode.Player;
using UnityEngine.UI;
using TMPro;

namespace WorldMode
{
    public class World : IMode
    {
        private InputController _inputController;


        private List<IAgent> _agentList;
        private List<AgentBar> _agentBars;

        private List<EnemyAction> _enemyActions;

        private PlayerAction _playerAction;


        private TileController _tileController;
        private GameObject levelObject;
 

        private float _timer;


        private List<PatchHolder> _patchHolders;

        //Ïðåôàáû ìîíñòðîâ (Íóæíî ñäåëàòü èíòåðôåéñ äëÿ âðàãîâ) 
        private GameObject _spiderPref, _spiderMagPref, _spiderSpeedPref, _slimePref, _slimeMagPref;

        private GameObject _player;

        private GameObject _menu;
        private TMP_Text _levelObject;
        private TMP_Text _damageCalculation;

        //Çàìåíèòü èç-çà ñîäåðæàíèå ñòàòè÷åñêîé ïåðåìåííîé
        private LevelData _levelData;

        private event EventHandler NewLevelHandler;
        private delegate void EventHandler();

        private MonoBehaviour _monoBehaviour;

        private bool _gameOver;

        //Êîíñòðóêòîð ïðåäïîëîãàþùèé, ÷òî èãðà íà÷íåòüñÿ â ðåæèìå àãåíòà
        public World(GameObject player, MonoBehaviour monoBehaviour)
        {
            Canvas canvas = Object.Instantiate(Resources.Load<Canvas>("Pref/UI"));
            _menu = canvas.transform.GetChild(0).gameObject;
            _menu.SetActive(false);
            _levelObject = canvas.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
            _damageCalculation= canvas.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
            _menu.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(onRestart);
            _menu.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(onExit);

            _gameOver = false;
            _timer = 0;

            _agentList = new List<IAgent>();
            _agentBars = new List<AgentBar>()
                ;
            _inputController = new InputController();
            _inputController.PauseEventHandler += OnPause;

            _enemyActions = new List<EnemyAction>();
            _patchHolders = new List<PatchHolder>();

            _levelData = new LevelData();

            _spiderPref = Resources.Load<GameObject>("Pref/Enemy/Spider");
            _spiderMagPref = Resources.Load<GameObject>("Pref/Enemy/SpiderMag");
            _spiderSpeedPref = Resources.Load<GameObject>("Pref/Enemy/SpiderSpeed");
            _slimePref = Resources.Load<GameObject>("Pref/Enemy/Slime");
            _slimeMagPref = Resources.Load<GameObject>("Pref/Enemy/SlimeMag");

            _monoBehaviour = monoBehaviour;
            _player = player;
            On1LevelStart();


        }

        // Ìåòîä â Update
        // Íóæíî îòäåëèòü ÷àñòü â fixUpdate
        public void GameExecution()
        {
            
            if (!_gameOver)
            {
                if (!_levelData.CheckSubDefeat())
                    _inputController.InputWorldMode();

                if (!_menu.activeSelf)
                {
                    if (!_levelData.CheckLevelState())
                    {
                        if (!_playerAction.GetAgent().GetAgentAction().CheckLiveStatus() && _playerAction.GetMapState() == false)
                        {
                            IAgent agent = _playerAction.GetAgent();
                            _playerAction.MapView();
                            _agentList.Remove(agent);
                            agent.GetAgentAction().StopCourutine(_monoBehaviour);
                            Object.Destroy(agent.GetAgentObject());
                            CheckGameOver();

                        }
                        else
                        {
                            _playerAction.ActionFlow(_inputController.GetInput());
                            _playerAction.StartCourutine(_monoBehaviour);
                            GeneralLiveForm targetForm;
                            if (_playerAction.GetAgent().GetAgentAction().GetTargetList().Count > 0)
                            {
                                GeneralLiveForm PlayerForm = _playerAction.GetAgent().GetAgentAction().GetLiveForm();
                                targetForm = _playerAction.GetAgent().GetAgentAction().GetTargetList()[0].GetLiveForm();
                                _damageCalculation.text = "Çäîðîâüå ïðîòèâíèêà= " + targetForm.GetHP() + "\nÎæèäàåìûé óðîí ïî ïðîòèâíèêó " + targetForm.GetExpectedDamage(PlayerForm.GetAtackType(), PlayerForm.GetAtack(), 0);
                            }
                            else { _damageCalculation.text = ""; }
                        }


                        foreach (PatchHolder patchHolder in _patchHolders)
                        {
                            _enemyActions.AddRange(patchHolder.SpawnCheker(_timer, _tileController));
                        }

                        for (int i = 0; i < _agentList.Count; i++)
                        {
                            if (!_agentList[i].GetAgentAction().CheckLiveStatus())
                            {
                                _agentList[i].GetAgentAction().StopCourutine(_monoBehaviour);
                                Object.Destroy(_agentList[i].GetAgentObject());
                                _agentList.RemoveAt(i);
                                Object.Destroy(_agentBars[i].GetBar());
                                _agentBars.RemoveAt(i);
                                CheckGameOver();
                                i--;
                            }
                            else
                            {
                                _agentList[i].GetAgentAction().StartCourutine(_monoBehaviour);
                                _agentList[i].SpellPassive(Time.deltaTime);
                                _agentBars[i].AgentUpdate();
                            }
                        }

                        for (int i = 0; i < _enemyActions.Count; i++)
                        {
                            if (!_enemyActions[i].CheckLiveStatus())
                            {
                                _enemyActions[i].StopCourutine(_monoBehaviour);
                                Object.Destroy(_enemyActions[i].GetLiveForm().GetGameObject());
                                _enemyActions.RemoveAt(i);
                                CheckSubObjectGameOver();
                                OnLevelDataChange();
                                i--;
                            }
                            else
                            {
                                _enemyActions[i].ActionFlow();
                                _enemyActions[i].StartCourutine(_monoBehaviour);
                            }
                        }
                    }
                    else
                    {
                        onLevelEnd(_monoBehaviour);
                        NewLevelHandler();
                    }

                    _timer += Time.deltaTime;
                }
                else
                {
                    if (_agentList.Count == 0)
                    {
                        _menu.SetActive(true);

                    }

                }
            }
        }

        // Ñòàðò ïåðâîãî óðîâíÿ
        private void On1LevelStart()
        {
            _levelData.OnLevelSet("Defend", 30, 5);//30

            _agentList.Add((Resources.Load<Qwerty>("Pref/Agent/Qwerty")));

            NewLevelHandler -= On1LevelStart;
            NewLevelHandler += On2LevelStart;

            OnLevelStart("Level1");


            List<Tile> Spawner = new List<Tile>();
            Spawner.AddRange(_tileController.GetTilesByType("Spawner"));

            _patchHolders.Add(new PatchHolder("WWEWWQWEWWWWW", Spawner[0]));
            _patchHolders.Add(new PatchHolder("WWWQWWEWWQWWWWWWWWQWQWWWWWWWWEWEWWWWW", Spawner[1]));
            _patchHolders.Add(new PatchHolder("WWWQWWWQWWEWWWWWWWEWWWWEWWWWWEWWEWWWW", Spawner[2]));
            _patchHolders.Add(new PatchHolder("EWQWEWWEWQWWWWW", Spawner[3]));
            _patchHolders.Add(new PatchHolder("QQWWQWWEWQWWWWW", Spawner[4]));
            _patchHolders.Add(new PatchHolder("QQWWWEWWQWWEWWWWWWWWEWWEWWWWW", Spawner[5]));
            _patchHolders.Add(new PatchHolder("QQWWWEWWWQWEWWWWWEWWEWEWW", Spawner[6]));

            _patchHolders[0].addEnemyType(_spiderPref);
            _patchHolders[3].addEnemyType(_spiderPref);
            _patchHolders[4].addEnemyType(_spiderPref);
            _patchHolders[1].addEnemyType(_spiderSpeedPref);
            _patchHolders[2].addEnemyType(_spiderSpeedPref);
            _patchHolders[5].addEnemyType(_spiderSpeedPref);
            _patchHolders[6].addEnemyType(_spiderSpeedPref);
            _patchHolders[1].addEnemyType(_slimePref);

            _patchHolders[1].addEnemy(_spiderSpeedPref, 3);

            _patchHolders[1].addEnemy(_spiderSpeedPref, 8);
            _patchHolders[2].addEnemy(_spiderSpeedPref, 9);


            _patchHolders[0].addEnemy(_spiderPref, 20);


            _patchHolders[1].addEnemy(_spiderSpeedPref, 36);
            _patchHolders[2].addEnemy(_spiderSpeedPref, 36);
            _patchHolders[5].addEnemy(_spiderSpeedPref, 36);


            _patchHolders[3].addEnemy(_spiderPref, 53);
            _patchHolders[3].addEnemy(_spiderPref, 54);
            _patchHolders[1].addEnemy(_spiderSpeedPref, 78);
            _patchHolders[2].addEnemy(_spiderSpeedPref, 78);
            _patchHolders[5].addEnemy(_spiderSpeedPref, 78);
            _patchHolders[6].addEnemy(_spiderSpeedPref, 78);

            _patchHolders[3].addEnemy(_spiderPref, 100);


            _patchHolders[1].addEnemy(_spiderSpeedPref, 102);
            _patchHolders[1].addEnemy(_spiderSpeedPref, 103);

            _patchHolders[3].addEnemy(_spiderPref, 110);
            _patchHolders[1].addEnemy(_spiderSpeedPref, 120);
            _patchHolders[2].addEnemy(_spiderSpeedPref, 120);


            _patchHolders[1].addEnemy(_spiderSpeedPref, 130);
            _patchHolders[2].addEnemy(_spiderSpeedPref, 130);
            _patchHolders[5].addEnemy(_spiderSpeedPref, 130);
            _patchHolders[6].addEnemy(_spiderSpeedPref, 130);

            _patchHolders[6].addEnemy(_spiderSpeedPref, 135);
            _patchHolders[6].addEnemy(_spiderSpeedPref, 135);

            _patchHolders[1].addEnemy(_spiderSpeedPref, 150);
            _patchHolders[3].addEnemy(_spiderPref, 153);
            _patchHolders[4].addEnemy(_spiderPref, 155);

            _patchHolders[0].addEnemy(_spiderPref, 160);

            _patchHolders[1].addEnemy(_slimePref, 170);


            _playerAction.OnMapEventHandler += OnMapViewEnter;
        }

        private void On2LevelStart()
        {
            _levelData.OnLevelSet("Kill", 15, 1);//15

            _levelObject.SetText("Survive");
            _agentList.Add((Resources.Load<SaintVictoria>("Pref/Agent/SaintVictoria")));

            NewLevelHandler -= On2LevelStart;
            NewLevelHandler += On3LevelStart;

            OnLevelStart("Level2");

            List<Tile> Spawner = new List<Tile>();
            Spawner.AddRange(_tileController.GetTilesByType("Spawner"));

            _patchHolders.Add(new PatchHolder("WWWWWWWWWWWWWWW", Spawner[0]));
            _patchHolders.Add(new PatchHolder("EWWWWWWWWWWWWWWW", Spawner[1]));
            _patchHolders.Add(new PatchHolder("QWWWWWWWWWWWWWWW", Spawner[2]));
            _patchHolders.Add(new PatchHolder("QQWWWWWWWWWWWWWW", Spawner[3]));

            _patchHolders[0].addEnemyType(_spiderPref);
            _patchHolders[1].addEnemyType(_spiderMagPref);
            _patchHolders[2].addEnemyType(_spiderPref);
            _patchHolders[3].addEnemyType(_spiderMagPref);

            for (int i = 0; i < 120;)
            {
                _patchHolders[0].addEnemy(_spiderPref, i += 8);
                _patchHolders[1].addEnemy(_spiderMagPref, i += 8);
                _patchHolders[2].addEnemy(_spiderPref, i += 8);
                _patchHolders[3].addEnemy(_spiderMagPref, i += 8);

            }

            _playerAction.OnMapEventHandler += OnMapViewEnter;
        }

        private void On3LevelStart()
        {
            _levelData.OnLevelSet("Kill", 14, 1);

            _levelObject.SetText("Destroy Slime");
            _agentList.Add((Resources.Load<Henkoten>("Pref/Agent/Henkoten")));

            NewLevelHandler -= On3LevelStart;
            NewLevelHandler += GameOver;

            OnLevelStart("Level3");

            List<Tile> Spawner = new List<Tile>();
            Spawner.AddRange(_tileController.GetTilesByType("Spawner"));

            _patchHolders.Add(new PatchHolder("W", Spawner[0]));
            _patchHolders.Add(new PatchHolder("W", Spawner[1]));
            _patchHolders.Add(new PatchHolder("W", Spawner[2]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[3]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[4]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[5]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[6]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[7]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[8]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[9]));
            _patchHolders.Add(new PatchHolder("EW", Spawner[10]));
            _patchHolders.Add(new PatchHolder("QQW", Spawner[11]));
            _patchHolders.Add(new PatchHolder("QQW", Spawner[12]));
            _patchHolders.Add(new PatchHolder("QQW", Spawner[13]));

            _patchHolders[0].addEnemyType(_slimePref);
            _patchHolders[1].addEnemyType(_slimeMagPref);
            _patchHolders[2].addEnemyType(_slimePref);
            _patchHolders[3].addEnemyType(_slimeMagPref);
            _patchHolders[4].addEnemyType(_slimePref);
            _patchHolders[5].addEnemyType(_slimeMagPref);
            _patchHolders[6].addEnemyType(_slimePref);
            _patchHolders[7].addEnemyType(_slimeMagPref);
            _patchHolders[8].addEnemyType(_slimePref);
            _patchHolders[9].addEnemyType(_slimeMagPref);
            _patchHolders[10].addEnemyType(_slimePref);
            _patchHolders[11].addEnemyType(_slimeMagPref);
            _patchHolders[12].addEnemyType(_slimePref);
            _patchHolders[13].addEnemyType(_slimeMagPref);
            _playerAction.OnMapEventHandler += OnMapViewEnter;

            _patchHolders[0].addEnemy(_slimePref, 1);
            _patchHolders[1].addEnemy(_slimeMagPref, 1);
            _patchHolders[2].addEnemy(_slimePref, 1);
            _patchHolders[3].addEnemy(_slimeMagPref, 1);
            _patchHolders[4].addEnemy(_slimePref, 1);
            _patchHolders[5].addEnemy(_slimeMagPref, 1);
            _patchHolders[6].addEnemy(_slimePref, 1);
            _patchHolders[7].addEnemy(_slimeMagPref, 1);
            _patchHolders[8].addEnemy(_slimePref, 1);
            _patchHolders[9].addEnemy(_slimeMagPref, 1);
            _patchHolders[10].addEnemy(_slimePref, 1);
            _patchHolders[11].addEnemy(_slimeMagPref, 1);
            _patchHolders[12].addEnemy(_slimePref, 1);
            _patchHolders[13].addEnemy(_slimeMagPref, 1);
        }

        private void GameOver()
        {
            _levelData.OnLevelSet("Kill", 1, 1);

            _levelObject.SetText("Find it in yourself");

            OnLevelStart("Limbo");


            _gameOver = true;
            _menu.transform.GetChild(4).gameObject.SetActive(true);
            OnPause();
        }

        // Ïðè âõîäå â ðåæèì êàðòû
        public void OnMapViewEnter()
        {
            foreach (Agent agent in _agentList)
            {
                agent.OnClickEventHandler += OnAgentClick;
            }
        }

        // Ïðè âûõîäå èç êàðòû
        private void OnMapViewExit()
        {
            foreach (Agent agent in _agentList)
            {
                agent.OnClickEventHandler -= OnAgentClick;
            }
        }

        // Ñîáûòèå ïðè íàæàòèè íà àãåíòà â ðåæèìå êàðòû
        private void OnAgentClick(Agent agent)
        {
            IAgent setAgent = null;

            foreach (IAgent agentItem in _agentList)
                if (agent.gameObject == agentItem.GetAgentObject())
                    setAgent = agentItem;

            _inputController.ClearInput();
            _playerAction.setAgent(setAgent);
            OnMapViewExit();
        }
        private void OnAgentBarClick(IAgent agent)
        {

            _playerAction.AgentDetach(_monoBehaviour);
            _inputController.ClearInput();
            _playerAction.setAgent(agent);
            OnMapViewExit();

        }

        private void OnEntityDataChange(GeneralLiveForm ChangeTrigger)
        {

            for (int i=0;i<_agentList.Count;i++)
            {
                if (_agentList[i].GetAgentAction().GetLiveForm() == ChangeTrigger)
                {
                    _agentBars[i].AgentUpdate();
                    return;
                }
            }
        }
        private void OnLevelDataChange()
        {
            _levelObject.text = _levelData.ToString();
        }

        private void onLevelEnd(MonoBehaviour monoBehaviour)
        {
            Object.Destroy(levelObject);

            monoBehaviour.StopAllCoroutines();



            while (_enemyActions.Count != 0)
            {
                Object.Destroy(_enemyActions[0].GetLiveForm().GetGameObject());
                _enemyActions.RemoveAt(0);
            }

            try
            {
                for (int i = 0; i < _agentList.Count; i++)
                {
                    Object.Destroy(_agentList[i].GetAgentObject());
                }
            }
            catch
            {
                Debug.Log("On destroy agent Error");
            }


            for (int i = 0; i < _agentBars.Count; i++)
            {
                Object.Destroy(_agentBars[i].GetBar());
            }
            
            
            _agentBars.Clear();
            _patchHolders.Clear();
            _tileController = null;
            _timer = 0;
        }

        private void OnLevelStart(string LevelName)
        {
            OnLevelDataChange();
            List<List<Tile>> tileMap = new List<List<Tile>>();
            _tileController = new TileController(tileMap);
            LevelGenerator levelGenerator = new LevelGenerator(LevelName, _agentList, _player, _tileController);
            levelObject = levelGenerator.GetLevelObject();
            _playerAction = levelGenerator.GetPlayerAction();

            foreach (IAgent agent in _agentList)
            {
                _agentBars.Add(new AgentBar(_agentBars.Count, agent, _menu));
                _agentBars[_agentBars.Count-1].OnClickEventHandler += OnAgentBarClick;
                agent.GetAgentAction().OnAtackEventHandler += OnEntityDataChange;
            }
        }

        private void OnPause()
        {
            _menu.SetActive(!_menu.activeSelf);
            Time.timeScale = _menu.activeSelf ? 0f : 1f;
        }


        private void onRestart()
        {
            if (NewLevelHandler != null)
                NewLevelHandler -= NewLevelHandler;

            _gameOver = false;
            onLevelEnd(_monoBehaviour);
            _agentList.Clear();
            _playerAction = null;

            _menu.SetActive(false);
            _menu.transform.GetChild(4).gameObject.SetActive(false);

            _patchHolders.Clear();
            Time.timeScale = 1;

            On1LevelStart();

        }

        private void onExit()
        {
            Application.Quit();
        }

        private void CheckGameOver()
        {
            if (_agentList.Count == 0)
                GameOver();
        }

        private void CheckSubObjectGameOver()
        {
            if (_levelData.CheckSubDefeat())
                GameOver();
        }
    }
}
