using UnityEngine;

public class Junk : Item, IJunk, IStackable
{
	[field: SerializeField] public Improvement[] Improvements { get; private set; }
	[field: SerializeField] public int MaxSize { get; private set; }
	public int CurrentSize { get; private set; } = 0;

	public int AddToStack(int amount)
	{
		
		if (CurrentSize + amount > MaxSize)
		{
			amount = (CurrentSize + amount) - MaxSize;
			CurrentSize = MaxSize;
			return amount;
		}
		CurrentSize += amount;
		return 0;
	}
	public int RemoveFromStack(int amount)
	{
		if (CurrentSize - amount < 0)
		{
			amount -= CurrentSize;
			CurrentSize = 0;
			return amount;
		}
		CurrentSize -= amount;
		return 0;
	}
}
