using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // NOTE: This is a very quick and not so flexible implementation to keep track of score, since it
    // involves every coin resetting total points to 0 on spawn, meaning that more coins can't be
    // spawned without points being reset
    static int totalPoints = 0;
    [SerializeField] int points = 1;
    [SerializeField] FloatEventChannelSO scoreChannel;
    
    // Start is called before the first frame update
    void Start()
    {
        totalPoints = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3) // hit player
        {
            totalPoints += points;
            scoreChannel.RaiseEvent(totalPoints);
            Debug.Log("added point");
            Destroy(gameObject);
        }

    }
}
