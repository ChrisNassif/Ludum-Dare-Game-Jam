// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class LavaObject : MonoBehaviour
{
    public static GameOver gameOverScript;
    public static GameObject Player;
    public int sectionNumber;

    // checks at 85, 221, 302, 410 (end)
    void Awake() {
        Player = GameObject.Find("Player");
        gameOverScript = GameObject.Find("GeneralManager").GetComponent<GameOver>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider == Player.GetComponent<Collider>()) {
            gameOverScript.gameOver();
        }
    }
}
