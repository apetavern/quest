using Quest.Player;
using Quest.Systems.Skills;
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
	public void BridgeExperienceAddedEvent( Client client, SkillType skillType )
	{
		SendClientExperienceAddedEvent( To.Single( client ), skillType );
	}

	[ClientRpc]
	public void SendClientExperienceAddedEvent( SkillType skillType )
	{
		Event.Run( SkillEvents.Client.ExperienceAdded, skillType );
	}
}
