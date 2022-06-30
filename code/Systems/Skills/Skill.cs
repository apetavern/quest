namespace Quest.Systems.Skills;

public abstract partial class Skill : BaseNetworkable
{
	/// <summary>
	/// The machine-readable identifier of the skill.
	/// </summary>
	public virtual string ID => "";

	/// <summary>
	/// The human-readable name of the skill.
	/// </summary>
	public virtual string Name => "";

	/// <summary>
	/// The level 
	/// </summary>
	[Net] public int Level { get; set; } = 0;

	[Net] public int Experience { get; set; } = 0;
}
