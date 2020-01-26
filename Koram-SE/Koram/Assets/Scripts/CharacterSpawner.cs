using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] Characters;
    public Transform PlayerSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Characters[CharacterSelect.PlayerNum], PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
    }
}
