using UnityEngine;

public abstract class Item : MonoBehaviour
{
	[field: Header("Item Properties")]
	[field: SerializeField] public string Name { get; protected set; }
	[field: SerializeField] public Sprite Icon { get; protected set; }
	[field: SerializeField] public float Weight { get; protected set; }
}
