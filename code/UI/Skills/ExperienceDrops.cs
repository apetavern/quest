using SkillEvents = Quest.Systems.Skills.GameEvent;
using Quest.Systems.Skills;

namespace Quest.UI.Skills;

[UseTemplate]
public partial class ExperienceDrops : Panel
{
	public ExperienceDrops()
	{

	}

	[SkillEvents.Client.ExperienceAdded]
	public void ExperienceAdded( SkillType skillType )
	{
		Log.Info( $"skillId: {skillType} was just updated for {Local.Client.Name}." );
		AddChild( new Drop( 10, skillType ) );
	}

	public partial class Drop : Panel
	{
		public TimeUntil TimeToLive { get; set; } = 2f;
		public Image Icon { get; set; }
		public Label DropText { get; set; }

		public Drop( int experience, SkillType skillType )
		{
			Icon = Add.Image( SkillHelpers.SkillIcons[skillType], "skill-icon" );
			DropText = Add.Label( $"+{experience}", "experience-label" );
		}

		public override void Tick()
		{
			float fadePercentage = TimeToLive / 2;
			Style.Opacity = fadePercentage;

			if ( TimeToLive <= 0 )
			{
				Delete();
			}
		}
	}
}
