using UnityEngine;

public class Football : MonoBehaviour
{
    public bool isInsideGate;
    [HideInInspector] public new Rigidbody rigidbody;
    [HideInInspector] public TrailRenderer trailRenderer;

    private Vector3 m_StartingPosition;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        m_StartingPosition = transform.position;
    }

    public void ActivateBallBounce(Vector3 position)
    {
        // Set normal gravity
        SetGravity(false);

        var incomingForce = rigidbody.velocity.magnitude;
        var extraForce = 1.25f;
        var direction = (transform.position - position).normalized;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(direction * (incomingForce * extraForce), ForceMode.Impulse);
    }
    public void ResetPosition()
    {
        // Play sound
        AudioManager.Instance.PlaySound(Sounds.bounce);

        // Disable trail renderer, prevent visual bug
        trailRenderer.enabled = false;

        // Reset rigidbody
        SetGravity(true);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Reset position
        transform.position = m_StartingPosition;

        // Reset state
        isInsideGate = false;
    }
    public void SetGravity(bool shoot)
    {
        Physics.gravity = new Vector3(0,shoot ? -1.5f : -9.81f,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GateHitBox"))
            isInsideGate = true;
        if (other.CompareTag("Glove"))
            ActivateBallBounce(other.transform.position);
    }
}
