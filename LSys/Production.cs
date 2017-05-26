using System;
using System.Collections.Generic;

namespace LSys
{
    public delegate void ParameterActionDelegate(List<Module> successor, Module leftContext, Module strictPredecessor, Module rightContext, int generation);
    public delegate bool ConditionDelegate(Module leftContext, Module strictPredecessor, Module rightContext);
    
    /// <summary>
    /// Class to define an L-System Production
    /// </summary>
    public class Production
    {
        // Form: id : lc < pred > rc : cond -> succ : prob

        #region Fields
        public int ID { get; private set; }
        public Module LeftContext { get; private set; }
        public Module StrictPredecessor { get; private set; }
        public Module RightContext { get; private set; }
        public ConditionDelegate Condition { get; private set; }
        public ParameterActionDelegate ParameterAction { get; private set; }
        private List<Module> _successor;
        public List<Module> Successor
        {
            get
            {
                List<Module> retList = new List<Module>();
                foreach (Module m in _successor)
                {
                    retList.Add(m.Clone() as Module);
                }
                return retList;
            }
        }
        public double Probability;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strictPredecessor">Strict Predecessor  Module of Production</param>
        /// <param name="successor">Production Successor as List of Modules</param>
        public Production(Module strictPredecessor, List<Module> successor)
        {
            StrictPredecessor = strictPredecessor;
            _successor = successor;
            ID = 0;
            LeftContext = null;
            RightContext = null;
            Condition = null;
            Probability = 1.0;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p">Production to clone</param>
        public Production(Production p)
        {
            ID = p.ID;
            LeftContext = p.LeftContext;
            StrictPredecessor = p.StrictPredecessor;
            RightContext = p.RightContext;
            Condition = p.Condition;
            ParameterAction = p.ParameterAction;
            _successor = p.Successor;
            Probability = p.Probability;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Returns the successor of this Production
        /// </summary>
        /// <param name="leftContext">Left Context of the Predecessor</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="rightContext">Right Context of the Predecessor</param>
        /// <param name="generation">The Generation of the L-System</param>
        /// <returns>Successor as List of Modules</returns>
        public List<Module> GetSuccessor(Module leftContext, Module predecessor, Module rightContext, int generation)
        {
            List<Module> successor = Successor;            
            ParameterAction?.Invoke(successor, leftContext, predecessor, rightContext, generation);
            return successor;
        }

        /// <summary>
        /// Determines if this production applies to a successor module
        /// </summary>
        /// <param name="leftContext">Left Context of the Predecessor</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="rightContext">Right Context of the Predecessor</param>
        /// <returns>True if this Production applies</returns>
        public bool AppliesTo(Module leftContext, Module predecessor, Module rightContext)
        {
            // TODO: Tidy up and improve robustness
            bool appliesTo = true;
            if (StrictPredecessor.Name == predecessor.Name && StrictPredecessor.Parameters.Length == predecessor.Parameters.Length)
            {
                if (LeftContext != null)
                {
                    if (leftContext == null || LeftContext.Name != leftContext.Name)
                    {
                        appliesTo = false;
                    }
                }
                if (RightContext != null)
                {
                    if (rightContext == null || RightContext.Name != rightContext.Name)
                    {
                        appliesTo = false;
                    }
                }
                if (appliesTo != false && Condition != null && !Condition(leftContext, predecessor, rightContext))
                {
                    appliesTo = false;
                } 
            }
            else
            {
                appliesTo = false;
            }
            return appliesTo;
        }

        /// <summary>
        /// Builder method for constructing a Production using method chaining
        /// </summary>
        /// <param name="strictPredecessor"></param>
        /// <param name="successor"></param>
        /// <returns></returns>
        public static Production Build(Module strictPredecessor, List<Module> successor)
        {
            return new Production(strictPredecessor, successor);
        }

        /// <summary>
        /// Sets the left context
        /// </summary>
        /// <param name="leftContext">Left Context Module</param>
        /// <returns>This production for method chaining</returns>
        public Production SetLeftContext(Module leftContext)
        {
            LeftContext = leftContext;
            return this;
        }

        /// <summary>
        /// Sets the Right Context
        /// </summary>
        /// <param name="rightContext">Right Context Module</param>
        /// <returns>This production for method chaining</returns>
        public Production SetRightContext(Module rightContext)
        {
            RightContext = rightContext;
            return this;
        }

        /// <summary>
        /// Sets the Condition Function
        /// </summary>
        /// <param name="condition">Condition Func</param>
        /// <returns>This production for method chaining</returns>
        public Production SetCondition(ConditionDelegate condition)
        {
            Condition = condition;
            return this;
        }

        /// <summary>
        /// Sets the Action that determines successor parameters
        /// </summary>
        /// <param name="pAction">Successor Parameter Action</param>
        /// <returns>This production for method chaining</returns>
        public Production SetParameterAction(ParameterActionDelegate pAction)
        {
            ParameterAction = pAction;
            return this;
        }

        /// <summary>
        /// Sets the probability (used for stochastic productions)
        /// </summary>
        /// <param name="probability">Stochastic Production Probability</param>
        /// <returns>This production for method chaining</returns>
        public Production SetProbability(double probability)
        {
            Probability = probability;
            return this;
        }

        /// <summary>
        /// Sets the Successor 
        /// </summary>
        /// <param name="successor">Successor as List of Modules</param>
        /// <returns>This production for method chaining</returns>
        public Production SetSuccessor(List<Module> successor)
        {
            _successor = successor;
            return this;
        }

        /// <summary>
        /// Returns a copy of this Production (useful for creating stochastic production sets)
        /// </summary>
        /// <returns>A copy of this Production</returns>
        public Production Copy()
        {
            return new Production(this);
        }
        #endregion Methods
    }
}
