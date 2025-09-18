using UnityEngine;
using GameLogic;

namespace Presentation
{
    [CreateAssetMenu(menuName = "PlayerCreators/" + nameof(FirstOptionAlwaysComputerPlayerCreator), fileName = nameof(FirstOptionAlwaysComputerPlayerCreator))]
    public class FirstOptionAlwaysComputerPlayerCreator : BasePlayerCreator
    {
        public override IPlayer CreateNewPlayer()
        {
            return new FirstOptionAlwaysComputerPlayer();
        }
    }
}