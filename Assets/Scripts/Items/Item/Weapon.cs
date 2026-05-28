using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
	[field: SerializeField] public float Damage { get; private set; }

	public override void Randomize(Dictionary<ItemData.RandomizedProperty, ItemData.Randomization> randomizations)
	{
		base.Randomize(randomizations);
		if (randomizations.TryGetValue(ItemData.RandomizedProperty.Damage, out var damageRandomization))
		{
			Damage = damageRandomization.Range.GetRand();
		}
	}
}
