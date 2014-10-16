using UnityEngine;
using System.Collections;

public class StaticField : MonoBehaviour {

	public static StaticField instance;

	void Awake(){
		instance = this;
	}

	public AnimationCurve curve;

}
