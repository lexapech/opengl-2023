using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    
    internal class Fractal
    {
        public struct State
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Scale;         
            public State(Vector2 position,Vector2 direction,float scale)
            {
                Position = position;
                Direction = direction;
                Scale = scale;
            }
        }

        public class Branch
        {
            public Vector2 Start;
            public Vector2 End;
            public Vector2 Direction;
            public float Scale;
            public float Length;
            public int Mirror;
            public Branch(Vector2 start,Vector2 direction, float scale, float length,int mirror = 1)
            {
                Start = start;
                Direction = direction;
                Scale = scale;
                Mirror = mirror;
                End = start + direction * scale * length;
                Length = length;
            }
            public Branch(State state) : this(state.Position, state.Direction, state.Scale,1) { }
            public Branch(State state, float length,int mirror) : this(state.Position, state.Direction, state.Scale, length, mirror) { }
        }


        public List<Branch> Branches = new List<Branch>();
        public int Steps;
        private State _currentState;
        private readonly Stack<State> _states = new Stack<State>();
        private readonly List<Branch> _terminals = new List<Branch>();
        private Random _random;
        private int _seed;

        public Fractal()
        {
            Steps = 1;
            var seedRandom = new Random();
            _seed = seedRandom.Next();
            _random = new Random(_seed);
        }

        public void ChangeSeed()
        {
            var seedRandom = new Random();
            _seed = seedRandom.Next();
        }

        public void Generate()
        {
            _random = new Random(_seed);
            _currentState = new State(Vector2.Zero,Vector2.UnitY,1);
            _states.Clear();
            Branches.Clear();
            _terminals.Clear();
            Scale(500);
            Rotate(-5);
            AddLeaf(0.08f,1);

            for (int i = 0; i < Steps; i++)
            {
                int count = _terminals.Count;
                for (int j = 0; j < count;  j++)
                {
                    _currentState.Position = _terminals[j].End;
                    _currentState.Direction = _terminals[j].Direction;
                    _currentState.Scale = _terminals[j].Scale;
                    int mirror = _terminals[j].Mirror;

                    PushState();
                    Rotate(-50 * mirror);
                    AddLine(0.19f);
                    Rotate(45 * mirror);
                    AddLine(0.08f);
                    Rotate(45 * mirror);
                    Scale(2 / 3f);
                    AddLeaf(0.08f *1.5f, GetRandomMirror());
                    PopState();

                    Rotate(45 * mirror);
                    AddLine(0.11f);
                    PushState();
                    Rotate(-31 * mirror);
                    Scale(1 / 3f);
                    AddLeaf(0.09f * 3, GetRandomMirror());
                    PopState();
                    Rotate(11 * mirror);
                    Scale(1 / 3f);
                    AddLeaf(0.15f * 3, GetRandomMirror());
                }
                _terminals.RemoveRange(0, count);
            }
        }
     
        private void AddLine(float length)
        {
            var newBranch = new Branch(_currentState, length,1);
            Branches.Add(newBranch);
            _currentState.Position = newBranch.End;
        }

        private void AddLeaf(float length,int mirror)
        {
            var newBranch = new Branch(_currentState, length, mirror);
            Branches.Add(newBranch);
            _terminals.Add(newBranch);
            _currentState.Position = newBranch.End;
        }

        private void Rotate(float angle)
        {
            _currentState.Direction = Vector2.Transform(_currentState.Direction,Quaternion.FromEulerAngles(0,0, -angle/180*(float)Math.PI));
        }

        private void Scale(float factor)
        {
            _currentState.Scale *= factor;
        }

        private void PushState()
        {
            _states.Push(_currentState);
        }

        private void PopState()
        {
            _currentState = _states.Pop();
        }

        private int GetRandomMirror()
        {
            return _random.Next(2) == 1 ? 1 : -1;
        }
    }
}
