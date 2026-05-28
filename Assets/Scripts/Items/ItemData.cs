using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
	public enum RandomizedProperty
	{
		Damage,
		Defense,
		Durability
	}
	[field: SerializeField] public Item Item { get; private set; }

	[Serializable]
	public struct Randomization
	{
		[field: SerializeField] public RandomizedProperty Property { get; set; }
		[field: SerializeField] public FloatRange Range { get; set; }
	}

	public Randomization[] Randomizations;
	
	public Item Generate()
	{
		if (Item.TryGetComponent(out Equipment e))
		{
			var dict = new Dictionary<RandomizedProperty, Randomization>();
			for (int i = 0; i < Randomizations.Length; i++)
			{
				dict[Randomizations[i].Property] = Randomizations[i];
			}
			e.Randomize(dict);
		}
		return Item;
	}
}
