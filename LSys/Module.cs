using System;
using System.Collections;
using TurtleGraph2D;

namespace LSys
{
    /// <summary>
    /// Clase to hold a single L-System Module
    /// </summary>
    public partial class Module : IEnumerable, ICloneable
    {
        #region Fields
        public string Name { get; private set; }
        public object[] Parameters;        

        public object this[int i]
        {
            get { return Parameters[i]; }
            set { Parameters[i] = value; }
        }
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Module name</param>
        public Module(string name) 
            : this(name , null) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="module">Module to copy</param>
        public Module(Module module) 
            : this (module.Name, module.Parameters) 
        { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Module name</param>
        /// <param name="parameters">Module Parameters</param>
        public Module(string name, object[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Returns Enumerator for parameters array
        /// </summary>
        /// <returns>Enumerator for parameter array</returns>
        public IEnumerator GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }

        /// <summary>
        /// Overrides base ToString method
        /// </summary>
        /// <returns>String representation of object</returns>
        public override string ToString()
        {
            string retStr = string.Format("$$ Module | Name: {0}, Parameters: ", Name);
            if (Parameters != null)
            {
                foreach (double d in Parameters)
                {
                    retStr += string.Format("{0:0.00} ", d);
                } 
            }
            return retStr;
        }

        /// <summary>
        /// Sets the parameters of the module
        /// </summary>
        /// <param name="parameters">Module parameters</param>
        /// <returns>This Module</returns>
        public Module SetParameters(object[] parameters)
        {
            Parameters = parameters;
            return this;
        }

        /// <summary>
        /// Generates a Turtle formatted string representing this module
        /// </summary>
        /// <returns>Turtle formatted string command</returns>
        public string ToTurtleString()
        {
            return (Name + GetParametersAsString());
        }

        /// <summary>
        /// Generates a TurtleCommand from the Module (if possible)
        /// </summary>
        /// <returns>TurtleCommand representing Module</returns>
        public TurtleCommand ToTurtleCommand()
        {
            return new TurtleCommand(TurtleCommand.GetCommandFromRaw(Name), ParametersToDoubleArray());
        }

        /// <summary>
        /// Generates a comma delimited string of parameters e.g. (x,y,z)
        /// </summary>
        /// <returns>Parameters as bracketed comma delimited string</returns>
        private string GetParametersAsString()
        {
            string parameters = "";
            if (Parameters != null)
            {                
                parameters = "(" + string.Join(",", Parameters) + ")";
            }
            return parameters;
        }

        /// <summary>
        /// Converts Parameters to Double Array, where the parameter is actually a double, 0 if not
        /// </summary>
        /// <returns>Parameters as Double[]</returns>
        private double[] ParametersToDoubleArray()
        {
            double[] retArr = new double[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
            {
                try
                {
                    if(Parameters[i] is double)
                    {
                        retArr[i] = (double)Parameters[i];
                    }
                    else
                    {
                        retArr[i] = 0;
                    }
                }
                catch (FormatException exc)
                {
                    Console.WriteLine("XXX Encountered Format Exception parsing Module Parameters");
                    Console.WriteLine("XXX Message: {0}", exc.Message);
                    Console.WriteLine("XXX Source: {0}", exc.Source);
                    Console.WriteLine("XXX Stack: {0}", exc.StackTrace);
                    retArr[i] = 0;
                }
            }
            return retArr;
        }

        /// <summary>
        /// Returns true if Module is a query
        /// </summary>
        /// <returns>True if Module is a query</returns>
        public bool IsQuery()
        {
            return (Name[0] == '?');
        }

        /// <summary>
        /// Returns a new clone of this Module
        /// </summary>
        /// <returns>Clone of this Module</returns>
        public object Clone()
        {
            return new Module(this);
        }
        #endregion Methods
    }
}
