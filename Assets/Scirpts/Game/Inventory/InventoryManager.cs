using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<InventorySlot> slots;   //  all the slots that are able to put stuff in
    
    private List<InventorySlot> usedSlots = new List<InventorySlot>();  //  all the slots that have been filled
    private Queue<InventorySlot> availableSlots = new Queue<InventorySlot>();   //  the available solots queue
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

    //  Get the first available slot and dequeue it(cause it will be filled once got returned)
    public InventorySlot GetAvailableSlot() {
        if (availableSlots.Count <= 0) {
            Debug.Log("No vailable slot now!!");
            return null;
        }
        return availableSlots.Dequeue();
    }

    //  Get slots that have been filled already
    public List<InventorySlot> GetUsedSlots { get { return this.usedSlots; }}

    public InventorySlot GetSlotByIndex(int queryIndex) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].slotIndex == queryIndex) {
                return slots[i];
            }
            continue;
        }
        return null;
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        //  init the slots queue
        for (int i = 0; i < slots.Count; i++) {
            availableSlots.Enqueue(slots[i]);
        }
    }
}
