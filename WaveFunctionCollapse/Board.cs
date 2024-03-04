using System.Text;

namespace WaveFunctionCollapse;

internal class Board : ICloneable
{
    private static readonly (int x, int y)[] s_blocks =
    [
        // 9x9 grid
        // 0,0 top left, 8,8 bottom right
        (0, 0),
        (3, 0),
        (6, 0),
        (0, 3),
        (3, 3),
        (6, 3),
        (0, 6),
        (3, 6),
        (6, 6)
    ];

    private Cell[,] _cells;

    public int Length { get; init; }

    public Board(int length, int[,] values)
    {
        Length = length;

        _cells = new Cell[Length, Length];
        for (var y = 0; y < length; y++)
        {
            for (var x = 0; x < length; x++)
            {
                _cells[y, x] = new Cell(values[y, x]);
            }
        }
    }

    /// <summary>
    /// Get the cell at the given coordinate
    /// </summary>
    /// <param name="p">The point</param>
    /// <returns>The cell</returns>
    /// <exception cref="ArgumentOutOfRangeException">p is out of range</exception>
    public Cell GetCell((int x, int y) p)
    {
        if (p.x < 0 || p.x >= Length || p.y < 0 || p.y >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(p));
        }

        return _cells[p.y, p.x];
    }

    /// <summary>
    /// Determine if the board is solved
    /// </summary>
    /// <returns>true if solved, else false</returns>
    public bool IsSolved()
    {
        var zeroToEight = Enumerable.Range(0, 9).ToArray();

        // check row
        for (var y = 0; y < 9; y++)
        {
            var cells = zeroToEight.Select(x => _cells[y, x]);
            if (cells.Distinct().Sum(x => x.Value) != 45) return false;
        }

        // check rows
        for (var x = 0; x < 9; x++)
        {
            var cells = zeroToEight.Select(y => _cells[y, x]);
            if (cells.Distinct().Sum(x => x.Value) != 45) return false;
        }

        // check squares
        var blockSize = (int)Math.Floor(Math.Sqrt(Length));
        foreach (var (x, y) in s_blocks)
        {
            // This is slow, but I like the LinQ syntax
            var rangeInX = Enumerable.Range(x, blockSize);
            var cells = rangeInX.SelectMany(x => {
                var rangeInY = Enumerable.Range(y, blockSize);
                return rangeInY.Select(y => _cells[y, x]);
            });

            if (cells.Distinct().Sum(x => x.Value) != 45) return false;
        }

        return true;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < 9; y++)
        {
            for (var x = 0; x < 9; x++)
            {
                sb.Append(_cells[y, x]);
            }
            sb.AppendLine();
        }

        return sb.ToString().Trim();
    }

    public object Clone()
    {
        var cells = new int[Length, Length];
        for (var y = 0; y < 9; y++)
        {
            for (var x = 0; x < 9; x++)
            {
                cells[y, x] = _cells[y, x].Value ?? 0;
            }
        }

        return new Board(Length, cells);
    }
}
