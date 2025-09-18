using System.Collections.Generic;

namespace GameLogic
{
    public class ActionVectorAndItsTarget
    {
        public List<Tile> LandingTargets { get; private set; }
        public Piece AttackTarget { get; private set; }

        public ActionVectorAndItsTarget(List<Tile> landingTargets, Piece attackTarget)
		{
            LandingTargets = landingTargets;
            AttackTarget = attackTarget;
        }
    }
}