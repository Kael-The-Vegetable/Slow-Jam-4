using UnityEngine;

public class Item : MonoBehaviour
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public Sprite Icon { get; private set; }
	[field: SerializeField] public float Weight { get; private set; }
}
