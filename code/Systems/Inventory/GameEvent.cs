using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Systems.Inventory;
public static class GameEvent
{
	public partial class Client
	{
		public const string InventoryChanged = "Client.Inventory.Changed";

		public partial class InventoryChangedAttribute : EventAttribute
		{
			public InventoryChangedAttribute() : base( InventoryChanged ) { }
		}
	}

	public partial class Server
	{
		public const string InventoryChanged = "Server.Inventory.Changed";

		public partial class InventoryChangedAttribute : EventAttribute
		{
			public InventoryChangedAttribute() : base( InventoryChanged ) { }
		}
	}
}
