using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public int floatForce = 1;
    private float gravityModifier = 2f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    private float backgroundHeight;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);

        backgroundHeight = (GameObject.Find("Background").transform.position.y * 2) - 2.0f;
   

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
           
        }
        if (transform.position.y > backgroundHeight && playerRb.velocity.y > 0)
        {
            floatForce = 4;
            transform.position = new Vector3(transform.position.x, backgroundHeight, transform.position.z);
            playerRb.AddForce(Vector3.down * floatForce, ForceMode.Impulse);
            playerRb.velocity = Vector3.up * -floatForce / 4;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground") && !gameOver){
            floatForce = 8;
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            playerRb.velocity = Vector3.up * floatForce / 8;
            playerAudio.PlayOneShot(bounceSound, 1.0f);
        }

    }

}
