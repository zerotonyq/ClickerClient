using System.Threading.Tasks;
using Gameplay.MiniGames.Base;
using Gameplay.MiniGames.SimpleClicker;
using UnityEngine;

namespace Gameplay.MiniGames.ScreenPointClicker
{
    public class ScreenPointClickerMiniGame : SimpleClickerMiniGame
    {
        protected override async Task PrepareMiniGame()
        {
            clickButton.OnClick.AddListener(() =>
            {
                var x = (float)(Screen.width * 0.7 * Random.value);
                var y = (float)(Screen.height * 0.7 * Random.value);
                clickButton.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, canvas.planeDistance));
                Process();
            });
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 10;
            SetState(MiniGameState.Ready);
        }
    }
}