namespace Quest.Systems.Interactions;

public interface IInteractable
{
	IEnumerable<Interaction> GetInteractions();

	string GetInteracteeName();

	string GetExamineText();
}
