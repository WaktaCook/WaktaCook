using WaktaCook.Mechanics.Characters;
using UnityEngine;

namespace WaktaCook.Model
{
    /// <summary>
    /// The main model containing needed data to implement a platformer style 
    /// game. This class should only contain data, and methods that operate 
    /// on the data. It is initialised with data in the GameController class.
    /// </summary>
    [System.Serializable]
    public class GameModel
    {
        //// 카메라 연출 사용할 일이 있을 것 같음
        ///// <summary>
        ///// The virtual camera in the scene.
        ///// </summary>
        //public Cinemachine.CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled 
        /// by the user.
        /// </summary>
        public Player player;

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform playerSpawnPoint;
    }
}