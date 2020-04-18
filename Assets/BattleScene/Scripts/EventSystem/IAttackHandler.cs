using UnityEngine.EventSystems;

namespace DemonicCity.Battle
{
    public interface IAttackHandler : IEventSystemHandler
    {
        void OnAttack();
    }
}