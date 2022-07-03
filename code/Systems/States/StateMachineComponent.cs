namespace Quest.Systems.States;

public partial class StateMachineComponent : EntityComponent
{
	/// <summary>
	/// The active StateMachine
	/// </summary>
	[Net]
	public StateMachine StateMachine { get; set; }

	public void Init()
	{
		StateMachine = new IdleStateMachine();
		StateMachine.Start();
	}

	public void ChangeStateMachine( StateMachine @new )
	{
		StateMachine?.Stop();
		StateMachine = @new;
		StateMachine.Owner = Entity;
		StateMachine?.Start();
	}
}
