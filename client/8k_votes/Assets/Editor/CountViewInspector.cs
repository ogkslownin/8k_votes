using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(CountViewer))]
public class CountViewerInspector : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		var t = target as CountViewer;
		t.UpdateMaterial( t.MaterialNum );
	}
}
