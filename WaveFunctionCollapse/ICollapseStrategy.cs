namespace WaveFunctionCollapse;

/// <summary>
/// Describes a strategy for collapsing a cell on a board.
/// </summary>
internal interface ICollapseStrategy
{
    /// <summary>
    /// The number of iterations that the wave function collapse algorithm should run.
    /// </summary>
    int Iterations { get; }

    /// <summary>
    /// Collapse a cell on the board.
    /// </summary>
    /// <param name="board">The board</param>
    void CollapseCell(Board board);
}
