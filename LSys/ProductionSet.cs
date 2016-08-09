using System;
using System.Collections.Generic;

namespace LSys
{
    /// <summary>
    /// Class to handle L-System production sets
    /// </summary>
    public class ProductionSet
    {
        #region Fields
        public List<List<Production>> _productionSet = new List<List<Production>>();
        private Random rand = new Random();
        #endregion Fields

        #region Constructors
        public ProductionSet() { }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Adds a single Production to the set
        /// </summary>
        /// <param name="p">Production to add</param>
        public void AddProduction(Production p)
        {
            _productionSet.Add(new List<Production>() { p });
        }

        /// <summary>
        /// Adds a stochastic Production set to the set
        /// </summary>
        /// <param name="lP">Stochastic Productions as a List of Production</param>
        public void AddStochasticProductionSet(List<Production> lP)
        {
            _productionSet.Add(lP);
        }

        /// <summary>
        /// Finds a applicable Production and applies it (if one is found)
        /// </summary>
        /// <param name="leftContext">Left Context Module</param>
        /// <param name="predecessor">Predecessor Module</param>
        /// <param name="rightContext">Right Context Module</param>
        /// <param name="generation">Generation</param>
        /// <returns>Successor as a List of Modules, or null if none apply</returns>
        public List<Module> ApplyTo(Module leftContext, Module predecessor, Module rightContext, int generation)
        {
            List<Module> successor = null;
            List<Production> applicableSet = null;
            foreach (List<Production> lP in _productionSet)
            {
                if (lP[0].AppliesTo(leftContext, predecessor, rightContext))
                {
                    applicableSet = lP;
                    break;
                }
            }
            if (applicableSet != null)
            {
                if (applicableSet.Count > 1)
                {
                    successor = GetSuccessorFromStochastic(applicableSet, leftContext, predecessor, 
                                                           rightContext, generation);
                }
                else
                {
                    successor = GetSuccessorFromNonStochastic(applicableSet, leftContext, predecessor, 
                                                              rightContext, generation);
                }
            }
            if (successor == null)
            {
                successor = new List<Module>() { new Module(predecessor) };
            }
            return successor;
        }

        /// <summary>
        /// Determines the successor from a non-stochastic Production
        /// </summary>
        /// <param name="lP">Applicable Production</param>
        /// <param name="leftContext">Left Context Module</param>
        /// <param name="predecessor">Predecessor Module</param>
        /// <param name="rightContext">Right Context Module</param>
        /// <param name="generation">Generation</param>
        /// <returns>Successor as List of Modules</returns>
        private List<Module> GetSuccessorFromNonStochastic(List<Production> lP, Module leftContext, 
                                                           Module predecessor, Module rightContext,
                                                           int generation)
        {
            return lP[0].GetSuccessor(leftContext, predecessor, rightContext, generation);
        }

        /// <summary>
        /// Determines the successor from a stochastic Production set
        /// </summary>
        /// <param name="lP">Applicable Production Set</param>
        /// <param name="leftContext">Left Context Module</param>
        /// <param name="predecessor">Predecessor Module</param>
        /// <param name="rightContext">Right Context Module</param>
        /// <param name="generation">Generation</param>
        /// <returns>Successor as List of Modules</returns>
        private List<Module> GetSuccessorFromStochastic(List<Production> lP, Module leftContext, 
                                                        Module predecessor, Module rightContext,
                                                        int generation)
        {
            List<Module> successor = null;
            double rnd = rand.NextDouble();
            foreach (Production p in lP)
            {
                if (rnd <= p.Probability)
                {
                    successor = p.GetSuccessor(leftContext, predecessor, rightContext, generation);
                    break;
                }
            }
            // If probabilities are mismatched, pick the last one
            if (successor == null)
            {
                successor = lP[lP.Count - 1].GetSuccessor(leftContext, predecessor, rightContext, generation);
            }
            return successor;
        }
        #endregion Methods
    }
}
