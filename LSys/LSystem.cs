using System.Collections.Generic;

namespace LSys
{
    public abstract class LSystem<T>
    {
        #region Fields
        protected ProductionSet _pSet = new ProductionSet();

        protected List<Module> _axiom;
        public List<Module> Axiom
        {
            get { return _axiom; }
            set { _axiom = value; }
        }
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LSystem() { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sets the Axiom for the L-System
        /// </summary>
        /// <param name="axiom">L-System Axiom</param>
        /// <returns>This L-System for method chaining</returns>
        public LSystem<T> SetAxiom(List<Module> axiom)
        {
            _axiom = axiom;
            return this;
        }

        /// <summary>
        /// Adds a single (non-stochastic) production to the L-System
        /// </summary>
        /// <param name="p">Production to add</param>
        /// <returns>This L-System for method chaining</returns>
        public LSystem<T> AddProduction(Production p)
        {
            _pSet.AddProduction(p);
            return this;
        }

        /// <summary>
        /// Adds a stochastic production set to the L-System
        /// </summary>
        /// <param name="lP">Stochastic production set as a list of productions</param>
        /// <returns>This L-System for method chaining</returns>
        public LSystem<T> AddStochasticProductionSet(List<Production> lP)
        {
            _pSet.AddStochasticProductionSet(lP);
            return this;
        }

        /// <summary>
        /// Generates the output of the L-System for the given number of generations 
        /// </summary>
        /// <param name="generations">Number of Generations to apply to the L-System</param>
        /// <param name="grammarOutput">The final result of the system as a list of modules</param>
        /// <param name="startPoint">Optional starting point of the L-System</param>
        /// <returns>Output specified by overriding implementation</returns>
        public abstract T GenerateOutput(int generations, out List<Module> grammarOutput, List<Module> startPoint = null);
        #endregion Methods
    }
}

