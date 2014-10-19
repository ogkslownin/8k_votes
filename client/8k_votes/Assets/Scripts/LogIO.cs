using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LogIO : MonoBehaviour {

	public static LogIO instance;
	void Awake(){
		if( instance == null )
			instance = this;
		else{
			Debug.LogWarning("LogIO is already exist.");
			Destroy(this);
		}
		this.gaugeInterface = GetComponent<GaugeInterface>();
		logPath = "/../../";
		this.yesterday = DateTime.Today.AddDays(-1).ToString(dateTimeFormat);
		this.today = DateTime.Today.ToString(dateTimeFormat);
		this.isFileOpen = false;
	}
	public string today;
	public string yesterday;
	private string dateTimeFormat = "MMMM_dd_yyyy";

	public bool isFileOpen{ private set; get;}
	public GaugeInterface gaugeInterface;
	public string path;
	public int splitCountAtOneFrame;
	public static string logPath;

	public static void Log( string _s ){
		string path = Application.dataPath + logPath + instance.today + ".log";
		if( instance.isFileOpen ) return;
		CheckAndCreateLogFile( path );
		using ( StreamWriter sw = new StreamWriter( path, true, System.Text.Encoding.GetEncoding("UTF-8")) ){
			sw.Write( _s );
			Debug.Log( Path.GetFullPath( path ) );
			sw.Close();
		}
	}
	public static void ErrorLog( string _s ){
		string elog = _s.Split( returnChar )[0];
		var l = DateTime.Now.ToString() + "," + LogEvent.Error.ToString() + "," + elog + "\r\n";
		Log(l);
	}
	private static void CheckAndCreateLogFile( string _path ){
		if (File.Exists( _path )) {
		  // filePathのファイルは存在する
		} else {
			using (System.IO.FileStream hStream = System.IO.File.Create( _path )) {
				if( hStream != null ) {
					hStream.Close();
				}
			}
		}
	}

	void Start(){
		Log( DateTime.Now.ToString() + ",Boot," + "0" + "\r\n" );
		StartCoroutine( this.Load() );
	}
	void OnQuit(){
		Log( DateTime.Now.ToString() + ",Shutdown," + "quit" + "\r\n" );
	}
	void OnExit(){
		Log( DateTime.Now.ToString() + ",Shutdown," + "exit" + "\r\n" );
	}
	IEnumerator Load(){
		this.isFileOpen = true;
		yield return StartCoroutine( this.LoadIteration( Application.dataPath + logPath + yesterday + ".log") );
		yield return new WaitForSeconds(0.1f);
		yield return StartCoroutine( this.LoadIteration( Application.dataPath + logPath + today + ".log") );
		yield return new WaitForSeconds(0.1f);
		this.isFileOpen = false;
	}

	IEnumerator LoadIteration( string _path ){
		Debug.Log( "load : " + _path);
		StreamReader sr = null;
		try{
			sr = new StreamReader( _path, Encoding.GetEncoding("UTF-8") );
		}catch( System.Exception e ){
			var elog = e.ToString();
			ErrorLog(elog);
			yield break;
		}
		string text = sr.ReadToEnd();
		string[] lines = text.Split( returnChar );
		yield return StartCoroutine( this.LoadOneLine( lines ) );
	}
	IEnumerator LoadOneLine( string[] _lines ){
		int returnCount =  _lines.Length;
		this.splitCountAtOneFrame = (int)( returnCount / 100 );
		for( int i = 0;  i < returnCount; i++ ){
			this.ParseLog( _lines[i] );
			if( i == 0 ) yield return null;
			splitCountAtOneFrame = splitCountAtOneFrame <= 0 ? 1 : splitCountAtOneFrame;
			if( i % splitCountAtOneFrame == 0)
				yield return null;
		}
		yield return null;
	}
	public enum LogEvent{
		InputKey, Boot, Shutdown, Error, ForceInput,
	}
	static char[] returnChar = { "\n".ToCharArray()[0], "\r".ToCharArray()[0] };
	static char[] separator = { ",".ToCharArray()[0] };
	private void ParseLog( string _line ){
		Debug.Log( _line );
		if( _line == "" ) return;
		string[] words = _line.Split( separator );
		//Debug.Log( "\n" + words[0] + "\n" + words[1] + "\n" + words[2] );
		//DateTime dayTime = DateTime.Parse( words[0] );
		//DateTime yesterday = DateTime.Today.AddDays( -1 );
		 // Event
		foreach( var k in this.keyToNumDict.Keys ){
		//	Debug.Log( k.ToString() + "\t:\t" + this.keyToNumDict[ k ] );
		}
		//this.keyToNumDict.ForEach( c => Debug.Log( c.ToString() ) );
		LogEvent eventType =(LogEvent)Enum.Parse( typeof(LogEvent), words[1] );
		var key = words[2].ToLower();
		//Debug.Log( key );
		switch( eventType ){
			case LogEvent.InputKey:
				var arr = key.ToCharArray();
				var lastOne = arr[arr.Length -1 ].ToString();
				if( this.keyToNumDict.ContainsKey( key ) )
					this.AdditionTo( this.keyToNumDict[ key ] );
				else if( this.keyToNumDict.ContainsKey( lastOne ) ) // Alpha1 とか記述される時用。
					this.AdditionTo( this.keyToNumDict[ lastOne ] );
				else
					Debug.Log( key + " could not found." );
			break;
			case LogEvent.Boot:
			break;
			case LogEvent.Shutdown:
			break;
			case LogEvent.Error:
			break;
			case LogEvent.ForceInput:
				Debug.Log(words[3]);
				int inputCount = System.Convert.ToInt32( words[3] );
				if( this.keyToNumDict.ContainsKey( key ) )
					for(int i = 0;  i < inputCount ; i++ )
						this.AdditionTo( this.keyToNumDict[ key ] );
			break;
		}
	}
	Dictionary<string,int> keyToNumDict = new Dictionary<string,int>{
		{"1",0},{"2",1},{"3",2},{"4",3},{"5",4},
		{"q",0},{"w",1},{"e",2},{"r",3},{"t",4},
		{"a",0},{"s",1},{"d",2},{"f",3},{"g",4},
		{"z",0},{"x",1},{"c",2},{"v",3},{"b",4},
	};
	private void AdditionTo( int vote){
		this.gaugeInterface.Addition( vote );
	}

}
