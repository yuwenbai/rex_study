public class Heroine
{
    public Heroine()
    {
        _state = new StandingState(this);
    }
    public HeroineBaseState _state;
    public void SetHeroineState(HeroineBaseState state)
    {
        _state = state;
    }
    public void HandleInput()
    {
        _state.HandleInput();
    }
}