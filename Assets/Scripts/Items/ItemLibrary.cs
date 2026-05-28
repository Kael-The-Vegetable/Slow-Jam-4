using UnityEngine;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ItemLibrary", menuName = "Scriptable Objects/ItemLibrary")]
public class ItemLibrary : ScriptableObject
{
    [field: SerializeField] public ItemData[] Items { get; private set; }

    public ItemData GetItem(string name) 
        => Array.Find(Items, item => item.Item.Name == name);
	public ItemData GetRandomItem() 
        => RandItem(Items);
	public ItemData GetRandomItemByName(string name) 
        => RandItem(Array.FindAll(Items, item => item.Item.Name == name));

	public ItemData GetRandomItemByType<T>() where T : class
        => RandItem(Array.FindAll(Items, item => item.Item is T));
	private ItemData RandItem(ItemData[] list)
    {
        if (list.Length == 0) return null;
        int index = Random.Range(0, list.Length);
        return list[index];
	}

}
