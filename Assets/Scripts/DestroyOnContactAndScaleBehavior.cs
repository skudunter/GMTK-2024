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

    void Start()
    {
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
            StartCoroutine(AbsorbAsteroid(other.gameObject));
        }
        else if (other.gameObject.name == "asteroid")
        {

            StartCoroutine(AbsorbAsteroid(other.gameObject));
        }
    }

    
    IEnumerator AbsorbAsteroid(GameObject other){
        Animator animator = other.GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.Play("asteroidDeath");
        }
        other.GetComponent<CircleCollider2D>().enabled = false;
        other.GetComponentInParent<Rigidbody2D>().velocity /= 2;
        yield return new WaitForSeconds(0.5f);
        Destroy(other.gameObject);
        GameManager.DoScreenShake(other.GetComponentInParent<Rigidbody2D>().mass/7);
        transform.localScale += new Vector3(newScale, newScale, newScale);
        gravitationalAttraction.IncrementGravitationalConstant(newGravitationalConstantIncrement);
    }
}
