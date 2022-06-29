using Quest.Systems.Interactions;

namespace Quest.Systems.Items;

public abstract partial class Item : BaseNetworkable, IInteractable
{
	/// <summary>
	/// The machine-readable identifier of the item.
	/// </summary>
	public virtual string ID => "";

	/// <summary>
	/// The human-readable name of the item.
	/// </summary>
	public virtual string Name => "";

	/// <summary>
	/// A path to the asset representing this item in the inventory.
	/// </summary>
	public virtual string InventoryAssetPath => "";

	/// <summary>
	/// A path to the asset representing this item in the world.
	/// </summary>
	public virtual string ModelAssetPath => "";

	/// <summary>
	/// Whether the item can be stacked in the inventory.
	/// </summary>
	public virtual bool Stackable => false;

	/// <summary>
	/// The maximum stack size for the item in the inventory.
	/// </summary>
	public virtual int MaxStackSize => 1;

	/// <summary>
	/// The number of items we have in this item stack.
	/// </summary>
	[Net] public int InventoryStackCount { get; set; } = 1;

	public abstract string GetExamineText();

	public string GetInteracteeName()
	{
		return Name;
	}

	public IEnumerable<Interaction> GetInteractions()
	{
		IEnumerable<Interaction> interactions = new List<Interaction>()
		{
			new ExamineInteraction( this )
		};

		return interactions.Concat( GetAdditionalInteractions() );
	}

	public abstract IEnumerable<Interaction> GetAdditionalInteractions();
}
