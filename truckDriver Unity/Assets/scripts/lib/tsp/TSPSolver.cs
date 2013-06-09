using UnityEngine;

public class TSPSolver
{
    /*
    private static int kMax = 1000;
    private static int eMax = 10;
    private static float startTemperature = 100000.0f;
*/

    public static TSPCase generateCase(int numPoints, Vector2 spaceDimensions)
    {
        Vector2[] nodes = new Vector2[numPoints];
        for (int i = 0; i < nodes.Length; i++)
        {
            int x = Random.Range(0, (int)spaceDimensions.x);
            int y = Random.Range(0, (int)spaceDimensions.y);
            Vector2 v = new Vector2(x, y);
            for (int j = 0; j < i; j++)
            {
                if (v == nodes[j])
                {
                    x = Random.Range(0, (int)spaceDimensions.x);
                    y = Random.Range(0, (int)spaceDimensions.y);
                    v = new Vector2(x, y);
                    j = 0;
                }
            }
            nodes[i] = v;
        }
        TSPCase tspCase = new TSPCase(nodes, spaceDimensions);
        return tspCase;
    }

    public static TSPCase generateCase(int numPoints, Vector2 startingPoint, Vector2 spaceDimensions)
    {
        Vector2[] nodes = new Vector2[numPoints];
        nodes[0] = startingPoint;
        for (int i = 1; i < nodes.Length; i++)
        {
            int x = Random.Range(0, (int)spaceDimensions.x);
            int y = Random.Range(0, (int)spaceDimensions.y);
            Vector2 v = new Vector2(x, y);
            for (int j = 0; j < i; j++)
            {
                if (v == nodes[j])
                {
                    x = Random.Range(0, (int)spaceDimensions.x);
                    y = Random.Range(0, (int)spaceDimensions.y);
                    v = new Vector2(x, y);
                    j = 0;
                }
            }
            nodes[i] = v;
        }
        TSPCase tspCase = new TSPCase(nodes, spaceDimensions);
        return tspCase;
    }

    public static int[] solveCase(TSPCase problem)
    {
        int count = 2 * problem.Nodes.Length;
        float[] all_De = new float[count];
        int[] currS = randomPermutation(problem.Nodes.Length);
        float currCost = calculateCost(currS, problem);
        for (int i = 0; i < count; i++)
        {
            currS = randomPermutation(problem.Nodes.Length);
            currCost = calculateCost(currS, problem);

            int[] newS = (int[])currS.Clone();
            int[] seleccion = randomPermutation(problem.Nodes.Length);

            int inicio, fin;
            if (seleccion[0] > seleccion[1])
            {
                inicio = seleccion[1];
                fin = seleccion[0];
            }
            else
            {
                inicio = seleccion[0];
                fin = seleccion[1];
            }
            int apunt = 0;
            for (int j = inicio; j <= fin; j++)
            {
                newS[j] = currS[fin - apunt];
                apunt++;
            }
            float newCost = calculateCost(newS, problem);
            all_De[i] = Mathf.Abs(newCost - currCost);
        }
        float dE = Mathf.Max(all_De);
        float temp = 100 * dE;
        float temp_inicial = temp;
        float temp_final = 0.0001f * temp;
        int m = 500 * problem.Nodes.Length;
        float tempRatio = 0.5f;
        int[] bestS = (int[])currS.Clone();
        float bestCost = currCost;

        int cont = 0;
        while (temp > temp_final)
        {
            for (int i = 0; i < m; i++)
            {
                cont++;
                int[] newS = (int[])currS.Clone();
                int n1 = Random.Range(0, problem.Nodes.Length);
                int n2 = Random.Range(0, problem.Nodes.Length);
                while (n1 == n2)
                {
                    n2 = Random.Range(0, problem.Nodes.Length);
                }
                int inicio = (n1 > n2) ? n2 : n1;
                int final = (n1 > n2) ? n1 : n2;
                int apunt = 0;

                for (int j = inicio; j <= final; j++)
                {
                    newS[j] = currS[final - apunt];
                    apunt++;
                }

                float newCost = calculateCost(newS, problem);
                if (newCost < bestCost)
                {
                    bestS = (int[])newS.Clone();
                    bestCost = newCost;
                }

                if (newCost < currCost || Random.Range(0.0f, 1.0f) < Mathf.Exp((currCost - newCost) / temp))
                {
                    currS = (int[])newS.Clone();
                    currCost = newCost;
                }
            }
            temp *= tempRatio;
        }
        return organizeNodes((int[])bestS.Clone(), problem);
    }

	public static int[] organizeNodes (int[] solution, TSPCase problem)
	{
		int originPos = -1;
		for(int i = 0; i < solution.Length; i++){
			if(solution[i] == 0){
				originPos = i;
			}
		}
	
		int[] organizedSolution = new int[solution.Length];
		for(int j=0; j<organizedSolution.Length; j++){
			organizedSolution[j] = solution[(j+originPos) % solution.Length];
		}
		
		return organizedSolution;
	}

