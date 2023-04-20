using WaktaCook.Core;
using WaktaCook.Model;

namespace WaktaCook.Gameplay
{
    /// <summary>
    /// This event is fired when user input should be enabled.
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput>
    {
        readonly private GameModel _model = Simulation.GetModel<GameModel>();

        public override void Execute()
        {
            var player = _model.player;
            player.controlEnabled = true;
        }
    }
}