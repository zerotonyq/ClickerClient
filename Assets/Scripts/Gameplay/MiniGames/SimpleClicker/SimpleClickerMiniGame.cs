using System.Threading.Tasks;
using Gameplay.MiniGames.Base;
using UI.Elements;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.MiniGames.SimpleClicker
{
    public class SimpleClickerMiniGame : MiniGame
    {
        [SerializeField] protected SimpleAnimatedButton clickButton;
        [SerializeField] protected Canvas canvas;
        protected override async Task PrepareMiniGame()
        {
            clickButton.OnClick.AddListener(Process);
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 10;
            SetState(MiniGameState.Ready);
        }

        public override void Start()
        {
            SetState(MiniGameState.Running);
        }

        public override void End()
        {
            clickButton.OnClick.RemoveAllListeners();
            SetState(MiniGameState.Ended);            
        }

    }
}