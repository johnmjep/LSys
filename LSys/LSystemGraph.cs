using System.Collections.Generic;
using BasicGraph;
using TurtleGraph2D;

namespace LSys
{
    /// <summary>
    /// Implementation of LSystem to create a Graph output
    /// </summary>
    public class LSystemGraph : LSystem<Graph<Vector2>>
    {
        #region Fields
        private Turtle _turtle = new Turtle();
        private Turtle _positionInterpretationTurtle = new Turtle();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public LSystemGraph() : base() { }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// Runs the L-System and produces a Graph output
        /// </summary>
        /// <param name="generations"></param>
        /// <param name="grammarOutput"></param>
        /// <param name="startPoint"></param>
        /// <returns></returns>
        public override Graph<Vector2> GenerateOutput(int generations, out List<Module> grammarOutput,  List<Module> startPoint = null)
        {
            _turtle.Initialise(0, 0);
            _positionInterpretationTurtle.Initialise(0, 0);
            grammarOutput = GenerateGrammar(generations, startPoint);
            return ProduceGraphFromGrammar(grammarOutput);
        }

        /// <summary>
        /// Generates the L-System Grammar output
        /// </summary>
        /// <param name="generations">Number of generations to perform</param>
        /// <param name="startPoint">Starting point of the L-System (could be the axiom or previous output)</param>
        /// <returns>Grammar as List of Modules</returns>
        private List<Module> GenerateGrammar(int generations, List<Module> startPoint)
        {
            List<Module> output = (startPoint == null) ? new List<Module>(_axiom) : startPoint;
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
                ProcessQueries(ref working);
                output = working;                
            }
            return output;
        }

        /// <summary>
        /// Runs through the L-System and fills in any query modules
        /// </summary>
        /// <param name="uninterpretedSet">L-System grammar with unfilled queries</param>
        private void ProcessQueries(ref List<Module> uninterpretedSet)
        {
            Vector2 currentPosition = new Vector2();
            foreach (Module m in uninterpretedSet)
            {
                currentPosition = _positionInterpretationTurtle.FindFinalPositionFrom(m.ToTurtleString());
                if (m == Module.PositionQuery)
                {
                    m.Parameters = new object[] { currentPosition.X, currentPosition.Y };
                }
            }
        }

        /// <summary>
        /// Produces a Graph from the L-System Grammar
        /// </summary>
        /// <param name="grammar">L-System Grammar as a List of Modules</param>
        /// <returns>Graph of Vector2</returns>
        private Graph<Vector2> ProduceGraphFromGrammar(List<Module> grammar)
        {
            string tString = "";
            foreach (Module m in grammar)
            {
                tString += m.ToTurtleString();
            }
            return _turtle.GenerateGraph(tString);
        }
        #endregion Methods
    }
}
