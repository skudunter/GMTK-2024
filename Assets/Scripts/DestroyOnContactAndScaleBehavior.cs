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

    IEnumerator ShowRestartScreen()
    {
        while (isPaused == false)
        {
            yield return new WaitForSeconds(0.5f);
            isPaused = true;
            
            GUI.ShowRestartScreen();
            GameManager.PauseGame();
        }
        StopCoroutine(ShowRestartScreen());
    }

    public bool isPaused = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.KillPlayer();
            StartCoroutine(ShowRestartScreen());
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
