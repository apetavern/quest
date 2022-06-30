namespace Quest.Systems.Skills;

public partial class GameEvent
{
	public partial class Client
	{
		public const string ExperienceAdded = "Client.Experience.Added";

		public partial class ExperienceAddedAttribute : EventAttribute
		{
			public ExperienceAddedAttribute() : base( ExperienceAdded ) { }
		}

	}

	public partial class Server
	{
		public const string ExperienceAdded = "Server.Experience.Added";

		public partial class ExperienceAddedAttribute : EventAttribute
		{
			public ExperienceAddedAttribute() : base( ExperienceAdded ) { }
		}
	}
}
