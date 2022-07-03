namespace Quest.Systems.Items.Mining;

public partial class PickaxeCarriable : Carriable
{
	public override HoldType HoldType => HoldType.Pickaxe;
	public override HoldHandedness HoldHandedness => HoldHandedness.TwoHands;

	public override string ModelPath => "models/tools/pickaxe/pickaxe.vmdl";

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );
	}
}
