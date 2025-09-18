using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public interface IBoard
    {
        int SizeX { get; }
        int SizeY { get; }
        Tile[,] Tiles { get; }

        event Action<Piece, int, int> EventPieceSpawnedOnPosition;

        void PlacePiecesOnProperTilesAndSetMovementDirection(List<Piece> pieces, bool shouldPlaceOnTopOfTheBoard);
        void PlacePieceOnCustomPosition(Piece piece, Vector2Int position, bool hasStartedGameOnTopOfTheBoard);
        Vector2Int GetPiecePositionOnBoard(Piece piece);
        Vector2Int GetTilePositionOnBoard(Tile tile);
        bool ManHasReachedOppositeEdgeOfTheBoard(Piece piece);
        void InvokeEventPieceSpawnedOnPosition(Piece piece, Tile tile);
    }
}