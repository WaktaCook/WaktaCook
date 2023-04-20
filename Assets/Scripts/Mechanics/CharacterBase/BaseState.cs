using UnityEngine;

namespace WaktaCook.Mechanics.Characters
{
    public class BaseState : MonoBehaviour
    {
        public virtual void Enter()
        {
            
        }

        // 게임 로직
        public virtual void LogicUpdate()
        {

        }

        // 물리 연산
        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Initailize()
        {

        }
    }
}
