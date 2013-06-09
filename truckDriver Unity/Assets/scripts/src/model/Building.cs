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
				resetFrames();
				transform.FindChild("BlueFrame").gameObject.SetActive(true);
            }
        }
    }

    public void Awake()
    {
        if(position==0 && Level.Instance.CurrentDayNumber!=0){
			Player.Instance.truck.GetComponent<Vehicle>().StartPosition=transform.position;
		}
		//turn red
		resetFrames();
		transform.FindChild("RedFrame").gameObject.SetActive(true);
		
        collider.enabled = true;
    }

    public void OnMouseDown()
    {
        if(position == 0 && !Player.Instance.CanVisitOrigin || !Player.Instance.DoneMoving){
            return;
        }
        //turn green
		resetFrames();
		transform.FindChild("GreenFrame").gameObject.SetActive(true);
		
		collider.enabled = false;
        Player.Instance.addNodeToSelection(position);

		Player.Instance.moveTruck(transform.position);
	}
   
	
	
    public void OnDestroy() {
        collider.enabled = false;
        renderer.material.color = new Color32(238,226,181,255);
		Destroy(newSphere);
		
    }
	
	private void resetFrames(){
		foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
	}

}