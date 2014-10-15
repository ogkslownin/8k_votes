using UnityEngine;
using System.Collections.Generic;

public class GaugeInterface : MonoBehaviour {

	public float addionRate = 1f;
	public List<Gauge> gauges;
	public void Addition( int i ){
		this.gauges[i].Vote(1);
	}
}
