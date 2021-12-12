using Sokoban;

for (int levelIndex = 0; levelIndex < MapCreator.Levels.Length; levelIndex += Game.IsOver ? 1 : 0)
{
    Game.CreateMap(levelIndex);

    while (!Game.IsOver)
    {
        var map = Game.Map;
        Console.Clear();
        Console.WriteLine("Level " + (levelIndex + 1).ToString() + ":\n");
        for (var i = 0; i < Game.MapWidth; ++i)
        {
            for (var j = 0; j < Game.MapHeight; ++j)
            {
                Console.ForegroundColor = map[i, j].GetColor();
                Console.Write(map[i, j].GetString());
            }
            Console.WriteLine();
        }
        Console.WriteLine("\nR to Restart");

        var dir = Console.ReadKey();
        Keys key = Keys.n;
        bool restart = false;
        switch (dir.Key)
        {
            case ConsoleKey.LeftArrow: key = Keys.d; break;
            case ConsoleKey.RightArrow: key = Keys.u; break;
            case ConsoleKey.UpArrow: key = Keys.l ; break;
            case ConsoleKey.DownArrow: key = Keys.r; break;
            case ConsoleKey.R: restart = true;  break;
        }

        if (restart)
        {
            break;
        }

        var commandList = new List<Command>();
        for (var i = 0; i < Game.MapWidth; ++i)
        {
            for (var j = 0; j < Game.MapHeight; ++j)
            {
                if (map[i, j].Entity == null)
                    continue;
                var commandListAdd = map[i, j].Entity.Act(i, j, key);
                if (commandListAdd == null)
                    continue;
                commandList.AddRange(commandListAdd);
            }
        }
        foreach (var command in commandList)
        {
            if (command.DeltaX != 0 || command.DeltaY != 0)
            {
                map[command.TargetX, command.TargetY].Entity = command.TransformTo ?? command.Entity;
                map[command.X, command.Y].Entity = null;
            }
        }
        Game.CheckIsOver();
    }
}
Console.Clear();
Console.WriteLine("U WIN!");
Console.ReadKey();
