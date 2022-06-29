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
			Label label = Add.Label( item.Name + " - " + item.InventoryStackCount );
			label.AddEventListener( "onclick", () =>
			{
				item.GetInteractions().First().ClientResolve();
			} );
		}
	}
}
