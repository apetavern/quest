using Quest.Systems.Interactions;

namespace Quest.Systems.Items.Mining;

public partial class Pickaxe : Item
{
	public override string ID => "item_pickaxe";
	public override string Name => "Pickaxe";
	public override string InventoryAssetPath => "/ui/items/pickaxe.png";

	public override IEnumerable<Interaction> GetAdditionalInteractions()
	{
		return Enumerable.Empty<Interaction>();
	}

	public override string GetExamineText()
	{
		return "I can use this to mine rocks.";
	}

	public Carriable GetCarriable()
	{
		return new PickaxeCarriable();
	}
}
