namespace Quest.Systems.Skills;

public partial class PlayerSkillComponent : EntityComponent
{
	[Net]
	public IList<Skill> Skills { get; set; }

	public void PopulateSkills()
	{
		Skills.Add( new MiningSkill() );
	}

	public void AddExperience( SkillType skillType, int experience )
	{
		Skill skill = Skills.Where( skill => skill.ID == skillType.ToString() ).First();
		skill.Experience += experience;

		Event.Run( GameEvent.Server.ExperienceAdded, Entity.Client, skillType );

		Log.Info( $"skill xp: {skill.Experience}" );
	}
}
