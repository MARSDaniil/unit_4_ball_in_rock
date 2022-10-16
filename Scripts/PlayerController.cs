using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;


    public bool hasPowerup = false;
    public bool isGameOver = false;

    public float speed = 5.0f;
    private float powerUpStrenght = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerDie();
        MovePlayer();
        StopPlayer();


    }
    private void MovePlayer()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }
    private void StopPlayer()
    {
        if (transform.position.y < -50)
        {
            transform.position = new Vector3(0, -50, 0);

        }
    }
    private void isPlayerDie()
    {
        if (playerRb.transform.position.y < -3)
        {
            isGameOver = true;
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup == true)
        {
            Rigidbody enemyRigitbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRigitbody.AddForce(awayFromPlayer * powerUpStrenght, ForceMode.Impulse);
          //  Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to  " + hasPowerup);
        }
    }
}
