namespace SkateGuy.Staties
{
    public abstract class BasicState
    {
        protected BasicState nextState = null;

        protected StateController stateController = null;

        public BasicState(StateController  _stateController)
        {
            stateController = _stateController;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void Track();

        public virtual void SetToNextState()
        {
            if (nextState == null)
            {
                return;
            }
            stateController.SetState(nextState);
        }
    }
}
