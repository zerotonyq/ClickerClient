using System.Threading.Tasks;
using Gameplay.MiniGames.Base;
using UI.Elements;
using UnityEngine;

namespace Gameplay.MiniGames.SimpleClicker
{
    public class SimpleClickerMiniGame : MiniGame
    {
        [SerializeField] private SimpleAnimatedButton clickButton;
        [SerializeField] private Canvas _canvas;
        protected override async Task PrepareMiniGame()
        {
            clickButton.OnClick.AddListener(Process);
            _canvas.worldCamera = Camera.main;
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