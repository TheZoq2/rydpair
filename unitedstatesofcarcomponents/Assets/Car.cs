using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public float velocityDecay;
    public float rotationAcceleration;
    public float rotationVelocityDecay;

    private Vector3 velocity;
    private float rotationVelocity = 0; //Negative right, positive right.

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    private void UpdateMovement() {
        transform.Rotate(0, rotationVelocity, 0);
        transform.position += velocity;

        velocity *= velocityDecay;
        rotationVelocity *= rotationVelocityDecay;
    }

    private void UpdateInput() {
        velocity += transform.forward * acceleration * Input.GetAxis("Accelerate");
        velocity -= transform.forward * acceleration * Input.GetAxis("Reverse");
        
        rotationVelocity += Input.GetAxis("Horizontal") * rotationAcceleration * Vector3.Project(velocity, transform.forward).normalized.magnitude;
    }
}
