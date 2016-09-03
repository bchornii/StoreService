namespace ServiceDomain.Utility
{
    public sealed class Helper
    {
        private Helper() { }
        public static Helper Instance { get { return Nested.instance; } }
        private class Nested
        {
            static Nested() { }
            internal static readonly Helper instance = new Helper();
        }        
    }
}