using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public interface ICell
    {
        bool CanStep();
        bool IsFinish();
        AbstractEntity Entity { get; set; }
        string GetImageFileName();
        string GetString();

        ConsoleColor GetColor();
    }
}
