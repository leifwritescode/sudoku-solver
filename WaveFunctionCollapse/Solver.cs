using WaveFunctionCollapse;

internal class Solver
{
    private Board _board;

    private readonly Board _snapshot;

    private readonly ICollapseStrategy _collapseStrategy;

    public Solver(Board board, ICollapseStrategy collapseStrategy)
    {
        _board = board;
        _snapshot = (Board)board.Clone();
        _collapseStrategy = collapseStrategy;

        // This is cheating
        // During solver construction, propagate the initial board state
        PropagateCollapsedCells();
    }

    public Board Solve()
    {
        // The absolute minimum iterations we can run to solve a board is 5, but the optimal seems to be around 9
        // Discovered by trial and error
        for (var i = 0; i < _collapseStrategy.Iterations; ++i)
        {
            // For each iteration, collapse a cell
            // Note that the cell we choose might already be collapsed
            // Internally, the board uses a strategy to determine which cell to collapse
            _board.CollapseCell(_collapseStrategy);

            // Then, we need to propagate the collapsed cells again
            PropagateCollapsedCells();

            // Lastly, compute if the board has been fully collapsed
            var xRange = Enumerable.Range(0, _board.Length);
            var collapsed = xRange.All(x => {
                var yRange = Enumerable.Range(0, _board.Length);
                return yRange.All(y => !_board.GetCell((x, y)).IsCollapsed);
            });

            // If the board has been collapsed, return it
            // Otherwise, go around again until either:
            // 1. The board is collapsed, or
            // 2. We've run out of iterations
            if (collapsed)
            {
                return _board;
            }
        }

        return _board;
    }

    public void Reset()
    {
        _board = (Board)_snapshot.Clone();

        // This is cheating
        // During solver construction, propagate the initial board state
        PropagateCollapsedCells();
    }

    private void PropagateCollapsedCells()
    {
        // updating possible values requires that we take the (x, y) coordinate
        // and update the values on the block, row, and column that contain that cell
        // if any of those cells collapse as a result, we run the routine recursively on _that_ cell
        // and so on and so forth until we unwind to the original cell
        for (var y = 0; y < _board.Length; ++y)
        {
            for (var x = 0; x < _board.Length; ++x)
            {
                var cell = _board.GetCell((x, y));
                if (cell.IsCollapsed)
                {
                    continue;
                }

                // iterate over the rows
                // eliminating collapsed values as we go
                for (var r = 0; r < _board.Length; ++r)
                {
                    var cellInRow = _board.GetCell((x, r));
                    if (cellInRow.IsCollapsed)
                    {
                        cell.Eliminate(cellInRow.Value!.Value);
                    }
                }

                // iterate over the columns...
                for (var c = 0; c < _board.Length; ++c)
                {
                    var cellInColumn = _board.GetCell((c, y));
                    if (cellInColumn.IsCollapsed)
                    {
                        cell.Eliminate(cellInColumn.Value!.Value);
                    }
                }

                // and the block
                var block = GetBlockFromCoordinate((x, y));
                foreach (var (bx, by) in block)
                {
                    var cellInBlock = _board.GetCell((bx, by));
                    if (cellInBlock.IsCollapsed)
                    {
                        cell.Eliminate(cellInBlock.Value!.Value);
                    }
                }

                // if this cell collapsed during the elimination process
                // we need to propagate the changes
                if (cell.IsCollapsed)
                {
                    PropagateCollapsedCells();
                }
            }
        }
    }

    private static IEnumerable<(int x, int y)> GetBlockFromCoordinate((int x, int y) coordinate)
    {
        var (x, y) = coordinate;
        var xStart = x - (x % 3);
        var yStart = y - (y % 3);
        var xRange = Enumerable.Range(xStart, 3);
        return xRange.SelectMany(x => {
            var yRange = Enumerable.Range(yStart, 3);
            return yRange.Select(y => (x, y));
        });
    }
}
