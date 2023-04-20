using WaktaCook.Core;
using WaktaCook.Mechanics;
using WaktaCook.Model;

namespace WaktaCook.Gameplay
{
    /// <summary>
    /// 게임 재시작 버튼 누르면 할 일들
    /// </summary>
    public class Restart : Simulation.Event<Restart>
    {
        public override void Execute()
        {
            Simulation.Clear();
            Simulation.Schedule<PlayerSpawn>();
        }
    }
}