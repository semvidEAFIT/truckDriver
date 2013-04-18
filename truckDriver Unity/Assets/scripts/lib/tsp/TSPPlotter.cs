using System.Collections.Generic;
using UnityEngine;

public class TSPPlotter : MonoBehaviour
{
    public GameObject nodeGameObject;
    public int nodeCount;
    public Vector2 dimensions;
    private static float y = -5.0f;
    private TSPCase currentCase;
    private int[] solution;
    private float startTime;
    private bool renderSolution = false, lineRenderersState = false;
    private List<GameObject> nodeSpheres;

    private void Awake()
    {
        nodeSpheres = new List<GameObject>();
    }

    // Update is called once per frame
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width / 8, Screen.height / 16), "New Case"))
        {
            createNewTSPCase();
        }
        if (GUI.Button(new Rect(0, Screen.height / 16, Screen.width / 8, Screen.height / 16), "Solve"))
        {
            solveCase();
        }
        renderSolution = GUI.Toggle(new Rect(Screen.width / 8, Screen.height / 16, Screen.width / 8, Screen.height / 16), renderSolution, "Render") && solution != null;
    }

    private void createNewTSPCase()
    {
        solution = null;
        currentCase = TSPSolver.generateCase(nodeCount, dimensions);
        realocateNodes();
    }

    private void cleanCurrentView()
    {
        foreach (GameObject sphere in nodeSpheres)
        {
            Destroy(sphere);
        }
        nodeSpheres.Clear();
    }

    private void realocateNodes()
    {
        cleanCurrentView();
        GameObject reference = (GameObject)Instantiate(nodeGameObject, new Vector3(0, y, 0), Quaternion.identity);
        reference.renderer.material.color = Color.red;
        foreach (Vector2 nodePosition in currentCase.Nodes)
        {
            nodeSpheres.Add((GameObject)Instantiate(nodeGameObject, new Vector3(nodePosition.x - currentCase.SpaceDimensions.x / 2, y, nodePosition.y - currentCase.SpaceDimensions.y / 2), Quaternion.identity));
        }
    }

    private void solveCase()
    {
        startTime = Time.time;
        setLineRenderers(false);
        renderSolution = lineRenderersState = false;
        solution = TSPSolver.solveCase(currentCase);
        Debug.Log("Computation Time: " + (Time.time - startTime));
    }

    private void Update()
    {
        if (renderSolution && !lineRenderersState)
        {
            setLineRenderers(true);
            lineRenderersState = true;
        }
        else
        {
            if (!renderSolution && lineRenderersState)
            {
                setLineRenderers(false);
                lineRenderersState = false;
            }
        }
    }

    private void setLineRenderers(bool state)
    {
        if (solution == null) return;
        /*foreach (int i in solution.OptimalOrder)
        {
            Debug.Log(i);
        }*/
        LineRenderer lineRenderer = nodeSpheres[solution[0]].gameObject.GetComponent<LineRenderer>();
        if (state)
        {
            lineRenderer.enabled = true;
            lineRenderer.material.color = Color.red;
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, nodeSpheres[solution[0]].transform.position);
            lineRenderer.SetPosition(1, nodeSpheres[solution[solution.Length - 1]].transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
            lineRenderer.SetVertexCount(0);
        }

        for (int i = 1; i < solution.Length; i++) //nodeSpheres[0] is the reference
        {
            lineRenderer = nodeSpheres[solution[i]].gameObject.GetComponent<LineRenderer>();
            if (state)
            {
                lineRenderer.enabled = true;
                lineRenderer.material.color = Color.red;
                lineRenderer.SetVertexCount(2);
                lineRenderer.SetPosition(0, nodeSpheres[solution[i]].transform.position);
                lineRenderer.SetPosition(1, nodeSpheres[solution[i - 1]].transform.position);
            }
            else
            {
                lineRenderer.enabled = false;
                lineRenderer.SetVertexCount(0);
            }
        }
    }
}