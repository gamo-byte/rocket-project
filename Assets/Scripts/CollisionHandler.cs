using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    private AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Go go go");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StopMovement()
        {
        isControllable = false;
        GetComponent<Movement>().enabled = false;
    }

    private void StartSuccessSequence()
    {
        StopMovement();
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel",levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        StopMovement();
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        Invoke("ReloadLevel",levelLoadDelay);
    }

    private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                ReturnToFirstLevel();
            }
        else
            {
                SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void ReturnToFirstLevel()
        {
            SceneManager.LoadScene(0);
        }

    private void ReloadLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }