//Coordinate.cs

namespace Client3
{
    internal class Coordinate
    {
        public int MiddleX { get; set; }
        public int MiddleY { get; set; }
        public int LabelX { get; set; }
        public int LabelY { get; set; }

        public Coordinate(int middleX, int middleY, int boxX, int boxY)
        {
            MiddleX = middleX;
            MiddleY = middleY;
            LabelX = boxX;
            LabelY = boxY;
        }
    }
}
