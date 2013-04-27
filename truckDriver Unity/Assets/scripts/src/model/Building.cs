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
        renderer.material.color = Color.green;
        collider.enabled = false;
        Player.Instance.addNodeToSelection(position);
		//Debug.Log(transform.localScale.x);
		Player.Instance.moveTruck(transform.position,transform.localScale.x,transform.localScale.y);
		
	 }

    public void OnDestroy() {
        collider.enabled = false;
        renderer.material.color = Color.gray;
    }

}