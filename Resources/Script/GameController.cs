using UnityEngine;
using WorldOrder.GameMode;
using WorldMode;

public class GameController : MonoBehaviour
{

    private IMode mode;

    World world;
    private void Awake()
    {
        world= new World(this.gameObject,this);
        mode = world;
    }

    void Update()
    {
        //����� ����� �������� ����� ��� ������� � FixUpdate
        mode.GameExecution();
    }   
}
