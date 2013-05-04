using UnityEngine;

public class Building : MonoBehaviour
{
    private int position;
	private GameObject newSphere;
	
    public int Position
    {
        get { return position; }
        set { 
            position = value;
            if (position == 0)
            {
                renderer.material.color = Color.blue;
            }
        }
    }

    public void Awake()
    {
        if(position==0 && Level.Instance.CurrentDayNumber!=0){
			Player.Instance.truck.GetComponent<Vehicle>().StartPosition=transform.position;
		}
		renderer.material.color = Color.red;
        collider.enabled = true;
    }

    public void OnMouseDown()
    {
        if(position == 0 && !Player.Instance.CanVisitOrigin){
            return;
        }
        renderer.material.color = Color.green;
		
		newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		newSphere.transform.position = transform.position+transform.forward*-5;
		collider.enabled = false;
        Player.Instance.addNodeToSelection(position);

		//Debug.Log(transform.localScale.x);
		Player.Instance.moveTruck(transform.position);
	}
   
	
	
    public void OnDestroy() {
        collider.enabled = false;
        renderer.material.color = new Color32(238,226,181,255);
		Destroy(newSphere);
		
    }

}