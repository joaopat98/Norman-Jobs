public abstract class Boss : Enemy
{
    protected bool acting;
    protected bool isAwake;
    public virtual void FinishAction()
    {
        acting = false;
    }

    public virtual void WakeUp()
    {
        isAwake = true;
    }
}