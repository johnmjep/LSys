namespace LSys
{
    public partial class Module
    {
        private static readonly Module _empty = new Module("Empty", new object[] { double.NaN });
        private static readonly Module _forward = new Module("F", new object[] { double.NaN });
        private static readonly Module _forwardBlank = new Module("f", new object[] { double.NaN });
        private static readonly Module _backward = new Module("B", new object[] { double.NaN });
        private static readonly Module _backwardBlank = new Module("b", new object[] { double.NaN });
        private static readonly Module _turnRight = new Module("+", new object[] { double.NaN });
        private static readonly Module _turnLeft = new Module("-", new object[] { double.NaN });
        private static readonly Module _setPosition = new Module("S", new object[] { double.NaN, double.NaN });
        private static readonly Module _setHeading = new Module("H", new object[] { double.NaN });
        private static readonly Module _stampPoint = new Module("@");
        private static readonly Module _pushStack = new Module("[");
        private static readonly Module _popStack = new Module("]");
        private static readonly Module _penUp = new Module("U");
        private static readonly Module _penDown = new Module("D");

        public static Module Empty { get { return new Module(_empty); } }
        public static Module Forward { get { return new Module(_forward); } }
        public static Module ForwardBlank { get { return new Module(_forwardBlank); } }
        public static Module Backward { get { return new Module(_backward); } }
        public static Module BackwardBlank { get { return new Module(_backwardBlank); } }
        public static Module TurnRight { get { return new Module(_turnRight); } }
        public static Module TurnLeft { get { return new Module(_turnLeft); } }
        public static Module SetPosition { get { return new Module(_setPosition); } }
        public static Module SetHeading { get { return new Module(_setHeading); } }
        public static Module StampPoint { get { return new Module(_stampPoint); } }
        public static Module PushStack { get { return new Module(_pushStack); } }
        public static Module PopStack { get { return new Module(_popStack); } }
        public static Module PenUp { get { return new Module(_penUp); } }
        public static Module PenDown { get { return new Module(_penDown); } }
    }
}