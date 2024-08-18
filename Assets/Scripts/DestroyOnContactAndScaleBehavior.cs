using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContactAndScaleBehavior : MonoBehaviour
{
    private GravitationalAttraction gravitationalAttraction;
    [SerializeField]
    private float newScale;
    [SerializeField]
    private float newGravitationalConstantIncrement;
    void Start() {
        gravitationalAttraction = GetComponent<GravitationalAttraction>();
     }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO add gameover sequence
            GameManager.RestartGame();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids"))
        {
            AbsorbAstroid();
            Destroy(other.gameObject);
        }
    }
    void AbsorbAstroid()
    {
        //TODO add score
        //TODO make black hole grow more when absorbing bigger asteroids
        transform.localScale += new Vector3(newScale, newScale, newScale);
        gravitationalAttraction.IncrementGravitationalConstant(newGravitationalConstantIncrement);

    }
}
