namespace WaveFunctionCollapse.Strategies;

/// <summary>
/// Defines a strategy for collapsing a cell on a board, deterministically, by selecting the first collapsible cell.
/// </summary>
internal class FirstCellCollapseStrategy : ICollapseStrategy
{
    public int Iterations => 7;

    public void CollapseCell(Board board)
    {
        for (var y = 0; y < board.Length; ++y)
        {
            for (var x = 0; x < board.Length; ++x)
            {
                var candidate = board.CellAt((x, y));
                if (candidate.IsCollapsed) continue;

                candidate.Collapse();
                return;
            }
        }
    }
}
