namespace Raele.SuperPlatformer;

public partial class LandingRecoveryState : BaseGroundedState
{
    public override void OnEnter(BaseMotionState.TransitionInfo transition)
    {
        base.OnEnter(transition);
		if (this.Character.JumpSettings.LandingRecoveryDurationMs <= 0)
        {
			this.Character.TransitionMotionState<OnFootState>();
		}
    }

    public override void OnProcessState(float delta)
    {
        base.OnProcessState(delta);
        if (this.DurationActiveMs > this.Character.JumpSettings.LandingRecoveryDurationMs) {
            this.Character.TransitionMotionState<OnFootState>();
        } else if (
            this.Character.JumpSettings.DashingCancelsLandingRecovery
            && this.Character.InputController.DashInputBuffer.ConsumeInput()
        ) {
            this.Character.TransitionMotionState<GroundDashingState>();
        }
    }

    public override void OnPhysicsProcessState(float delta)
    {
        base.OnPhysicsProcessState(delta);
        (float velocityX, float accelerationX) = this.Character.JumpSettings.LandingCancelsMomentum
            ? (0, float.PositiveInfinity)
            : this.CalculateHorizontalOnFootPhysics(delta);
        (float velocityY, float accelerationY) = this.CalculateVerticalOnFootPhysics();
        this.Character.Accelerate(velocityX, velocityY, accelerationX, accelerationY);
        this.Character.MoveAndSlide();
    }
}
