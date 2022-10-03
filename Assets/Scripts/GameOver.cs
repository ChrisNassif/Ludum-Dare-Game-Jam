// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour{

    public Vector3 respawnPos;
    public Quaternion respawnRotation;
    public GameObject player;

    public void gameOver() {
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = respawnPos; //reset the player's position
        player.transform.GetChild(0).transform.rotation = respawnRotation; //reset the camera's rotation
        cc.enabled = true;
    }
}
