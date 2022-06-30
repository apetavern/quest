using Quest.Player;
using InvEvents = Quest.Systems.Inventory.GameEvent;
using SkillEvents = Quest.Systems.Skills.GameEvent;

namespace Quest;

public partial class Game
{
	[InvEvents.Server.InventoryChanged]
	public static void BridgeInventoryChangedEvent( Client client )
	{
		SendClientInventoryChangedEvent( To.Single( client ) );
	}

	/*
	 * ClientRpc necessary here due to bug in Components. See issue below:
	 * https://github.com/Facepunch/sbox-issues/issues/1417
	 */
	[ClientRpc]
	public static void SendClientInventoryChangedEvent()
	{
		Event.Run( InvEvents.Client.InventoryChanged, (Local.Pawn as QuestPlayer).Inventory.InventoryItems );
	}

	[SkillEvents.Server.ExperienceAdded]
	public static void BridgeExperienceAddedEvent( Client client, string skillId )
	{
		SendClientExperienceAddedEvent( To.Single( client ), skillId );
	}

	[ClientRpc]
	public static void SendClientExperienceAddedEvent( string skillId )
	{
		Event.Run( SkillEvents.Client.ExperienceAdded, skillId );
	}
}
