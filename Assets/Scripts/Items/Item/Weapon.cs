using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
	[field: Header("Weapon Properties")]
	[field: SerializeField] public float Damage { get; protected set; }

	public enum Handedness { OneHanded, TwoHanded }
	[field: SerializeField] public Handedness WeaponHandedness { get; protected set; }

	public enum WeaponType { Melee, Ranged }
	[field: SerializeField] public WeaponType Type { get; protected set; }

	public override void Randomize(Dictionary<ItemData.RandomizedProperty, ItemData.Randomization> randomizations)
	{
		base.Randomize(randomizations);
		if (randomizations.TryGetValue(ItemData.RandomizedProperty.Damage, out var damageRandomization))
		{
			Damage = damageRandomization.Range.GetRand();
		}
	}
}
