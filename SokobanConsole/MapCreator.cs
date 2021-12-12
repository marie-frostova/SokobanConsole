namespace Sokoban
{
    internal class MapCreator
    {
        public static readonly string[] Levels = new string[]
        {
            @"
WWWWWWWWWW
W        W
W F      W
W  Wb b  W
W   FFbP W
W        W
WWWWWWWWWW",
            @"
WWWWW
WP  W
W b W
W  FW
WWWWW",
            @"
WWWWW
WP  W
W b W
W  FW
WWWWW"
        };

        private static readonly Dictionary<Tuple<string, string>, Tuple<Func<ICell>, Func<AbstractEntity>>> CellFactory = new Dictionary<Tuple<string, string>, Tuple<Func<ICell>, Func<AbstractEntity>>>();

        public static ICell[,] CreateMap(int level, string separator = "\r\n")
        {
            var map = Levels[level];
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var result = new ICell[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateCellBySymbol(rows[y][x]);
            return result;
        }

        private static ICell CreateCellBySymbol(char c)
        {
            switch (c)
            {
                case 'P':
                    return CreateCellByTypeName(typeof(Empty), typeof(Player));
                case 'b':
                    return CreateCellByTypeName(typeof(Empty), typeof(Box));
                case 'B':
                    return CreateCellByTypeName(typeof(Finish), typeof(FinishedBox));
                case 'W':
                    return CreateCellByTypeName(typeof(Wall));
                case 'F':
                    return CreateCellByTypeName(typeof(Finish));
                case ' ':
                    return CreateCellByTypeName(typeof(Empty));
                default:
                    throw new Exception($"wrong character for ICreature {c}");
            }
        }

        private static ICell CreateCellByTypeName(Type name, Type entityName = null)
        {
            var key = new Tuple<string, string>(name.Name, entityName == null ? null : entityName.Name);
            if (!CellFactory.ContainsKey(key))
            {
                Func<AbstractEntity> entityCreator = null;
                if (entityName != null)
                    entityCreator = () => (AbstractEntity)Activator.CreateInstance(entityName);
                CellFactory[key] = new Tuple<Func<ICell>, Func<AbstractEntity>>(() => (ICell)Activator.CreateInstance(name), entityCreator);
            }

            ICell res = CellFactory[key].Item1();
            if (CellFactory[key].Item2 != null)
                res.Entity = CellFactory[key].Item2();
            return res;
        }
    }
}