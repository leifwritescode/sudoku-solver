using LDK.Collections;

namespace WaveFunctionCollapse;

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
            _collapseStrategy.CollapseCell(_board);

            // Then, we need to propagate the collapsed cells again
            PropagateCollapsedCells();

            // If the board has been collapsed, return it
            // Otherwise, go around again until either:
            // 1. The board is collapsed, or
            // 2. We've run out of iterations
            if (_board.IsCollapsed)
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
        // prepare a queue of all cells
        var queue = new Queue<(int x, int y)>();
        var allCells = _board.AllCells;
        foreach (var cell in allCells)
        {
            queue.Enqueue(cell.Coordinate);
        }

        // each time a cell collapses, affected cells are enqueue to see if they also collapse
        // process the queue until no more cells are affected by this "ripple effect"
        while (queue.TryDequeue(out var coordinate))
        {
            // get the cell
            var cell = _board.CellAt(coordinate);

            // if the cell is collapsed, we don't need to do anything
            if (cell.IsCollapsed) continue;

            // otherwise, find all cells affected by this cell, and eliminate the values of the collapsed cells
            var allAffectedCells = _board.AllCellsAffectedByCell(cell);
            var values = allAffectedCells.Where(c => c.IsCollapsed).Select(c => c.Value!.Value);
            //Console.WriteLine($"Affected cells: {allAffectedCells.Count()}, collapsed cells: {values.Count()}");
            foreach (var v in values)
            {
                cell.Eliminate(v);
            }

            // if the cell should collapse during this process, enqueue all affected cells to see if they also collapse
            if (cell.IsCollapsed)
            {
                foreach (var affectedCell in allAffectedCells)
                {
                    queue.Enqueue(affectedCell.Coordinate);
                }
            }
        }
    }
}
