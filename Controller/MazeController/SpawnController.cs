using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [SerializeField] Transform posCharacter;
    [SerializeField] Transform posSpawnRoom;
    // Start is called before the first frame update
    void Start()
    {
        posCharacter.transform.position = posSpawnRoom.transform.position;
    }
}
