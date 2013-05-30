using UnityEngine;

public class Day
{
    private int[] solution;

    public int[] Solution
    {
        get { return solution; }
    }

    private TSPCase tspCase;

    public TSPCase TspCase
    {
        get { return tspCase; }
        set { tspCase = value; }
    }

    public Day(TSPCase tspCase)
    {
        this.tspCase = tspCase;
        solution = TSPSolver.solveCase(tspCase);
    }
}