public interface ITool
{
    public bool IsActive { get; }

    public void Activate();
    public void Deactivate();
}