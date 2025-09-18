using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class ManPieceBehaviour : IPieceBehaviour
	{
		public List<Vector2Int> MovementVectors { get; private set; }
		public List<Vector2Int> AttackVectors { get; private set; }
		public bool HasInfiniteMovementRange { get; private set; }

		public ManPieceBehaviour()
		{
			MovementVectors = new List<Vector2Int>
			{
				new Vector2Int(-1, 1),
				new Vector2Int(1, 1)
			};

			AttackVectors = new List<Vector2Int>
			{
				new Vector2Int(-1, 1),
				new Vector2Int(1, 1),
				new Vector2Int(1, -1),
				new Vector2Int(-1, -1)
			};

			HasInfiniteMovementRange = false;
		}

		public void SetMovementDirection(bool shouldMoveFromTopToBottom)
		{
			if (shouldMoveFromTopToBottom)
			{
				for (int i = 0; i < MovementVectors.Count; i++)
				{
					MovementVectors[i] = new Vector2Int(MovementVectors[i].x, MovementVectors[i].y * -1);
				}
			}
		}
	}
}