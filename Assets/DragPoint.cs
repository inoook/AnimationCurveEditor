using UnityEngine;
using System.Collections;

public class DragPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDrag()
	{
		float z = Camera.main.transform.position.z - this.transform.position.z;
		Vector3 screenPos = Input.mousePosition;
		screenPos.z = Mathf.Abs(z);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
		this.transform.position = worldPos;
		
	}
}
