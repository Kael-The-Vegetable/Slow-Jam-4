using UnityEngine;

public class Room : MonoBehaviour
{
	[field:SerializeField] public Vector2Int Size { get; private set; }
	[SerializeField] private float _personalScalar = 1f;
	[field:SerializeField] public Vector2Int[] Entrances { get; private set; } = new Vector2Int[0];

	public enum RoomType { Starting, Normal, Boss, Treasure }
	[field:SerializeField] public RoomType Type { get; private set; } = RoomType.Normal;

	public void Initialize(Vector2 gridSize)
	{
		transform.localScale = gridSize / _personalScalar;
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0, 0, 0.5f);
		Gizmos.DrawCube(transform.position, (Vector2)Size * _personalScalar);
		Gizmos.color = Color.blue;
		for (int i = 0; i < Entrances.Length; i++)
		{
			Vector2 loc = Entrances[i] - (Size - Vector2.one) / 2f;
			Gizmos.DrawSphere((Vector2)transform.position + loc * _personalScalar, 0.2f);
		}
	}

}
