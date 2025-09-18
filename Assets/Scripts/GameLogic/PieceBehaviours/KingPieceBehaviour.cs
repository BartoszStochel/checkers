using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class KingPieceBehaviour : IPieceBehaviour
	{
		public List<Vector2Int> MovementVectors { get; private set; }
		public List<Vector2Int> AttackVectors { get; private set; }
		public bool HasInfiniteMovementRange { get; private set; }

		public KingPieceBehaviour()
		{
			MovementVectors = new List<Vector2Int>
			{
				new Vector2Int(-1, 1),
				new Vector2Int(1, 1),
				new Vector2Int(1, -1),
				new Vector2Int(-1, -1)
			};

			AttackVectors = new List<Vector2Int>
			{
				new Vector2Int(-1, 1),
				new Vector2Int(1, 1),
				new Vector2Int(1, -1),
				new Vector2Int(-1, -1)
			};

			HasInfiniteMovementRange = true;
		}
		public void SetMovementDirection(bool shouldMoveFromTopToBottom)
		{
			
		}
	}
}