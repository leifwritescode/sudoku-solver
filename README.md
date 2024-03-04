# Wave Function Collapse

### TL;DR

A sudoku solving application implementing the wave function collapse algorithm, inspired by quantum mechanics.

### Not TL;DR

Wave function collapse is a concept in quantum mechanics. It is the mechanism in which a system, by interacting with its environment, is transformed from a superposition of states into a definite state with a well-defined value.[^1]

If we consider the sudoku grid, each cell either has no value or a pre-defined value. Those cells with no values can be considered to be every possible value until their actual value is determined by a process of elimination. In this way, it could be considered that each cell is a quantum superposition of states until it is observed.

As a result, it is possible to apply the ideas of wave function collapse to sudoku.

In _computing_, wave function collapse is nominally an algorithm that teaches your computer how to generate images. An archetypal source image is used to procedurally generate outputs that are similar to it. Under the hood, the algorithm is determining the rules that govern the source image, and producing an output image governed by the same rules.

Sudoku follows a very straightforward ruleset: Given a grid of cells, 9x9, organised into 3x3 blocks, each row, column, and block must contain all the numbers 1 to 9 exactly once.

It is possible, then, using this ruleset, as well as the initial set of states and superpositions, to determine the final state of a sudoku board. We can either do this non-deterministically, by picking a random cell to collapse during each iteration; or deterministically, by picking the cell with the lowest entropy.[^2] By iteratively collapsing cells, and propagating the effect of each collapse, we can apply the concept of wave function collapse to solve sudoku automatically.


[^1]: https://www.southampton.ac.uk/~doug/quantum_physics/collapse.pdf

[^2]: The entropy of a cell is the number of possible values it could contain, lower is better.