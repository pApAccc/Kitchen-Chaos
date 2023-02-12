namespace Common.SavingSystem
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}