using System.Collections.Generic;

namespace LSys
{
    public abstract class LSystem<T>
    {
        protected ProductionSet _pSet = new ProductionSet();

        protected List<Module> _axiom;
        public List<Module> Axiom
        {
            get { return _axiom; }
            set { _axiom = value; }
        }

        public LSystem() { }

        public LSystem<T> SetAxiom(List<Module> axiom)
        {
            _axiom = axiom;
            return this;
        }

        public LSystem<T> AddProduction(Production p)
        {
            _pSet.AddProduction(p);
            return this;
        }

        public LSystem<T> AddStochasticProductionSet(List<Production> lP)
        {
            _pSet.AddStochasticProductionSet(lP);
            return this;
        }

        public abstract T GenerateOutput(int generations);
    }
}
