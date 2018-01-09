using UnityEngine;

public class StandingState : HeroineBaseState
{
    public StandingState(Heroine heroine)
    {
        this._heroine = heroine;
    }
    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this._heroine.SetHeroineState(new JumpingState(this._heroine));
        }
    }
}