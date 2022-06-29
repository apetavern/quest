using Quest.Player;
using Quest.Systems.Items;
using Quest.Systems.Inventory;

namespace Quest.UI.Inventory;

[UseTemplate]
public class InventoryDisplay : Panel
{
	public List<Item> Inventory { get; set; }

	public InventoryDisplay()
	{

	}

	[GameEvent.Client.InventoryChanged]
	public void InventoryChanged( IList<Item> updatedInventory )
	{
		DeleteChildren();

		foreach ( var item in updatedInventory )
		{
			Add.Label( item.Name + " - " + item.InventoryStackCount );
		}
	}
}
