using UnityEngine;

public class Room : MonoBehaviour
{
	[field:SerializeField] public RectInt Area { get; private set; }
	[SerializeField] private float _personalScalar = 1f;
	[field:SerializeField] public Entrance[] Entrances { get; private set; } = new Entrance[0];

	public enum RoomType { Starting, Normal, Boss, Treasure }
	[field:SerializeField] public RoomType Type { get; private set; } = RoomType.Normal;

	public void Initialize(Vector2 gridSize, Vector2Int position)
	{
		transform.localScale = gridSize / _personalScalar;
		var a = Area;
		a.position = position;
		Area = a;
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0, 0, 0.5f);
		Gizmos.DrawCube(transform.position, (Vector2)Area.size * _personalScalar * transform.localScale);
		Gizmos.color = Color.blue;
		for (int i = 0; i < Entrances.Length; i++)
		{
			Vector2 loc = Entrances[i] - (Area.size - Vector2.one) / 2f;
			Gizmos.DrawSphere((Vector2)transform.position + loc * _personalScalar * transform.localScale, 0.2f);
		}
	}

}
