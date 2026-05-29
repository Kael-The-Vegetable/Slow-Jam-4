using System.Collections.Generic;
using UnityEngine;

public class Armour : Equipment
{
	[field: Header("Armour Properties")]
	[field: SerializeField] public float Defense { get; protected set; }

	public override void Randomize(Dictionary<ItemData.RandomizedProperty, ItemData.Randomization> randomizations)
	{
		base.Randomize(randomizations);
		if (randomizations.TryGetValue(ItemData.RandomizedProperty.Defense, out var defenseRandomization))
		{
			Defense = defenseRandomization.Range.GetRand();
		}
	}
}
