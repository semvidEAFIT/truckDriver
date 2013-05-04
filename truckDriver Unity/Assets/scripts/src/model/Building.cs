using UnityEngine;

public class Building : MonoBehaviour
{
    private int position;

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
        renderer.material.color = Color.red;
        collider.enabled = true;
    }

    public void OnMouseDown()
    {
        if(position == 0 && !Player.Instance.CanVisitOrigin){
            return;
        }
        renderer.material.color = Color.green;
        collider.enabled = false;
        Player.Instance.addNodeToSelection(position);
		//Debug.Log(transform.localScale.x);
		Player.Instance.moveTruck(transform.position);
		
	 }

    public void OnDestroy() {
        collider.enabled = false;
        renderer.material.color = new Color32(238,226,181,255);
    }

}