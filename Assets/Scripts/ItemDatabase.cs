using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase {

	static public Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>() {
		{"water of life", new InventoryItem(5, 0, 0)},
		{"better sword", new InventoryItem(0, 1, 0)},
		{"better shield", new InventoryItem(0, 0, 1)}
	};

}
