﻿namespace Quest.Systems.States;

public abstract partial class StateMachine : BaseNetworkable
{
	/// <summary>
	/// Networked list of states.
	/// </summary>
	[Net, SkipHotload] public IList<State> States { get; set; }

	/// <summary>
	/// The currently active state.
	/// </summary>
	[Net, SkipHotload] public State ActiveState { get; set; }

	/// <summary>
	/// The owner of this machine.
	/// </summary>
	[Net] public Entity Owner { get; set; }

	/// <summary>
	/// The index of the current state.
	/// </summary>
	[Net] public int StateIndex { get; set; } = 0;

	public abstract void Start();

	public abstract void Simulate();

	public abstract void Stop();

	/// <summary>
	/// Method to change the active state of the current machine.
	/// </summary>
	/// <param name="new">The new State.</param>
	protected void ChangeState( State @new )
	{
		ActiveState?.Stop();
		ActiveState = @new;
		ActiveState.Owner = Owner;
		ActiveState.Start();
	}
}
