using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicGraph;
using TurtleGraph2D;

namespace LSys
{
    public class LSystemGraph : LSystem<Graph<Vector2>>
    {
        private Turtle _turtle = new Turtle();
        private string _intialTurtleString;

        public LSystemGraph(Vector2 startPos, double startHeading) : base()
        {
            _intialTurtleString = string.Format("H({0})S({1},{2})D@", startHeading, startPos.X, startPos.Y);
        }

        public override Graph<Vector2> GenerateOutput(int generations)
        {
            return ProduceGraphFromGrammar(GenerateGrammar(generations));
        }

        private List<Module> GenerateGrammar(int generations)
        {
            List<Module> output = new List<Module>(_axiom);
            Module leftContext;
            Module rightContext;
            for (int gen = 0; gen < generations; gen++)
            {
                List<Module> working = new List<Module>();

                for (int x = 0; x < output.Count; x++)
                {
                    leftContext = x > 0 ? output[x - 1] : Module.Empty;
                    rightContext = x < (output.Count - 1) ? output[x + 1] : Module.Empty;
                    working.AddRange(_pSet.ApplyTo(leftContext, output[x], rightContext, gen));
                }
                output = working;
            }
            return output;
        }

        private Graph<Vector2> ProduceGraphFromGrammar(List<Module> grammar)
        {
            string tString = _intialTurtleString;
            foreach (Module m in grammar)
            {
                tString += m.ToTurtleString();
            }
            return _turtle.GenerateGraph(tString);
        }
    }
}