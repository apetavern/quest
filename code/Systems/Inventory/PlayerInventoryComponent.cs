using Quest.Systems.Items;

namespace Quest.Systems.Inventory;

public partial class PlayerInventoryComponent : EntityComponent
{
	/// <summary>
	/// Networked list of items, representing the player's inventory.
	/// </summary>
	[Net] public IList<Item> InventoryItems { get; set; }

	public void AddItem( Item newItem )
	{
		var existingItems = InventoryItems.Where( i => i.InventoryStackCount < i.MaxStackSize && i.ID == newItem.ID );
		if ( existingItems.Any() )
			existingItems.First().InventoryStackCount += 1;
		else
			InventoryItems.Add( newItem );
	}
}
