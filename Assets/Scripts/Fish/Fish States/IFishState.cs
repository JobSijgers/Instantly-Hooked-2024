
public interface IFishState
{
    public void UpdateState();
    public void OnStateActivate();
    public void ResetState();
    public IFishState SwitchState();
}
