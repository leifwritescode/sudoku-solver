using System.Text;
using LDK.Extensions;

namespace WaveFunctionCollapse;

internal class Board : ICloneable
{
    private const int SUM = 45;

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

    private readonly IList<Cell> _cells = [];

    public int Length { get; init; }

    public bool IsCollapsed => _cells.All(cell => cell.IsCollapsed);

    public Board(int length, int[,] values)
    {
        Length = length;

        for (var y = 0; y < length; y++)
        {
            for (var x = 0; x < length; x++)
            {
                _cells.Add(new Cell((x, y), values[y, x]));
            }
        }
    }

    private Board(int length, IEnumerable<Cell> cells)
    {
        Length = length;
        _cells = cells.Select(cell => (Cell)cell.Clone()).ToList();
    }

    /// <summary>
    /// Get the cell at the given coordinate
    /// </summary>
    /// <param name="p">The point</param>
    /// <returns>The cell</returns>
    /// <exception cref="ArgumentOutOfRangeException">p is out of range</exception>
    public Cell CellAt((int x, int y) p)
    {
        if (p.x < 0 || p.x >= Length || p.y < 0 || p.y >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(p));
        }

        return _cells.Single(cell => cell.Coordinate == p);
    }

    /// <summary>
    /// Determine if the board is solved
    /// </summary>
    /// <returns>true if solved, else false</returns>
    public bool IsSolved()
    {
        // break early if the board is not collapsed
        if (!IsCollapsed) return false;

        // check row
        for (var y = 0; y < 9; y++)
        {
            var cells = _cells.Where(cell => cell.Coordinate.y == y);
            if (cells.Distinct().Sum(x => x.Value) != SUM) return false;
        }

        // check rows
        for (var x = 0; x < 9; x++)
        {
            var cells = _cells.Where(cell => cell.Coordinate.x == x);
            if (cells.Distinct().Sum(x => x.Value) != SUM) return false;
        }

        // check squares
        // we know the rows/columns are valid, so we can just check the 3x3 blocks and return the result
        var blockSize = (int)Math.Floor(Math.Sqrt(Length));
        return s_blocks.All(block => {
            var cells = _cells.Where(cell => {
                return cell.Coordinate.x.IsInRange(block.x, block.x + blockSize + 2) &&
                       cell.Coordinate.y.IsInRange(block.y, block.y + blockSize + 2);
            });
            return cells.Distinct().Sum(x => x.Value) == SUM;
        });
    }

    public IEnumerable<Cell> AllCells => _cells;

    public IEnumerable<Cell> AllCellsAffectedByCell(Cell cell)
    {
        var row = AllCellsInRow(cell.Coordinate.y);
        var column = AllCellsInColumn(cell.Coordinate.x);
        var block = AllCellsInBlock(cell.Coordinate.x, cell.Coordinate.y);
        return row.Concat(column).Concat(block).Distinct();
    }

    private IEnumerable<Cell> AllCellsInRow(int row)
    {
        if (row < 0 || row >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        return _cells.Where(cell => cell.Coordinate.y == row);
    }

    private IEnumerable<Cell> AllCellsInColumn(int column)
    {
        if (column < 0 || column >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        return _cells.Where(cell => cell.Coordinate.x == column);
    }

    private IEnumerable<Cell> AllCellsInBlock(int column, int row)
    {
        if (column < 0 || column >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        if (row < 0 || row >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        var xStart = column - (column % 3);
        var yStart = row - (row % 3);

        return _cells.Where(cell => {
            return cell.Coordinate.x.IsInRange(xStart, xStart + 2) &&
                   cell.Coordinate.y.IsInRange(yStart, yStart + 2);
        });
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Length; y++)
        {
            var cells = _cells.Where(cell => cell.Coordinate.y == y).OrderBy(cell => cell.Coordinate.x);
            foreach (var cell in cells)
            {
                sb.Append(cell.Value ?? ' ');
            }
            sb.AppendLine();
        }

        return sb.ToString().Trim();
    }

    public object Clone()
    {
        return new Board(Length, _cells);
    }
}
