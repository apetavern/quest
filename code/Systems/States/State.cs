namespace Quest.Systems.States;

public abstract partial class State : BaseNetworkable
{
	/// <summary>
	/// The machine-readable identifier of the state.
	/// </summary>
	public virtual string ID => "";

	/// <summary>
	/// The human-readable name of the state.
	/// </summary>
	public virtual string Name => "";

	/// <summary>
	/// The owner of this state.
	/// </summary>
	[Net] public Entity Owner { get; set; }

	[Net] public bool Done { get; set; } = false;

	public abstract void Start();

	public abstract void Simulate();

	public abstract void Stop();
}
