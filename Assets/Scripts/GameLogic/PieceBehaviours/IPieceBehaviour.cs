using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public interface IPieceBehaviour
    {
        List<Vector2Int> MovementVectors { get; }
        List<Vector2Int> AttackVectors { get; }
        bool HasInfiniteMovementRange { get; }
        void SetMovementDirection(bool shouldMoveFromTopToBottom);
    }
}