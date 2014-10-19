using UnityEngine;
using System;
using System.Collections.Generic;

public class Inputer : MonoBehaviour {

	public KeyCode key1;
	public KeyCode key2;
	public KeyCode key3;
	public KeyCode key4;
	public KeyCode key5;

	private List<KeyCode> keys;
	public GaugeInterface target;
	void Start(){
		this.keys = new List<KeyCode>();
		this.keys.Add(this.key1);
		this.keys.Add(this.key2);
		this.keys.Add(this.key3);
		this.keys.Add(this.key4);
		this.keys.Add(this.key5);
		this.target = GameObject.Find("GaugeInterface").GetComponent<GaugeInterface>();
	}
	void Update(){
		if( Util.instance.isDebugMode ) return;
		for( var i = 0; i < 5 ; i ++ ){
			if(Input.GetKeyDown(keys[i]) && LogIO.instance.isFileOpen == false ){
				this.target.Addition(i);
				this.Logging(i);
			}
		}
	}
	void Logging( int i ){
		var log = DateTime.Now.ToString() + ",InputKey," + this.keys[i].ToString() + "\r\n" ;
		LogIO.Log( log );
# if UNITY_EDITOR
		Debug.Log( log );
# endif
	}
	void Logging( LogIO.LogEvent _event ){
		var log = DateTime.Now.ToString() + "," + _event.ToString() + ",0" + "\r\n";
		LogIO.Log( log );
# if UNITY_EDITOR
		Debug.Log( log );
# endif
	}
}
