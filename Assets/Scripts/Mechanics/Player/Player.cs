using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaktaCook.Core;
using WaktaCook.Gameplay;

namespace WaktaCook.Mechanics.Characters
{
    public class Player : Character
    {
        // 공격
        [HideInInspector]
        public bool canAttack = true;
        [HideInInspector]
        public bool isAttacking = false;
        [HideInInspector]
        public bool moveWhileAttacking = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Initialize()
        {
            base.Initialize();

            canAttack = true;
            isAttacking = false;
            moveWhileAttacking = false;
        }

        protected override void InitializeStateMachine()
        {
            stateMachine.AddState("Standing", GetComponent<PlayerStandingState>());
            stateMachine.AddState("Attack", GetComponent<PlayerAttackState>());

            stateMachine.ChangeState("Standing");
        }
    }
}