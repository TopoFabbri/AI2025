using System;

namespace FSM
{
    public abstract class State
    {
        public Action<Enum> OnFlag;
        public abstract BehaviourActions GetOnEnterBehaviours(params object[] parameters);
        public abstract BehaviourActions GetOnTickBehaviours(params object[] parameters);
        public abstract BehaviourActions GetOnExitBehaviour(params object[] parameters);
    }
}