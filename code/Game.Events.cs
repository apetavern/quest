using Quest.Player;
using Quest.Systems.Inventory;

namespace Quest;

public partial class Game
{
	[GameEvent.Server.InventoryChanged]
	public static void BridgeEvent( Client client )
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
		Event.Run( GameEvent.Client.InventoryChanged, (Local.Pawn as QuestPlayer).Inventory.InventoryItems );
	}
}
