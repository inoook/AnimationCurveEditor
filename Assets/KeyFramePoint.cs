using UnityEngine;
using System.Collections;

public enum TANGENT_MODE
{
	Free, Linear, Constant
}
public enum HANDLE_MODE
{
	Auto, FreeSmooth, Flat, Broken
}

//[ExecuteInEditMode]
public class KeyFramePoint : MonoBehaviour {

	public Transform inTrans;
	public Transform outTrans;

	public float inTangent;
	public float outTangent;

	public TANGENT_MODE inTangentMode = TANGENT_MODE.Free;
	public TANGENT_MODE outTangentMode = TANGENT_MODE.Free;

	public HANDLE_MODE handleMode = HANDLE_MODE.Broken;

	private float handleDist = 1.0f;

	// Use this for initialization
	void Awake () {
		Update();
	}
	
	// Update is called once per frame
	void Update () {
		if(handleMode == HANDLE_MODE.Flat){
			Vector3 basePos = this.transform.position;
			inTrans.position = new Vector3(basePos.x-1, basePos.y, 0);
			outTrans.position = new Vector3(basePos.x+1, basePos.y, 0);
		}else if(handleMode == HANDLE_MODE.FreeSmooth){
			inTrans.localPosition = -outTrans.localPosition;
		}

		Vector3 inNormalVec = inTrans.localPosition;
		Vector3 outNormalVec = outTrans.localPosition;
		inNormalVec.Normalize();
		outNormalVec.Normalize();

		if(inNormalVec.x < 0 && inTangentMode != TANGENT_MODE.Constant){
			inTangent = inNormalVec.y / inNormalVec.x;
		}else{
			inTangent = Mathf.Infinity;
		}
		if(outNormalVec.x > 0 && outTangentMode != TANGENT_MODE.Constant){
			outTangent = outNormalVec.y / outNormalVec.x;
		}else{
			outTangent = Mathf.Infinity;
		}

		// control position
		if(inNormalVec.x > 0){
			Vector3 tIPos = inTrans.localPosition;
			tIPos.x = 0;
			inTrans.localPosition = tIPos;
		}
		if(outNormalVec.x < 0){
			Vector3 tOPos = outTrans.localPosition;
			tOPos.x = 0;
			outTrans.localPosition = tOPos;
		}

		inTrans.localPosition = inTrans.localPosition.normalized * handleDist;
		outTrans.localPosition = outTrans.localPosition.normalized * handleDist;

		// handle visible
		if(inTangentMode == TANGENT_MODE.Constant || handleMode == HANDLE_MODE.Auto){
			inTrans.gameObject.SetActive(false);
		}else{
			inTrans.gameObject.SetActive(true);
		}
		if(outTangentMode == TANGENT_MODE.Constant || handleMode == HANDLE_MODE.Auto){
			outTrans.gameObject.SetActive(false);
		}else{
			outTrans.gameObject.SetActive(true);
		}
	}

	public float time
	{
		get{
			return  this.transform.position.x;
		}
	}
	public float v
	{
		get{
			return  this.transform.position.y;
		}
	}

	public Vector3 position
	{
		get{
			return this.transform.position;
		}
	}

	public void SetInTangent(float tangent)
	{
		float t = Mathf.Atan( tangent );
		inTrans.localPosition = -new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0) * handleDist;
	}
	public void SetOutTangent(float tangent)
	{
		float t = Mathf.Atan( tangent );
		outTrans.localPosition = new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0) * handleDist;
	}

	public Keyframe GetKeyframe()
	{
		Keyframe kf = new Keyframe(time, v, inTangent, outTangent);
		return kf;
	}
}
