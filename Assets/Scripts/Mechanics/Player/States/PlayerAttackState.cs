using System.Collections;
using UnityEngine;

namespace WaktaCook.Mechanics.Characters
{
    public class PlayerAttackState : BaseState
    {
        private Player _player;
        private PlayerControls _controls;

        private float _moveX;

        private void Awake()
        {
            _player = GetComponent<Player>();

            // 컨트롤 바인딩
            _controls = new PlayerControls();

            _controls.Gameplay.Move.performed += ctx =>
            {

            };
            _controls.Gameplay.Move.canceled += ctx =>
            {
                
            };            
        }

        public override void Enter()
        {
            _controls.Enable();

            _moveX = 0;

            _player.animator.SetTrigger("attack");
            _player.animator.SetBool("attackDone", false);
            StartAttack();
        }

        public override void LogicUpdate()
        {
            
        }

        public override void Exit()
        {
            _controls.Disable();
        }

        public override void Initailize()
        {
            
        }

        private void StartAttack()
        {
            
        }

        private IEnumerator Attack()
        {
            yield return null;
        }

    }
}
