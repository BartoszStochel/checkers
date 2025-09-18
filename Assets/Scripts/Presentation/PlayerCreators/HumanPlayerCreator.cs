using UnityEngine;
using GameLogic;

namespace Presentation
{
	[CreateAssetMenu(menuName = "PlayerCreators/" + nameof(HumanPlayerCreator), fileName = nameof(HumanPlayerCreator))]
	public class HumanPlayerCreator : BasePlayerCreator
    {
		public override IPlayer CreateNewPlayer()
		{
			return new HumanPlayer();
		}
	}
}