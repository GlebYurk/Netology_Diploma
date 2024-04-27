using agent;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentBar 
{
    Button _bar;

    TMP_Text _stat;

    IAgent _agent;

    public event ClickEventHandler OnClickEventHandler;
    public delegate void ClickEventHandler(IAgent agent);

    public AgentBar(int index, IAgent agent, GameObject canvas)
    {
        _agent = agent;

        _bar = Object.Instantiate(Resources.Load<Button>("Pref/AgentUI"));
        _bar.transform.SetParent(canvas.transform.parent);
        _bar.GetComponent<RectTransform>().anchoredPosition = new Vector3(4, 69 - 51 * index, 0);
        _bar.onClick.AddListener(OnClikcEvent);
        string Name=null;
        switch (agent.GetAgentObject().name)
        {
            case "Qwerty":
                Name = "Qwerty             Телепортация";
                break;
            case "SaintVictoria":
                Name = "Saint Victoria     Регенерация";
                break;
            case "Henkoten":
                Name = "Henkoten      Смена типа урона";
                break;

        }

        _bar.transform.GetChild(1).GetComponent<TMP_Text>().text=Name;

        _stat = _bar.transform.GetChild(0).GetComponent<TMP_Text>();
        _stat.text=_agent.ToString();
    }

    private void OnClikcEvent() 
    {
        OnClickEventHandler.Invoke(_agent);
    }

    public void AgentUpdate()
    {
        _stat.text = _agent.ToString();
    }

    public GameObject GetBar() { return _bar.gameObject; }
}
