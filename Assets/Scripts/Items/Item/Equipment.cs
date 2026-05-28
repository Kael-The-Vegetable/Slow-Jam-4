using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item, IEquipable
{
	[field: SerializeField] public EquipSlot Slot { get; private set; }
	public Junk[] JunkImprovements { get; private set; } = new Junk[0];
	[field: SerializeField] public int MaxDurability { get; private set; }
	private int _durability;
	public int CurrentDurability
	{
		get => _durability;
		set
		{
			_durability = Mathf.Clamp(value, 0, MaxDurability);
			if (_durability == 0)
			{
				// Handle weapon breaking logic here
				Debug.Log($"{name} has broken!");
			}
		}
	}

	public virtual void Randomize(Dictionary<ItemData.RandomizedProperty, ItemData.Randomization> randomizations)
	{
		if (randomizations.TryGetValue(ItemData.RandomizedProperty.Durability, out var durabilityRandomization))
		{
			CurrentDurability = (int)durabilityRandomization.Range.GetRand();
		}
	}
}
