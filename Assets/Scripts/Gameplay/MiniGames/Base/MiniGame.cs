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
        [SerializeField] private int performPoints;

        [SerializeField] private List<Rule> rules;
        
        private MiniGameManager _manager;
        
        private MiniGameState _state;
        
        public async Task Initialize(MiniGameManager manager)
        {
            _manager = manager;
            
            await PrepareMiniGame();
            
            SetState(MiniGameState.Ready);
        }
        

        protected abstract Task PrepareMiniGame();
        
        public abstract void Start();
        
        public abstract void End();
        
        private bool CheckRules() => rules.All(rule => rule.CheckPerformance());
        
        private void SendPointsToManager()
        {
            if (_state is not MiniGameState.Running and MiniGameState.Ended)
                return;
            
            _manager.AddPoints(performPoints);
        }
         
        private void SetState(MiniGameState state)
        {
            if (state == _state)
                return;
            
            SetStateInternal(state);

            _state = state;
        }

        protected abstract void SetStateInternal(MiniGameState state);
        
        public void Process()
        {
            if (!CheckRules())
                return;
            
            SendPointsToManager();
        }
    }
}
