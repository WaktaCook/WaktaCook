using WaktaCook.Core;
using WaktaCook.Model;
using UnityEngine;

namespace WaktaCook.Mechanics.Characters
{
    public enum Dir
    {
        Left,
        Right,
        Up,
        Down
    }

    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class CharacterController2D : MonoBehaviour
    {
        // 컴포턴트
        [HideInInspector]
        public Collider2D collider2d;
        SpriteRenderer spriteRenderer;
        
        readonly private GameModel model = Simulation.GetModel<GameModel>();

        float maxSpeed = 3;

        public Dir initialDir;
        [HideInInspector]
        public Dir dir = Dir.Right;

        Vector2 move;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider2d = GetComponentInChildren<Collider2D>();
        }

        protected void Start()
        {
            InitializeStates();
        }

        public void InitializeStates()
        {
            collider2d.enabled = true;
           
            move = Vector2.zero;
        }

        protected void FixedUpdate()
        {
            
        }

        protected void ComputeVelocity()
        {
            
        }

        public void MoveX(float value)
        {
            move = Vector2.right * value * maxSpeed;
        }

        // 바로 캐릭터 아래에 붙은 자식들만 플립이 되기 때문에 오프셋 위치를 바꾸고 싶으면
        // 자식은 0, 0으로 두고 자식의 자식의 위치를 변환하면 됨
        public void FlipChildObjects()
        {
            int sign = dir == Dir.Left ? -1 : 1;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                childTransform.localScale = new Vector3(Mathf.Abs(childTransform.localScale.x) * sign, childTransform.localScale.y, childTransform.localScale.z);
            }
        }
    }
}
