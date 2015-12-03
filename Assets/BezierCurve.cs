using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour {

	[System.Serializable]
	public class BezierPoint{
		public Vector3 position;
		public Vector3 handle1;
		public Vector3 handle2;
	}

	public LTBezierPath path;
	public List<BezierPoint> points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int split = 30;
	public Color drawColor = Color.red;

	public float x = 0;
	
	void OnDrawGizmos () {

		List<Vector3> nodes = new List<Vector3>();
		for(int i = 0; i < points.Count-1; i++){
			BezierPoint start = points[i];
			BezierPoint end = points[i+1];
			nodes.Add(start.position);
			nodes.Add(end.handle1);
			nodes.Add(start.handle2);
			nodes.Add(end.position);
		}
		path = new LTBezierPath(nodes.ToArray());

		float delta = 1.0f / split;
		for(int i = 0; i < split-1; i++){
			float t0 = delta * i;
			float t1 = delta * (i+1);
			Vector3 pos0 = path.point(t0);
			Vector3 pos1 = path.point(t1);
			
			Gizmos.color = (i % 2 == 0) ? drawColor : Color.clear;
			Gizmos.DrawLine(pos0, pos1);
		}

		float protSize = 0.05f;
		for(int i = 0; i < points.Count; i++){
			BezierPoint p = points[i];
			Gizmos.color = drawColor;
			Gizmos.DrawWireSphere( p.position, protSize);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere( p.handle1, protSize);
			Gizmos.DrawWireSphere( p.handle2, protSize);
		}

		//
		Vector3 v = GetEvaluate(x);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere( v, 0.1f);
	}

	public Vector3 GetEvaluate(float x, int split = 100)
	{
		x = Mathf.Clamp01(x);
		if(x == 0.0f || x == 1.0f){
			return path.point(x);
		}

		Vector3 prot0 = new Vector3(x, 0, 0);
		Vector3 prot1 = new Vector3(x, 1, 0);
		float delta = 1.0f / split;
		for(int i = 0; i < split; i++){
			float t0 = delta * i;
			float t1 = delta * (i+1);
			Vector3 pos0 = path.point(t0);
			Vector3 pos1 = path.point(t1);
			
			Vector3 crossPt = getClossPoint(prot0, pos0, prot1, pos1);
			if(crossPt.x >= pos0.x && crossPt.x <= pos1.x){
				return crossPt;
			}
		}
		Debug.Log("Error");
		return Vector3.zero;
	}
	
	public float length
	{
		get{
			if(path != null){
				return path.length;
			}else{
				return 0;
			}
		}
	}
	public Vector3 GetPointAt(float p)
	{
		if(path != null){
			return path.point(p);
		}else{
			return Vector3.zero;
		}
	}

	//
	Vector3 getClossPoint(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4){
		float S1 = ((P4.x-P2.x)*(P1.y-P2.y)-(P4.y-P2.y)*(P1.x-P2.x))*0.5f;
		float S2 = ((P4.x-P2.x)*(P2.y-P3.y)-(P4.y-P2.y)*(P2.x-P3.x))*0.5f;
		Vector3 crossPoint = new Vector3(0,0,0);
		crossPoint.x = P1.x + (P3.x-P1.x)*(S1/(S1 + S2));
		crossPoint.y = P1.y + (P3.y-P1.y)*(S1/(S1 + S2));
		
		return crossPoint;
	}
}