    // public static TSPSolution solveCaseTheirs(TSPCase problem) {
    //        int randomTestCount = 20;
    //        int[] bestState = null;
    //        float[]alldE = new float[randomTestCount];
    //        int[] currentState = null;
    //        float currentCost = 0;
    //        for (int i = 0; i < alldE.Length; i++)
    //        {
    //            currentState = randomPermutation(problem.Nodes.Length);
    //            currentCost = calculateCost(currentState, problem);
    //            int[] newState = new int[currentState.Length];
    //            for (int j = 0; j < currentState.Length; j++)
    //            {
    //                newState[j] = currentState[j];
    //            }
    //            int[] selection = randomPermutation(problem.Nodes.Length);
    //            int start = Mathf.Min(selection[0], selection[1]);
    //            int end = Mathf.Max(selection[0], selection[1]);
    ////            Debug.Log("Start"+start+" End"+end);
    //            int apunt = 0;
    //            for (int j = start; j <= end ; j++)
    //            {
    //                newState[j] = currentState[end - apunt];
    //                apunt++;
    //            }
    //            float newCost = calculateCost(newState, problem);
    //            alldE[i] = Mathf.Abs(currentCost - newCost);
    //        }

    //        float dE = Mathf.Max(alldE);
    //        float temp = 100 * dE;
    //        float finalTemperature = 0.0001f * temp;
    //        int m = 500 * problem.Nodes.Length;
    //        float tempRatio = 0.5f;
    //        float bestCost = currentCost;
    //        Debug.Log(bestCost);
    ////		Debug.Log("Annealing Main Loop");
    //        int cont = 0;
    //        while(temp > finalTemperature){
    //            for (int i = 0; i < m; i++)
    //            {
    //                cont++;
    //                int[] newState = new int[currentState.Length];
    //                for (int j = 0; j < currentState.Length; j++){
    //                    newState[j] = currentState[j];
    //                }
    //                int[] selection = randomPermutation(problem.Nodes.Length);
    //                int start = Mathf.Min(selection[1], selection[2]);
    //                int end = Mathf.Max(selection[1], selection[2]);
    //                int apunt = 0;
    //                for (int j = start; j <= end; j++)
    //                {
    //                    newState[j] = currentState[end - apunt];
    //                    apunt++;
    //                }
    //                float newCost = calculateCost(newState, problem);
    //                if(newCost < bestCost){
    //                    bestState = newState;
    //                    bestCost = newCost;
    //                }
    //                float exp = (currentCost - newCost)/temp;
    //                exp = (exp > 709)? 709:exp;
    //                if(newCost < currentCost || Random.Range(0, 1) < Mathf.Exp(exp)){
    //                    currentCost = newCost;
    //                    currentState = newState;
    //                }
    //            }
    ////			Debug.Log(temp);
    //            temp *= tempRatio;
    //        }
    //  //  	Debug.Log(cont);
    //        Debug.Log(bestCost);
    //        return new TSPSolution(problem, bestState);
    //    }

    /*
    public static TSPSolution solveCaseMine(TSPCase problem) {
        int[] s = randomPermutation(problem.Nodes.Length);
        int e = calculateCost(s, problem);
        int[] sbest = s;
        int ebest = e;
        int k = 0;
        while(k < kMax && e > eMax){
            int[] snew = twoOpNeighbourMove(s, problem);
            int enew = calculateCost(snew, problem);

            if(enew < e || Random.Range(0, 1) < Mathf.Exp(-(enew - e)/temperature(k, kMax)) ){
                s = snew;
                e = enew;
            }
            if(enew < ebest){
                sbest = snew;
                ebest = enew;
            }
            k++;
            Debug.Log(k);
        }

        return new TSPSolution(problem, sbest);
    }
     */
    /*private static float temperature(int k, int kMax)
    {
        return startTemperature * ((kMax - k) / kMax);
    }

    private static int[] closeNeighbourMove(int[] s, TSPCase problem)
    {
        int v = Random.Range(0, s.Length);
        bool leftNeighbour = Random.Range(-1, 2) < 0;

        int aux = s[v];
        if (leftNeighbour)
        {
            int ln = (v - 1 < 0) ? s.Length - 1 : v - 1;
            s[v] = s[ln];
            s[ln] = aux;
        }
        else
        {
            int rn = (v + 1 == s.Length) ? 0 : v + 1;
            s[v] = s[rn];
            s[rn] = aux;
        }
        return s;
    }

    private static int[] twoOpNeighbourMove(int[] s, TSPCase problem) {
        int n1 = Random.Range(0, s.Length);
        int n2 = Random.Range(0, s.Length);
        while(Mathf.Abs(n2-n1) <= 1 || Mathf.Abs(n2-n1)==s.Length-1){
            n2 = Random.Range(0, s.Length);
        }
        int n1Next = (n1 == s.Length - 1) ? 0 : n1 + 1;
        int n2Next = (n2 == s.Length - 1) ? 0 : n2 + 1;

        int aux;
        int[] crossed = s;
        aux = crossed[n1Next];
        crossed[n1Next] = crossed[n2Next];
        crossed[n2Next] = aux;

        int[]  peers = s;
        aux = peers[n1Next];
        peers[n1Next] = peers[n2];
        peers[n2] = aux;

        return (calculateCost(crossed, problem) < calculateCost(peers, problem)) ? crossed : peers;
    }*/

    private static int[] randomPermutation(int n)
    {
        int[] permutation = new int[n];
        bool[] visited = new bool[n];
        for (int i = 0; i < permutation.Length; i++)
        {
            int index = Random.Range(0, n);
            while (visited[index])
            {
                index = Random.Range(0, n);
            }
            permutation[index] = i;
            visited[index] = true;
        }
        return permutation;
    }

    public static float calculateCost(int[] permutation, TSPCase tspCase)
    {
        float acum = tspCase.DistanceMatrix[permutation[permutation.Length - 1], permutation[0]];
        for (int i = 0; i < permutation.Length - 1; i++)
        {
            acum += tspCase.DistanceMatrix[permutation[i], permutation[i + 1]];
        }
        return acum;
    }
}