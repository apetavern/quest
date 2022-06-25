namespace Quest.Systems.Interactions;

public partial class TestInteraction : Interaction
{
	public override string ID => "test_interaction";
	public override string Name => "Test";

	public TestInteraction( Entity entity )
	{
		Owner = entity;
	}

	public override bool CanResolve => true;

	public override bool ResolveOnServer => false;

	protected override void OnClientResolve()
	{
		ChatBox.Say( "Test Interaction" );
	}
}
