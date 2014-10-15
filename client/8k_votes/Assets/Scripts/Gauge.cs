using UnityEngine;
using System.Collections;

public class Gauge : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
		var fact = this.maximumLimitScale / limit;
		var b = (limit - input) / limit + 1;
		var count = input < limit ? input : limit;
		var a = fact * count * b;
		float c = (float)input / (float)limit;
		float d = this.curve.Evaluate(c);
		f = d;
		this.transform.localScale = Vector3.Lerp(new Vector3(10f, this.lowerLimitScale , 10f), new Vector3(10f, this.maximumLimitScale , 10f), f );
	}
	public float f = 0f;
	public float g = 0f;
	public float maximumLimitScale;
	public float lowerLimitScale;
	public int limit = 4000;
	public int input;
	public AnimationCurve curve;
	public void Vote( int i = 1 ){
		input += i;
	}
	private float GetFact(){
		return input / limit;
	}
}
