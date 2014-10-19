using UnityEngine;
using System;
using System.Collections.Generic;

public class Util : MonoBehaviour {

	public static Util instance;

	Color normalColor = Color.black;
	Color successColor = new Color( 0f, 0f, 0.5f, 1f );
	Color errorColor = new Color( 0.5f, 0f, 0f, 1f );
	Color debugColor = new Color( 0f, 0.25f, 0f, 1f );
	Color changedColor = Color.white;
	const string debugStartingString = "Debug Mode";

	private TextMesh console;
	[SerializeField]private Color fromColor;
	[SerializeField]private Color modeColor;
	[SerializeField]private float colorChangeProgress = 0f;
	public bool isInputtable{ private set; get; }
	public bool isDebugMode{ private set; get; }
	public float colorChangeSpeed = 1f;
	public int inputTarget;
	string inputTargetString;

	GaugeInterface gaugeInterface;
	void Awake(){
		Screen.SetResolution(1920, 1080, true, 30);
		if(instance == null){
			instance = this;
		}else{
			Debug.LogError("Util is exists at " + this.gameObject.name );
			Destroy(this);
		}
		this.gaugeInterface = GetComponent<GaugeInterface>();
	}

	void Start(){
		this.fromColor = normalColor;
		this.modeColor = normalColor;
		this.console = this.console ?? GameObject.Find("MainCamera/DebugScreen").GetComponent<TextMesh>();
		this.console.text = "";
		this.isDebugMode = false;
		this.isInputtable = false;
	}

	List<string> keyInput = new List<string>(){"a","s","d","f","g"};
	void Update(){
		if( Input.GetKeyDown( KeyCode.Return ) ){
			ChangeDebugMode();
		}

		// bgcolor;
		var clr = Color.Lerp(this.fromColor, this.modeColor, this.colorChangeProgress );
		this.colorChangeProgress += Time.deltaTime * this.colorChangeSpeed;
		Camera.main.backgroundColor = clr;

		if( ! this.isDebugMode ) return;

		// key check inputMode
		foreach( var k in keyInput ){
			if( Input.GetKeyDown(k) ){
				Debug.Log("Key down " + k);
				this.inputTarget = keyInput.IndexOf(k);
				this.inputTargetString = k;
				break;
			}
			if( Input.GetKeyUp(k) ){
				Debug.Log("Key up " + k);
				this.inputTargetString = "";

				if( this.console.text != "" && this.console.text != debugStartingString && inputTarget >= 0 ){
					try{
						var inputCount = System.Convert.ToInt32( this.console.text );
						this.gaugeInterface.ForceAddion(inputTarget, inputCount);
						this.fromColor = successColor;
						this.colorChangeProgress = 0f;
						LogIO.Log( DateTime.Now.ToString() + ",ForceInput," + k + "," + this.console.text + "\r\n" );
					}catch(System.Exception e){
						// 桁数大杉とかでやれない場合。
						LogIO.ErrorLog( e.ToString() );
						this.fromColor = errorColor;
						this.colorChangeProgress = 0f;
					}
				}
				this.console.text = "";
				inputTarget = -1;
			}
		}

		CheckKeyInput();
	}

	void ChangeDebugMode(){
		this.isDebugMode = this.isDebugMode == true ? false : true ;
		if( this.isDebugMode ){
			// デバッグモードになった時の表現
			this.fromColor = changedColor;
			this.modeColor = debugColor;
			this.colorChangeProgress = 0f;
			Camera.main.backgroundColor = this.fromColor;
			this.isInputtable = true;
			this.console.text = debugStartingString;
		}else{
			// ノーマルモードになった時の表現
			this.fromColor = changedColor;
			this.modeColor = normalColor;
			this.colorChangeProgress = 0f;
			Camera.main.backgroundColor = this.fromColor;
			this.console.text = "";
			this.isInputtable = false;
		}
	}
	void CheckKeyInput(){
		if( this.isInputtable )
		for(int i = 0; i < 10 ; i++ ){
			var iString = i.ToString();
			if( Input.GetKeyDown( iString ) ){
				if( this.console.text == debugStartingString ) this.console.text = "";
				this.console.text += iString;
			}else{

			}
		}
	}

}
