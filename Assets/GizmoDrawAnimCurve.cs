using UnityEngine;
using System.Collections;

public class GizmoDrawAnimCurve : MonoBehaviour {

	private AnimationCurve curve;
	public float split = 100;

	public Color lineColor = Color.red;
	public Color pointColor = Color.yellow;
	public Color tangentColor = Color.cyan;


	bool CheckKeyTime(float t0, float t1, out float t, out float v, out Keyframe? out_key)
	{
		Keyframe[] keys = curve.keys;
		for(int i = 0; i < keys.Length; i++){
			Keyframe key = keys[i];
			float keyTime = key.time;
			if(keyTime >= t0 && keyTime <= t1){
				t = key.time;
				v = key.value;
				out_key = key;
				return true;
			}
		}
		t = -1;
		v = -1;
		out_key = null;
		return false;
	}

	public void DrawGizmos(AnimationCurve curve_)
	{
		this.curve = curve_;

		Gizmos.color = Color.red;
		
		// drawLne

		float endTime = curve.keys[curve.keys.Length-1].time;
		float delta = (1.0f / split) * endTime;
		for(int i = 0; i < split; i++){
			float t0 = delta * i;
			float v0 = curve.Evaluate(t0);
			
			float t1 = delta * (i+1);
			float v1 = curve.Evaluate(t1);
			
			Gizmos.color = (i % 2 == 0) ? lineColor : Color.white;

			float t = 0.0f;
			float v = 0.0f;
			Keyframe? out_key = null;
			if(CheckKeyTime(t0, t1, out t, out v, out out_key)){
				if( IsConstainDraw((Keyframe)out_key) ){
					Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t, v0, 0));
					t0 = t1 = t;
					Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t1, v1, 0));
				}else{
					Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t, v, 0));
					Gizmos.DrawLine(new Vector3(t, v, 0), new Vector3(t1, v1, 0));
				}
			}else{
				Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t1, v1, 0));
			}

//			Gizmos.DrawLine(new Vector3(t0, v0, 0), new Vector3(t1, v1, 0));
		}
		
		// protPoint
		Gizmos.color = pointColor;

		Keyframe[] keys = curve.keys;
		for(int i = 0; i < keys.Length; i++){
			Keyframe key = keys[i];
			Vector3 pos = new Vector3(key.time, key.value, 0);
			Gizmos.DrawWireSphere(pos, 0.05f);
		}
//		Debug.Log(keys[1].inTangent + " / "+keys[1].outTangent + " / "+keys[1].tangentMode);
		
		//  tangent
		Gizmos.color = tangentColor;

		for(int i = 0; i < keys.Length; i++){
			Keyframe key = keys[i];
			Vector3 pos = new Vector3(key.time, key.value, 0);

			float inTan = Mathf.Atan( key.inTangent );
			Vector3 pos_inTangent = pos - new Vector3(Mathf.Cos(inTan), Mathf.Sin(inTan), 0);
			
			float outTan = Mathf.Atan( key.outTangent );
			Vector3 pos_outTangent = pos + new Vector3(Mathf.Cos(outTan), Mathf.Sin(outTan), 0);

			Gizmos.DrawLine(pos, pos_inTangent);
			Gizmos.DrawLine(pos, pos_outTangent);
		}
	}

	bool IsConstainDraw(Keyframe key)
	{
		bool isConstain = IsConstain(key);
		if(!isConstain){
			int keyIndex = System.Array.IndexOf(curve.keys, key);

			// check preKey
			int preKeyIndex = keyIndex - 1;
			preKeyIndex = Mathf.Clamp(preKeyIndex, 0, curve.keys.Length-1);
			Keyframe preKey = curve.keys[preKeyIndex];
			isConstain = IsConstain(preKey);
		}
		return isConstain;
	}
	bool IsConstain(Keyframe key)
	{
		return (key.inTangent == Mathf.Infinity || key.outTangent == Mathf.Infinity);
	}
}
