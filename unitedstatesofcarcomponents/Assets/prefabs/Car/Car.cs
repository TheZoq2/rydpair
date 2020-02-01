using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public float velocityDecay;
    public float rotationSpeed;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    private void UpdateMovement() {
        transform.position += velocity * Time.deltaTime;

        velocity *= velocityDecay;
    }

    private void UpdateInput() {
        velocity += transform.forward * acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
        velocity -= transform.forward * acceleration * Input.GetAxis("Reverse") * Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * (Vector3.Project(velocity, transform.forward).magnitude != 0 ? 1 : 0) * Time.deltaTime, 0);
    }
}
