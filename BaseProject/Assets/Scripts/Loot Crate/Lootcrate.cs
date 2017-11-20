using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootcrate : MonoBehaviour {

	public bool genCrate = false;
	public List<float> chance = new List<float>();
	public int reward = 0;
	public int maxChance = 10000;

	void Update() {
		if (genCrate) {
			generateCrate ();
			genCrate = false;
		}
	}

	public void generateCrate() {
		float temp = Random.Range (0, maxChance);
		reward = -1;
		for (int a = 0; a < chance.Count; a++) {
			if (temp < chance[a]) {
				reward = a;
			}
		}
	}

}
