using UnityEngine.EventSystems;

namespace DemonicCity.BattleScene
{
    public interface IAttackHandler : IEventSystemHandler
    {
        void OnAttack();
    }
}