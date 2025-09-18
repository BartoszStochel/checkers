using UnityEngine;
using GameLogic;

namespace Presentation
{
    [CreateAssetMenu(menuName = "PlayerCreators/" + nameof(RandomMovesComputerPlayerCreator), fileName = nameof(RandomMovesComputerPlayerCreator))]
    public class RandomMovesComputerPlayerCreator : BasePlayerCreator
    {
        public override IPlayer CreateNewPlayer()
        {
            return new RandomMovesComputerPlayer();
        }
    }
}