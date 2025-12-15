using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{

    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 10f;


    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ApplyThrust();
        ApplyRotation();
    }

    void ApplyThrust()
        {
            if (thrust.IsPressed())
            {
                rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
                if (!audioSource.isPlaying)
                {
                audioSource.Play();
                }
            }
            else
            {
                audioSource.Pause();
            }
        }

    private void ApplyRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            Rotate(rotationStrength);
        }
        else if (rotationInput > 0)
        {
            Rotate(-rotationStrength);
        }
    }

    private void Rotate(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}

