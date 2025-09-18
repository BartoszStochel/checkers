using UnityEngine;
using GameLogic;

namespace Presentation
{
    public abstract class BasePlayerCreator : ScriptableObject
    {
        public abstract IPlayer CreateNewPlayer();
    }
}