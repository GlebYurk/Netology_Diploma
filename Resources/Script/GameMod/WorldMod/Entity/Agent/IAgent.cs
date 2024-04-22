using UnityEngine;

namespace agent
{
    public interface IAgent
    {
        void Spell();

        void SpellPassive(float timer);
        GameObject GetAgentObject();
        void SetAgentAction(TileController tileController);
        BasicAtackAction GetAgentAction();
    }
}
