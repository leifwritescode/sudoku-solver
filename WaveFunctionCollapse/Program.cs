using WaveFunctionCollapse.Strategies;

namespace WaveFunctionCollapse;

public static class Program
{
    private const int LENGTH = 9;
    private const int ITERATIONS = 10000;

    /// <summary>
    /// Aftenposten Lørdag 3. Mars 2023, vanskelig sudoku
    /// </summary>
    private static readonly int[,] s_testBoard = new int[LENGTH, LENGTH]
    {
        { 6,0,0,  0,0,0,  7,0,0 },
        { 0,0,0,  4,7,0,  3,0,0 },
        { 0,0,0,  0,0,0,  0,0,0 },

        { 1,9,0,  0,0,0,  0,0,0 },
        { 0,0,6,  8,0,0,  1,0,5 },
        { 0,0,7,  0,0,0,  0,0,6 },

        { 0,0,0,  0,9,0,  0,5,2 },
        { 0,5,0,  1,0,0,  0,0,0 },
        { 0,4,0,  0,0,0,  0,0,9 }
    };

    /// <summary>
    /// The solution to the default board.
    /// </summary>
    private static readonly string s_defaultSolution = """
    635981724
    219475368
    478632591
    194756283
    326849175
    587213946
    761394852
    952168437
    843527619
    """;

    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var strategy = new StochasticCollapseStrategy();
        var board = new Board(LENGTH, s_testBoard);
        var solver = new Solver(board, strategy);

        // The solver is non-deterministic, but fast.
        // We'll run it a few times to see if we can get a solution.
        for (var i = 0; i < ITERATIONS; ++i)
        {
            Console.Write($"Iteration {i + 1}... ");
            var finalBoard = solver.Solve();
            if (finalBoard.IsSolved())
            {
                Console.WriteLine("SOLVED!");
                Console.WriteLine(finalBoard);
                break;
            }
            else
            {
                Console.WriteLine("didn't yield a solution.");
                solver.Reset();
            }
        }
    }
}
