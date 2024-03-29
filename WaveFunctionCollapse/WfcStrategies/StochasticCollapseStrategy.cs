namespace WaveFunctionCollapse.Strategies;

/// <summary>
/// Defines a strategy for collapsing a cell on a board, non-deterministically, by selecting a random cell.
/// </summary>
internal class StochasticCollapseStrategy : ICollapseStrategy
{
    public int Iterations => 7;

    public void CollapseCell(Board board)
    {
        var rx = Random.Shared.Next(0, board.Length);
        var ry = Random.Shared.Next(0, board.Length);
        var cell = board.GetCell((rx, ry));
        cell.Collapse();
    }
}
