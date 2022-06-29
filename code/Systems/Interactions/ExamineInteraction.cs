namespace Quest.Systems.Interactions;

public partial class ExamineInteraction : Interaction
{
	public override string ID => "examine_interaction";
	public override string Name => "Examine";

	public ExamineInteraction( BaseNetworkable entity )
	{
		Owner = entity;
	}

	protected override void OnClientResolve()
	{
		ChatBox.Say( (Owner as IInteractable).GetExamineText() );
	}
}
