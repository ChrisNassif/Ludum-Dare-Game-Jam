using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LavaScript : MonoBehaviour{

    public uint lavaRaiseTimer = 6000; // 10 seconds at 60 fps
    public uint lavaLowerTimer = 100; // a little under 2 seconds at 60 fps
    public float maxLavaHeight; //should be changed after every checkpoint
    public float minLavaHeight;
    public float lavaRaiseRate = 0;
    public float lavaQuickRiseRate = 0;

    public Collider playerCollider;
    public GameOver gameOverScript;
    
    public GameObject mapObject;
    public Material lavaMaterial;
    public List<GameObject> lavaObjects;
    public List<GameObject> safePoints;
    public List<GameObject> seenSafePoints;
    public Queue<int> lavaHeights;

    public TextMeshProUGUI maxLavaHeightText;


    //lava at 18, 176, 177, 178, 191, 310, 340, 351, 352, 354, 356, 399, 402


    void Awake() {
        //Search the map for all of the lavas in the map
        lavaObjects = new List<GameObject>();
        foreach (Transform child in mapObject.transform) {
            if (child.gameObject.GetComponent<Renderer>().material == lavaMaterial)
                lavaObjects.Add(child.gameObject);
        }

    }

    void FixedUpdate() {
        //lava starts rising every 10 seconds
        //once you reach a checkpoint, that lava will rise to 
        // if (lavaRaiseTimer == 0)
        //     lavaRaiseTimer = 6000;
        // else
        //     lavaLowerTimer--;
    }

    void raiseLava() {

    }

    void lavaQuickRise() {

    }

    void lowerLava() {

    }

    void OnTriggerEnter(Collider collider) {
        if (collider == playerCollider) {
            Debug.Log("Lava is touching player");
        }

        foreach (GameObject safePoint in safePoints) {
            if (collider == safePoint.GetComponent<Collider>() && !seenSafePoints.Contains(safePoint))
                this.changeMaxLavaHeight(lavaHeights.Dequeue());
                this.changeMinLavaHeight(safePoint.transform.position.y-0.1f);
        }
    }

    void changeMaxLavaHeight (float maxLavaHeight_) {
        maxLavaHeight = maxLavaHeight_;
        maxLavaHeightText.text = "Lava Rises to " + maxLavaHeight_ + " Meters";
    }
    void changeMinLavaHeight (float minLavaHeight_) {
        minLavaHeight = minLavaHeight_;
    }
    
}
