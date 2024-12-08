using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.MiniGames.Base
{
    public enum MiniGameState
    {
        None,
        Ready,
        Running,
        Ended
    }
    public abstract class MiniGame : MonoBehaviour
    {
        [SerializeField] protected int performPoints;

        [SerializeField] protected List<Rule> rules = new();
        
        private MiniGameManager _manager;
        
        private MiniGameState _state;
        
        public async Task Initialize(MiniGameManager manager)
        {
            _manager = manager;
            
            await PrepareMiniGame();
        }
        

        protected abstract Task PrepareMiniGame();
        
        public abstract void Start();
        
        public abstract void End();
        
        private bool CheckRules() => rules.Count == 0 || rules.All(rule => rule.CheckPerformance());
        
        private void SendPointsToManager()
        {
            if (_state is not MiniGameState.Running and MiniGameState.Ended)
                return;
            
            _manager.AddPoints(performPoints);
        }
         
        protected void SetState(MiniGameState state) => _state = state;
        
        public void Process()
        {
            if (!CheckRules())
                return;
            
            SendPointsToManager();
        }
    }
}
