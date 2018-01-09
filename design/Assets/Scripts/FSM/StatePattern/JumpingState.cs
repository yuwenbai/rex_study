using UnityEngine;

public class JumpingState : HeroineBaseState
{
    public JumpingState(Heroine heroine)
    {
        this._heroine = heroine;
    }
    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this._heroine.SetHeroineState(new StandingState(this._heroine));
        }
    }
}