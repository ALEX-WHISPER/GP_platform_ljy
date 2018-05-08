using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<InventorySlot> slots;
    public List<InventorySlot> usedSlots = new List<InventorySlot>();

    private Queue<InventorySlot> availableSlots = new Queue<InventorySlot>();
    private static InventoryManager _instance = null;

    public static InventoryManager GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<InventoryManager>();

                if (_instance == null) {
                    GameObject go = new GameObject("InventoryManager");
                    go.AddComponent<InventoryManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    public InventorySlot GetAvailableSlot() {
        if (availableSlots.Count <= 0) {
            Debug.Log("No vailable slot now!!");
            return null;
        }
        return availableSlots.Dequeue();
    }

    public InventorySlot GetSlotByIndex(int queryIndex) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].slotIndex == queryIndex) {
                return slots[i];
            }
            continue;
        }
        return null;
    }

    private void Start() {
        for (int i = 0; i < slots.Count; i++) {
            availableSlots.Enqueue(slots[i]);
        }
    }
}
