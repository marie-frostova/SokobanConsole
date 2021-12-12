namespace Sokoban
{
    public class Command
    {
        public Command(AbstractEntity entity, int x, int y)
        {
            Entity = entity;
            X = x;
            Y = y;
        }

        public AbstractEntity Entity;
        public int X;
        public int Y;
        public int DeltaX;
        public int DeltaY;
        public AbstractEntity TransformTo;

        public int TargetX { get => X + DeltaX; }
        public int TargetY { get => Y + DeltaY; }
    }
}
