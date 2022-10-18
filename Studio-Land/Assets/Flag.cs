using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3) // hit player
        {
            Debug.Log("loading scene");
            SceneManager.LoadScene("Main");
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
