using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSys
{
    public class ProductionSet
    {
        public List<List<Production>> _productionSet = new List<List<Production>>();

        private Random rand = new Random();

        public ProductionSet()
        {
        }

        public void AddProduction(Production p)
        {
            _productionSet.Add(new List<Production>() { p });
        }

        public void AddStochasticProductionSet(List<Production> lP)
        {
            _productionSet.Add(lP);
        }

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

        private List<Module> GetSuccessorFromNonStochastic(List<Production> lP, Module leftContext,
                                                           Module predecessor, Module rightContext,
                                                           int generation)
        {
            return lP[0].GetSuccessor(leftContext, predecessor, rightContext, generation);
        }

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

    }
}