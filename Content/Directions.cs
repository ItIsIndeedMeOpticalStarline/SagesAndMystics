namespace SagesAndMystics.Content
{
    public static class Directions
    {
        public enum Direction
        {
            Left = -1,
            Right = 1
        }

        public static Direction DirectionRelativeTo(float x1, float x2) { return x1 > x2 ? Direction.Right : Direction.Left; }

        public static Direction Flip(Direction d) { return (Direction)((int)d * -1); } // Ugly hack

        public static int ToValue(Direction d) { return (int)d; }
    }
}
