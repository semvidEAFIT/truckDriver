using UnityEngine;

public class Builder : MonoBehaviour
{
    private GameObject[,] buildings;
    public GameObject street;
    public GameObject building;
    private Vector2 distance;
    private Day loadedDay;
	
	
	private float buildingsScale=1;
	private float buildingX;
	private float buildingY;
	
	public int streetSize=5;

    void Awake() {
		
        buildings = new GameObject[(int)LevelSettings.Instance.CityDimensions.x, (int)LevelSettings.Instance.CityDimensions.y];
        distance = new Vector2((street.transform.localScale.x) / (LevelSettings.Instance.CityDimensions.x+1), (street.transform.localScale.y) / (LevelSettings.Instance.CityDimensions.y+1));
        float x0 = -1*LevelSettings.Instance.CityDimensions.x/2*distance.x + distance.x/2;
        float y0 = LevelSettings.Instance.CityDimensions.y / 2 * distance.y - distance.y/2;
		
		buildingX = (LevelSettings.Instance.CityDimensions.x * distance.x - streetSize*(LevelSettings.Instance.CityDimensions.x)) / LevelSettings.Instance.CityDimensions.x;
		buildingY = (LevelSettings.Instance.CityDimensions.y * distance.y - streetSize*(LevelSettings.Instance.CityDimensions.y)) / LevelSettings.Instance.CityDimensions.y;
		buildingsScale = Mathf.Min(buildingX/building.transform.localScale.x, buildingY/building.transform.localScale.y);
		
		Debug.Log("scale " + buildingsScale);
		Debug.Log("x " + buildingX);
		Debug.Log("y " + buildingY);
		Debug.Log((int)LevelSettings.Instance.CityDimensions.x);
		
        for (int i = 0; i < LevelSettings.Instance.CityDimensions.x; i++)
        {
            for (int j = 0; j < LevelSettings.Instance.CityDimensions.y; j++)
            {			
				buildings[i, j] = Instantiate(building, new Vector3(x0+i*distance.x, y0-j*distance.y, 0), Quaternion.identity) as GameObject;
				
				buildings[i,j].transform.localScale *= buildingsScale;
            }
        }
    }

    public void buildCity(Day currentDay)
    {
        if(loadedDay != null){
            for (int i = 0; i < loadedDay.TspCase.Nodes.Length; i++)
            {
                Vector2 node = loadedDay.TspCase.Nodes[i];
                GameObject b = buildings[(int)node.x, (int)node.y];
                DestroyImmediate(b.GetComponent<Building>());
            }
        }
        for (int i = 0; i < currentDay.TspCase.Nodes.Length; i++)
        {
            Vector2 node = currentDay.TspCase.Nodes[i];
            Building b = buildings[(int)node.x, (int)node.y].AddComponent<Building>();
            b.Position = i;
        }
        loadedDay = currentDay;
    }
}