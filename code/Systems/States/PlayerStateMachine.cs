﻿namespace Quest.Systems.States;

public abstract partial class PlayerStateMachine : StateMachine
{
	public override void Simulate()
	{
		DebugOverlay.ScreenText( $"State: {ActiveState.Name}", 0 );
		ActiveState?.Simulate();
	}
}
