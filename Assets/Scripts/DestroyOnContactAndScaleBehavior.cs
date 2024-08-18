using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContactAndScaleBehavior : MonoBehaviour
{
    private GravitationalAttraction gravitationalAttraction;
    void Start() {
        gravitationalAttraction = GetComponent<GravitationalAttraction>();
     }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO add gameover sequence
            Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids"))
        {
            Destroy(other.gameObject);
        }
    }
    void AbsorbAstroid()
    {
        //TODO add score
        //TODO make black hole grow more when absorbing bigger asteroids
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        gravitationalAttraction.IncrementGravitationalConstant(0.1f);

    }
}
