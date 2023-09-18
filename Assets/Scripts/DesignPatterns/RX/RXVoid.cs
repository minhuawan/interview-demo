namespace DesignPatterns.RX
{
    public sealed class RXVoid
    {
        public static readonly RXVoid Void = new RXVoid();

        // Disallow to create instance
        private RXVoid()
        {
        }
    }
}