using UnityEngine;
using System.Collections;
using CurveExtended;

public class AnimationCurveTool : MonoBehaviour {

	public AnimationCurve curve;

	// Use this for initialization
	void Start () {
		Keyframe[] ks = new Keyframe[3];

		ks[0] = new Keyframe(0, 0);
		ks[0].inTangent = 0;

		ks[1] = new Keyframe(0.5f, 1.0f);
		ks[1].inTangent = 45;
		ks[1].outTangent = -45;

		ks[2] = new Keyframe(1, 0);
		ks[2].inTangent = 200;


//		ks[0] = KeyframeUtil.GetNew(0, 0, TangentMode.Linear, TangentMode.Stepped);
//		ks[1] = KeyframeUtil.GetNew(0.5f, 5.0f, TangentMode.Linear, TangentMode.Stepped);
//		ks[2] = KeyframeUtil.GetNew(1, 0, TangentMode.Linear, TangentMode.Stepped);
		
//		KeyframeUtil.SetKeyBroken(ks[1], false);
//		KeyframeUtil.SetKeyTangentMode(ks[1], 0, TangentMode.Linear);
//		KeyframeUtil.SetKeyTangentMode(ks[1], 1, TangentMode.Stepped);

		curve = new AnimationCurve(ks);
		curve.UpdateAllLinearTangents();

		Keyframe key = KeyframeUtil.GetNew(0.5f, 1.0f, TangentMode.Editable, TangentMode.Stepped);
		key.inTangent = 45;
		UpdateKey(1, key);

//		curve.UpdateAllLinearTangents();
	}

	void UpdateKey(int index, Keyframe key)
	{
		curve.MoveKey(index, key);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public float split = 100;
//
//	bool CheckKeyTime(float t0, float t1, out float t)
//	{
//		Keyframe[] keys = curve.keys;
//		for(int i = 0; i < keys.Length; i++){
//			Keyframe key = keys[i];
//			float keyTime = key.time;
//			if(keyTime >= t0 && keyTime <= t1){
//				t = key.time;
//				return true;
//			}
//		}
//		t = Mathf.NegativeInfinity;
//		return false;
//	}

	public GizmoDrawAnimCurve gizmoDrawAnimCurve;

	void OnDrawGizmos()
	{
		if(gizmoDrawAnimCurve != null){
			gizmoDrawAnimCurve.DrawGizmos(curve);
		}
//		Gizmos.DrawWireSphere(Vector3.zero, 1.0f);
//
//		Gizmos.color = Color.red;
//
//		float delta = 1.0f / split;
//		for(int i = 0; i < split; i++){
//			float t0 = delta * i;
//			float v0 = curve.Evaluate(t0);
//
//			float t1 = delta * (i+1);
//			float v1 = curve.Evaluate(t1);
//
//			float t = 0.0f;
//			if(CheckKeyTime(t0, t1, out t)){
//				t0 = t1 = t;
//			}
//
//			Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t1, v1, 0));
//		}
//
//		Keyframe[] keys = curve.keys;
//		for(int i = 0; i < keys.Length; i++){
//			Keyframe key = keys[i];
//			Vector3 pos = new Vector3(key.time, key.value, 0);
//			Gizmos.DrawWireSphere(pos, 0.05f);
//
////			Vector3 posT = new Vector3(key.time, key.inTangent, 0);
////			Gizmos.DrawWireSphere(posT, 0.05f);
//		}
//
////		Debug.Log(keys[1].inTangent + " / "+keys[1].outTangent + " / "+keys[1].tangentMode);
//
//
//		Vector3 pos1 = new Vector3(keys[1].time, keys[1].value, 0);
//		float inTan = Mathf.Atan( keys[1].inTangent );
//		Vector3 pos1_intangent = pos1 - new Vector3(Mathf.Cos(inTan), Mathf.Sin(inTan), 0);
//		Gizmos.color = Color.cyan;
//		Gizmos.DrawLine(pos1, pos1_intangent);
	}
}

