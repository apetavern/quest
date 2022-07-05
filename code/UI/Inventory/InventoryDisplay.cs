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
		Inventory = updatedInventory.ToList();
		BuildInventory();
	}

	private void BuildInventory()
	{
		foreach ( var item in Inventory )
		{
			var invItem = new InventoryItem( item );
			AddChild( invItem );
		}
	}
}

public partial class InventoryItem : Panel
{
	public Item Item { get; private set; }
	public Image Icon { get; private set; }
	public Label Count { get; private set; }

	public InventoryItem( Item item )
	{
		Item = item;

		var invIconPath = item.InventoryAssetPath;
		var count = item.InventoryStackCount;

		if ( invIconPath == null || invIconPath.Length == 0 )
		{
			Log.Error( $"{item.ID} has a null or empty icon!" );
		}

		Icon = Add.Image( invIconPath, "item-icon" );
		Count = Add.Label( count.ToString(), "item-count" );

		if ( count == 1 )
		{
			Count.AddClass( "hide" );
		}

		AddEventListener( "onclick", () =>
		{
			Log.Info( "OnClick" );
		} );

		AddEventListener( "onrightclick", () =>
		{
			Log.Info( "Rightclick" );
		} );
	}


}
