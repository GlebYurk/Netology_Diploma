using UnityEngine;
using UnityEngine.EventSystems;

namespace agent
{
    //����� ����� ������ ��� ��������� �������
    //���������� �� MonoBehaviour ������������ ������ 
    public class Agent : MonoBehaviour, IPointerDownHandler
    {
        public event ClickEventHandler OnClickEventHandler;
        public void OnPointerDown(PointerEventData eventData)
        {

            OnClickEventHandler?.Invoke(this);
        }

        public delegate void ClickEventHandler(Agent agent);

    }
}
