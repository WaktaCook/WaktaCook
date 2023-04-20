using WaktaCook.Core;
using WaktaCook.Gameplay;
using WaktaCook.Model;
using UnityEngine;

namespace WaktaCook.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        PlayerControls controls;

        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public GameModel model = Simulation.GetModel<GameModel>();

        private void Awake()
        {
            controls = new PlayerControls();
            controls.UI.Restart.started += 
                ctx => {
                    
                };
            controls.UI.Restart.performed +=
                ctx => {
                    Simulation.Schedule<Restart>();
                };
        }

        void OnEnable()
        {
            // 중복 처리
            if (Instance != null)
            {
#if ENABLE_LOG
                Debug.LogAssertion("Game Controller instance is duplicated!");          
#endif
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            controls.Enable();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
            controls.Disable();
        }

        void Start()
        {
            Simulation.Schedule<PlayerSpawn>();
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}