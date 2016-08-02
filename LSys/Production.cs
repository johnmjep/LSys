using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSys
{
    public class Production
    {
        // id : lc < pred > rc : cond -> succ : prob
        public int ID { get; private set; }
        public Module LeftContext { get; private set; }
        public Module StrictPredecessor { get; private set; }
        public Module RightContext { get; private set; }
        public Func<Module, Module, Module, bool> Condition { get; private set; }
        public Action<List<Module>, Module, Module, Module, int> ParameterAction { get; private set; }
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

        public List<Module> GetSuccessor(Module leftContext, Module predecessor, Module rightContext, int generation)
        {
            List<Module> successor = Successor;
            ParameterAction?.Invoke(successor, leftContext, predecessor, rightContext, generation);
            return successor;
        }

        public bool AppliesTo(Module leftContext, Module predecessor, Module rightContext)
        {
            // TODO: Tidy up and improve robustness
            bool appliesTo = true;
            if (LeftContext != null)
            {
                if (leftContext == null || LeftContext.Name != leftContext.Name)
                {
                    appliesTo = false;
                }
            }
            if (StrictPredecessor.Name != predecessor.Name)
            {
                appliesTo = false;
            }
            if (RightContext != null)
            {
                if (rightContext == null || RightContext.Name != rightContext.Name)
                {
                    appliesTo = false;
                }
            }
            if (Condition != null && !Condition(leftContext, predecessor, rightContext))
            {
                appliesTo = false;
            }
            return appliesTo;
        }

        public static Production Build(Module strictPredecessor, List<Module> successor)
        {
            return new Production(strictPredecessor, successor);
        }

        public Production SetLeftContext(Module leftContext)
        {
            LeftContext = leftContext;
            return this;
        }

        public Production SetRightContext(Module rightContext)
        {
            RightContext = rightContext;
            return this;
        }

        public Production SetCondition(Func<Module, Module, Module, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public Production SetParameterAction(Action<List<Module>, Module, Module, Module, int> pAction)
        {
            ParameterAction = pAction;
            return this;
        }

        public Production SetProbability(double probability)
        {
            Probability = probability;
            return this;
        }

        public Production SetSuccessor(List<Module> successor)
        {
            _successor = successor;
            return this;
        }

        public Production Copy()
        {
            return new Production(this);
        }
    }
}