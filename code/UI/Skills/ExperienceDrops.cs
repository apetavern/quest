using Quest.Systems.Skills;

namespace Quest.UI.Skills;

[UseTemplate]
public class ExperienceDrops : Panel
{
	public ExperienceDrops()
	{

	}

	[GameEvent.Client.ExperienceAdded]
	public static void ExperienceAdded( string skillId )
	{
		Log.Info( $"skillId: {skillId} was just updated for {Local.Client.Name}." );
	}
}
