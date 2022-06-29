using Quest.Systems.Interactions;

namespace Quest.Systems.Items.Mining;

public partial class Ore : Item
{
	public override string ID => "item_ore";
	public override string Name => "Ore";

	public override IEnumerable<Interaction> GetAdditionalInteractions()
	{
		return Enumerable.Empty<Interaction>();
	}

	public override string GetExamineText()
	{
		return "I should refine this.";
	}
}
