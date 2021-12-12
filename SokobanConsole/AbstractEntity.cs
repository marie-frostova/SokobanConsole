using System.Collections.Generic;

namespace Sokoban
{
    public abstract class AbstractEntity
    {
        public abstract string GetImageFileName();

        public abstract List<Command> Act(int x, int y, Keys keys);

        public abstract bool IsMovable();

        public abstract AbstractEntity Transform(ICell targetCell);

        public abstract string GetString();

        public abstract ConsoleColor GetColor();

        ICell cell;

        public ICell Cell { get => cell; set => cell = value; }
    }
}
