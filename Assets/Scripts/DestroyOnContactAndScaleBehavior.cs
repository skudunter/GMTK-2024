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
    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.ShowRestartScreen();
        Time.timeScale = 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Animator>().Play("asteroidDeath");
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.GetComponent<PlayerController>().enabled = false;
            other.gameObject.GetComponent<ShootGrappleGun>().enabled = false;
            other.gameObject.GetComponent<ShootLaser>().enabled = false;
            var audioSource = GameObject.Find("AudioInitializer").GetComponent<AudioSource>().volume = 0.1f;
 
            StartCoroutine(restartGame());
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids"))
        {
            SoundManager.PlayScaleSound(transform.position);
            StartCoroutine(AbsorbAsteroid(other.gameObject));
        }
        else if (other.gameObject.name == "asteroid")
        {
            SoundManager.PlayScaleSound(transform.position);

            StartCoroutine(AbsorbAsteroid(other.gameObject));
        }
    }

    IEnumerator AbsorbAsteroid(GameObject other)
    {
        Animator animator = other.GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.Play("asteroidDeath");
        }
        other.GetComponent<CircleCollider2D>().enabled = false;
        other.GetComponentInParent<KillOffScreen>().enabled = false;
        other.GetComponentInParent<Rigidbody2D>().velocity /= 2;
        yield return new WaitForSeconds(0.5f);
        Destroy(other.gameObject.transform.parent.gameObject);
        GameManager.DoScreenShake(other.GetComponentInParent<Rigidbody2D>().mass / 7);

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = initialScale + new Vector3(newScale, newScale, newScale);
        float duration = 1f; // Time it takes to scale up
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Ensure the final scale is reached
        gravitationalAttraction.IncrementGravitationalConstant(newGravitationalConstantIncrement);
    }
}
