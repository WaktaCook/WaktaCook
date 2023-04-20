using WaktaCook.Core;
using WaktaCook.Mechanics.Characters;
using WaktaCook.Model;

namespace WaktaCook.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        readonly private GameModel _model = Simulation.GetModel<GameModel>();

        public override void Execute()
        {
            var player = _model.player;

            player.Initialize();
            player.controlEnabled = false;
            Simulation.Schedule<EnablePlayerInput>(1f);
        }
    }
}