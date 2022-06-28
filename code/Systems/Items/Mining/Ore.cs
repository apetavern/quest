namespace Quest.Systems.Items.Mining;

public partial class Ore : Item
{
	public override string ID => "item_ore";
	public override string Name => "Ore";
	public override bool Stackable => true;
	public override int MaxStackSize => 4;
}
