using UnityEngine;
using System.Collections;

public class InventoryItem {
	private int maxHp;
	private int atk;
	private int def;

	public InventoryItem(int maxHp, int atk, int def) {
		this.maxHp = maxHp;
		this.atk = atk;
		this.def = def;
	}

	public int getMaxHp() {
		return maxHp;
	}

	public int getAtk() {
		return atk;
	}

	public int getDef() {
		return def;
	}
}
