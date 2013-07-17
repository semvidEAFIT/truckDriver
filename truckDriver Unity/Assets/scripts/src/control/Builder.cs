/*<<<<<<< HEAD

using UnityEngine;

public class Builder : MonoBehaviour
{
    private GameObject[,] buildings;
	public GameObject[,] Buildings {
		get {
			return this.buildings;
		}
	}
    public GameObject street;
    public GameObject building;
	public GameObject truck;
    private Vector2 distance;
    private Day loadedDay;
	private Vector3 vectorPosition ;
	private bool rate;
	private float buildingsScale=1;
	private float buildingX;
	private float buildingY;
	
	public Material[] buildingMaterials;
	
	public int streetSize=5;

    void Awake() {
		
        buildings = new GameObject[(int)LevelSettings.Instance.CityDimensions.x, (int)LevelSettings.Instance.CityDimensions.y];
        distance = new Vector2((street.transform.localScale.x) / (LevelSettings.Instance.CityDimensions.x+1), (street.transform.localScale.y) / (LevelSettings.Instance.CityDimensions.y+1));
        LevelSettings.Instance.DistanceManhatan = new Vector3(distance.x, distance.y, 0);
		float x0 = -1*LevelSettings.Instance.CityDimensions.x/2*distance.x + distance.x/2;
        float y0 = LevelSettings.Instance.CityDimensions.y / 2 * distance.y - distance.y/2;
		
		buildingX = (LevelSettings.Instance.CityDimensions.x * distance.x - streetSize*(LevelSettings.Instance.CityDimensions.x)) / LevelSettings.Instance.CityDimensions.x;
		buildingY = (LevelSettings.Instance.CityDimensions.y * distance.y - streetSize*(LevelSettings.Instance.CityDimensions.y)) / LevelSettings.Instance.CityDimensions.y;
		buildingsScale = Mathf.Min(buildingX/building.transform.localScale.x, buildingY/building.transform.localScale.y);
		
		if(buildingMaterials.Length==0) Debug.LogError("No buildings materials assigned!");
		
		rate=false;
		
        for (int i = 0; i < LevelSettings.Instance.CityDimensions.x; i++)
        {
            for (int j = 0; j < LevelSettings.Instance.CityDimensions.y; j++)
            {			
				buildings[i, j] = Instantiate(building, new Vector3(x0+i*distance.x, y0-j*distance.y, 0), Quaternion.identity) as GameObject;
				buildings[i, j].renderer.material = buildingMaterials[Random.Range(0, buildingMaterials.Length)];
				
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
				//Destroy(truck);
            }
        }
		
		for (int i = 0; i < currentDay.TspCase.Nodes.Length; i++)
        {
	        Vector2 node = currentDay.TspCase.Nodes[i];
	        Building b = buildings[(int)node.x, (int)node.y].AddComponent<Building>();
	        b.Position = i;
			if(i==0 && !rate){
				//b.transform.position.y + distance.y/2 + streetSize/2
			    vectorPosition = new Vector3(b.transform.position.x,b.transform.position.y + 9,b.transform.position.z);
				truck=Instantiate(truck,vectorPosition,truck.transform.rotation)as GameObject;
	//				truck.GetComponent<Vehicle>().widthBlock=distance.x;
	//				
	//				truck.GetComponent<Vehicle>().heigthBlock=9;
				truck.GetComponent<Vehicle>().setStretSize(streetSize);
				rate=true;
			
	    	}
		}
        
        loadedDay = currentDay;
        
    
 }
=======*/

using UnityEngine;

public class Builder : MonoBehaviour
{
    private GameObject[,] buildings;
	public GameObject[,] Buildings {
		get {
			return this.buildings;
		}
	}
    public GameObject street;
    public GameObject building;
	public GameObject truck;
    private Vector2 distance;
    private Day loadedDay;
	private Vector3 vectorPosition ;
	private bool rate;
	private float buildingsScale=1;
	private float buildingX;
	private float buildingY;
	
	public Material[] buildingMaterials;
	
	public int streetSize=5;

    void Awake() {
		
        buildings = new GameObject[(int)LevelSettings.Instance.CityDimensions.x, (int)LevelSettings.Instance.CityDimensions.y];
        distance = new Vector2((street.transform.localScale.x) / (LevelSettings.Instance.CityDimensions.x+1), (street.transform.localScale.y) / (LevelSettings.Instance.CityDimensions.y+1));
        LevelSettings.Instance.DistanceManhatan = new Vector3(distance.x, distance.y, 0);
		float x0 = -1*LevelSettings.Instance.CityDimensions.x/2*distance.x + distance.x/2;
        float y0 = LevelSettings.Instance.CityDimensions.y / 2 * distance.y - distance.y/2;
		
		buildingX = (LevelSettings.Instance.CityDimensions.x * distance.x - streetSize*(LevelSettings.Instance.CityDimensions.x)) / LevelSettings.Instance.CityDimensions.x;
		buildingY = (LevelSettings.Instance.CityDimensions.y * distance.y - streetSize*(LevelSettings.Instance.CityDimensions.y)) / LevelSettings.Instance.CityDimensions.y;
		buildingsScale = Mathf.Min(buildingX/building.transform.localScale.x, buildingY/building.transform.localScale.y);
		
		if(buildingMaterials.Length==0) Debug.LogError("No buildings materials assigned!");
		
		rate=false;
		
		int housePark = 0, park = 0, timeSinceGreen = 0;
		
        for (int i = 0; i < LevelSettings.Instance.CityDimensions.x; i++)
        {
            for (int j = 0; j < LevelSettings.Instance.CityDimensions.y; j++)
            {			
				buildings[i, j] = Instantiate(building, new Vector3(x0+i*distance.x, y0-j*distance.y, 0), Quaternion.identity) as GameObject;
				int bTexture = Random.Range(0, buildingMaterials.Length-1);
				while(((bTexture == 4 || bTexture == 5)&& housePark >= 5) || (bTexture == 12 && park >= 3) || ((bTexture == 4 || bTexture == 5 || bTexture == 12)&& timeSinceGreen < 13)){
					bTexture = Random.Range(0, buildingMaterials.Length-1);
				}
				if(bTexture == 4 || bTexture == 5){
					housePark++;	
					timeSinceGreen = 0;
				}else if(bTexture == 12){
					park++;
					timeSinceGreen = 0;
				}else{
					timeSinceGreen++;
				}
				//Debug.Log(bTexture + " " + housePark + " " + park);
				buildings[i, j].renderer.material = buildingMaterials[bTexture];
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
				//Destroy(truck);
            }
        }
		
		for (int i = 0; i < currentDay.TspCase.Nodes.Length; i++)
        {
	        Vector2 node = currentDay.TspCase.Nodes[i];
	        Building b = buildings[(int)node.x, (int)node.y].AddComponent<Building>();
	        b.Position = i;
			if(i==0 && !rate){
				//b.transform.position.y + distance.y/2 + streetSize/2
			    vectorPosition = new Vector3(b.transform.position.x,b.transform.position.y + distance.y/2,b.transform.position.z);
				truck=Instantiate(truck,vectorPosition,truck.transform.rotation)as GameObject;
				b.renderer.material = buildingMaterials[13];
	//				truck.GetComponent<Vehicle>().widthBlock=distance.x;
	//				
	//				truck.GetComponent<Vehicle>().heigthBlock=9;
				truck.GetComponent<Vehicle>().setStretSize(streetSize);
				rate=true;
			
	    	}
		}
        
        loadedDay = currentDay;
        
    
 }
//>>>>>>> 22cae25b023b481fa55cd5adf808360b6dac668d
}