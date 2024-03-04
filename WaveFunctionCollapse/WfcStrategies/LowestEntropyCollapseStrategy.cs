namespace WaveFunctionCollapse.Strategies;

/// <summary>
/// Defines a strategy for collapsing a cell on a board, deterministically, by selecting the cell with the lowest entropy.
/// </summary>
internal class LowestEntropyCollapseStrategy : ICollapseStrategy
{
    public int Iterations => 10;

    public void CollapseCell(Board board)
    {
        var cell = new Cell(); // dummy cell with maximum entropy
        for (var y = 0; y < board.Length; ++y)
        {
            for (var x = 0; x < board.Length; ++x)
            {
                var candidate = board.GetCell((x, y));
                if (candidate.IsCollapsed) continue;

                if (candidate.Entropy < cell.Entropy)
                {
                    cell = candidate;
                }
            }
        }

        cell.Collapse();
    }
}
