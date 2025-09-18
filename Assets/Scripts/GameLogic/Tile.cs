namespace GameLogic
{
    public class Tile
    {
        public bool CanPieceBePlacedHere { get; private set; }
        public Piece PieceOnTile { get; private set; }

        public Tile(bool canPieceBePlacedHere)
		{
            CanPieceBePlacedHere = canPieceBePlacedHere;
		}

        public bool TrySetPieceOnTile(Piece newPiece)
		{
            if (!CanPieceBePlacedHere || PieceOnTile != null)
			{
                return false;
			}

            PieceOnTile = newPiece;
            return true;
		}

        public void RemovePieceFromTile()
		{
            PieceOnTile = null;
		}
    }
}