using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public enum Keys { l, r, u, d, n };
    internal class Game
    {
        public static bool IsOver;
        public static int Level;
        public static ICell[,] Map;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);
        public static void CreateMap(int level)
        {
            Map = MapCreator.CreateMap(level);
            IsOver = false;
        }

        public static void CheckIsOver()
        {
            IsOver = true;
            for (var i = 0; i < MapWidth; ++i)
            {
                for (var j = 0; j < MapHeight; ++j)
                {
                    if(Map[i, j].IsFinish() && ! (Map[i, j].Entity is FinishedBox))
                    {
                        IsOver = false;  
                    }
                }
            }
        }
    }

    public class Player : AbstractEntity
    {
        public override List<Command> Act(int x, int y, Keys KeyPressed)
        {
            var commands = new List<Command>();
            var res = new Command(this, x, y);
            var map = Game.Map;
            if (KeyPressed == Keys.u && y < Game.MapHeight - 1)
                res.DeltaY += 1;
            else if (KeyPressed == Keys.d && y > 0)
                res.DeltaY -= 1;
            else if (KeyPressed == Keys.r && x < Game.MapWidth - 1)
                res.DeltaX += 1;
            else if (KeyPressed == Keys.l && x > 0)
                res.DeltaX -= 1;

            var cell = map[res.DeltaX + x, res.DeltaY + y];
            if (cell is Wall)
            {
                res.DeltaX = 0;
                res.DeltaY = 0;
            }
            else if (cell.Entity != null && cell.Entity.IsMovable())
            {
                var nextCell = map[2 * res.DeltaX + x, 2 * res.DeltaY + y];
                if (!(nextCell is Empty) || nextCell.Entity != null)
                {
                    res.DeltaX = 0;
                    res.DeltaY = 0;
                }
                Command move = new Command(cell.Entity, x + res.DeltaX, y + res.DeltaY);
                move.DeltaX = res.DeltaX;
                move.DeltaY = res.DeltaY;
                move.TransformTo = cell.Entity.Transform(nextCell);
                commands.Add(move);
            }

            commands.Add(res);
            return commands;
        }

        public override string GetImageFileName()
        {
            return "Player.png";
        }

        public override string GetString()
        {
            return "@";
        }

        public override ConsoleColor GetColor()
        {
            return ConsoleColor.Cyan;
        }

        public override bool IsMovable()
        {
            return false;
        }

        public override AbstractEntity Transform(ICell cell)
        {
            return this;
        }
    }

    public class Box : AbstractEntity
    {
        public override List<Command> Act(int x, int y, Keys KeyPressed)
        {
            return null;
        }

        public override string GetImageFileName()
        {
            return "Box.png";
        }

        public override string GetString()
        {
            return "O";
        }
        public override ConsoleColor GetColor()
        {
            return ConsoleColor.Yellow;
        }

        public override bool IsMovable()
        {
            return true;
        }
        public override AbstractEntity Transform(ICell cell)
        {
            return cell.IsFinish() ? new FinishedBox() : new Box();
        }
    }

    public class FinishedBox : Box
    {
        public override string GetImageFileName()
        {
            return "FinishedBox.png";
        }
        public override ConsoleColor GetColor()
        {
            return ConsoleColor.Green;
        }
    }

    public class Wall : ICell
    {
        AbstractEntity ICell.Entity { get => null; set => throw new NotImplementedException(); }

        public bool CanStep()
        {
            return false;
        }

        public string GetImageFileName()
        {
           return "Wall.png";
        }

        public string GetString()
        {
            return "#";
        }
        public ConsoleColor GetColor()
        {
            return ConsoleColor.White;
        }

        public bool IsFinish()
        {
            return false;
        }
    }

    public class Empty : ICell
    {
        protected AbstractEntity entity;

        AbstractEntity ICell.Entity {
            get => entity;
            set {
                entity = value;
                if (value != null)
                    value.Cell = this;
            }
        }

        public bool CanStep()
        {
            return true;
        }

        public string GetImageFileName()
        {
            return entity == null ? null : entity.GetImageFileName();
        }

        public virtual string GetString()
        {
            return entity == null ? " " : entity.GetString();
        }
        public virtual ConsoleColor GetColor()
        {
            return entity == null ? ConsoleColor.White : entity.GetColor();
        }

        public virtual bool IsFinish()
        {
            return false;
        }
    }

    public class Finish : Empty
    {
        public override bool IsFinish()
        {
            return true;
        }
        public override string GetString()
        {
            return entity == null ? "+" : entity.GetString();
        }
        public override ConsoleColor GetColor()
        {
            return entity == null ? ConsoleColor.Green : entity.GetColor();
        }
    }
}
