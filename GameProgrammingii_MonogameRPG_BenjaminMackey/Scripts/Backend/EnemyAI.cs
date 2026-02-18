using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend
{
    public class EnemyAIController : Vector2InputMap
    {
        public Transform _target;
        public Transform _self;

        public float _searchDistance;
        public IMovmentStrategy _movmentStrategy;
        public EnemyAIController(bool empty) : base(empty)
        {

        }
        public EnemyAIController() : base()
        {

        }
        public override void Update()
        {
            if (_movmentStrategy == null) return;
            Vector2 potentialMove = _movmentStrategy.MoveTo(this);
            x = potentialMove.x;
            y = potentialMove.y;
        }
    }

    //movment strategies
    public interface IMovmentStrategy
    {
        Vector2 MoveTo(EnemyAIController parent);
    }
    public class AgressiveMovmentStrategy : IMovmentStrategy
    {
        public Vector2 MoveTo(EnemyAIController parent)
        {
            if (parent._target == null || parent._self == null) return new Vector2(0,0);
            Vector3 movePos = parent._target._position - parent._self._position;
            
            if(movePos.Magnitude() > parent._searchDistance) return new Vector2(0,0);
            return Vector2.Normal(new Vector2(movePos.x, movePos.z));
        }
    }
}
