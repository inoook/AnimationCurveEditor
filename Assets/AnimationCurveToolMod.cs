using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CurveExtended;

public class AnimationCurveToolMod : MonoBehaviour {

	public AnimationCurve curve;

	public List<KeyFramePoint> keyFramePoints;

	public KeyFramePoint addKeyFramePoint;//debug

	// Use this for initialization
	void Start () {
		Keyframe[] ks = new Keyframe[keyFramePoints.Count];

		for(int i = 0; i < ks.Length; i++){
			KeyFramePoint kp = keyFramePoints[i];
			float t = kp.time;
			float v = kp.v;
			float inT = kp.inTangent;
			float outT = kp.outTangent;
			ks[i] = new Keyframe(t, v, inT, outT);
		}
		
		curve = new AnimationCurve(ks);
		curve.UpdateAllLinearTangents();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAllKeyframePoints();
	}

	void UpdateAllKeyframePoints()
	{
		for(int i = 0; i < keyFramePoints.Count; i++){
			UpdateKeyframePoint(i, keyFramePoints[i]);
		}
	}

	void UpdateKeyframePoint(int i, KeyFramePoint kp)
	{
		float t = kp.time;
		float v = kp.v;
		float inT = kp.inTangent;
		float outT = kp.outTangent;

		// tangentMode
		if(i > 0 && kp.inTangentMode == TANGENT_MODE.Linear){
			KeyFramePoint preKp = keyFramePoints[i-1];
			Vector3 inDir = preKp.position - kp.position;
			inDir.Normalize();
			inT = inDir.y / inDir.x;
		}
		if(i < keyFramePoints.Count-1 && kp.outTangentMode == TANGENT_MODE.Linear){
			KeyFramePoint nextKp = keyFramePoints[i+1];
			Vector3 outDir = nextKp.position - kp.position;
			outDir.Normalize();
			outT = outDir.y / outDir.x;
		}

		// update
		Keyframe key = new Keyframe(t, v, inT, outT);
		UpdateKey(i, key);

		// handleMode
		if( kp.handleMode == HANDLE_MODE.Auto ){
			curve.SmoothTangents(i, 1.0f);
			Keyframe indexKey = curve.keys[i];
			kp.SetInTangent(indexKey.inTangent);
			kp.SetOutTangent(indexKey.outTangent);
		}
	}

	void UpdateKey(int index, Keyframe key)
	{
		curve.MoveKey(index, key);
	}


	public GizmoDrawAnimCurve gizmoDrawAnimCurve;

	void OnDrawGizmos()
	{
		if(gizmoDrawAnimCurve != null){
			gizmoDrawAnimCurve.DrawGizmos(curve);
		}
	}

	
	public void RemoveKeyAt(int index)
	{
		keyFramePoints.RemoveAt(index);
		curve.RemoveKey(index);
		Debug.Log(curve.keys.Length);
	}
	public void AddKeyAt()
	{
		KeyFramePoint kf = addKeyFramePoint;

		int i = curve.AddKey( kf.GetKeyframe() );
		keyFramePoints.Insert(i, kf);
		Debug.Log(i + " / "+curve.keys.Length);
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10,10,200,200));
		if(GUILayout.Button("RemoveAt 1")){
			RemoveKeyAt(1);
		}
		if(GUILayout.Button("AddKeyAt")){
			AddKeyAt();
		}
		GUILayout.EndArea();
	}
}

