using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WaktaCook.Mechanics.Characters
{
    public abstract class Character : MonoBehaviour
    {
        public CharacterController2D Controller { get; private set; }
        public StateMachine stateMachine = new StateMachine();

        internal Animator animator;

        // 상태
        public bool controlEnabled = true;

        // TODO: 나중에 info로 빼야 됨
        // 세팅 값
        // 이동
        [Header("이동")]
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 3;

        protected virtual void Awake()
        {
            this.Controller = GetComponent<CharacterController2D>();
            animator = GetComponent<Animator>();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            InitializeStateMachine();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdateState();
        }

        // 각 캐릭터에 맞게 오버라이드 필요
        protected abstract void InitializeStateMachine();

        public virtual void Initialize()
        {
            stateMachine.Initialize();
            stateMachine.ChangeState("Standing");

            animator.SetBool("dead", false);
            animator.SetBool("attackDone", true);
            animator.SetBool("initialize", false);

            Controller.InitializeStates();
        }
    }
}