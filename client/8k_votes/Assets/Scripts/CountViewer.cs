using UnityEngine;
using System.Collections;

public class CountViewer : MonoBehaviour {
	[SerializeField] private TextMesh counter;
	[SerializeField] private Transform CounterPos;
	[SerializeField] private Gauge gauge;

	public GameObject board;
	public GameObject cube;

	void Start(){
		UpdateMaterial( this.MaterialNum );
	}
	void Update(){
		counter.gameObject.transform.position = CounterPos.position;
		counter.text = gauge.input.ToString();
		var a = (float)gauge.input / ((float)gauge.limit / 2f);
		a = a + 0.5f;
		counter.color = new Color( 1f, 1f, 1f, a );
		UpdateMaterial( this.MaterialNum );
	}
	public int MaterialNum = 1;
	private int currentMaterialNum;
	public void UpdateMaterial(int i){
		if( i != currentMaterialNum ){
			this.currentMaterialNum = i;
			var boardMate = "Materials/Item" + i.ToString();
			var cubeMate = "Materials/ScreenDraw0" + i.ToString();
			Debug.Log( "b : " + boardMate + "\t c : " + cubeMate.ToString() );
			this.board.renderer.material = Resources.Load(boardMate) as Material;
			 this.cube.renderer.material = Resources.Load(cubeMate) as Material;
		}
	}

}
