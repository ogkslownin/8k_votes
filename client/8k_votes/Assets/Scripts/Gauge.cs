using UnityEngine;
using System.Collections;

public class Gauge : MonoBehaviour {

	Vector3 defaultScale;
	// Use this for initialization
	void Start () {
		this.defaultScale = this.transform.localScale;
	}
	// Update is called once per frame
	void Update () {
		var fact = this.maximumLimitScale / limit;
		var b = (limit - input) / limit + 1;
		var count = input < limit ? input : limit;
		var a = fact * count * b;
		float c = (float)input / (float)limit;
		float d = StaticField.instance.curve.Evaluate(c);
		f = d;
		f += g;
		this.transform.localScale = Vector3.Lerp(new Vector3(this.defaultScale.x, this.lowerLimitScale , this.defaultScale.z), new Vector3(this.defaultScale.x, this.maximumLimitScale , this.defaultScale.y), f );
	}
	public float f = 0f;
	public float g = 0f;
	public float maximumLimitScale;
	public float lowerLimitScale;
	public int limit = 4000;
	public int input;
	public void Vote( int i = 1 ){
		input += i;
	}
	private float GetFact(){
		return input / limit;
	}
}
