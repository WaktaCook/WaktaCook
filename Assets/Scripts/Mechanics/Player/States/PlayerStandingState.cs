namespace WaktaCook.Mechanics.Characters
{
    public class PlayerStandingState : BaseState
    {
        private Player _player;
        private PlayerControls _controls;

        private void Awake()
        {
            _player = GetComponent<Player>();

            // 컨트롤 바인딩
            _controls = new PlayerControls();
        }

        public override void Enter()
        {
            
        }

        public override void LogicUpdate()
        {

        }

        public override void Exit()
        {
            _controls.Disable();
        }
    }
}
