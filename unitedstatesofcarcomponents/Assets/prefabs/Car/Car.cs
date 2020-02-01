using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public float velocityDecay;
    public float minimumSpeed;
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

    public void UpdateComponentEffects() {

    }

    private void UpdateMovement() {
        transform.position += velocity * Time.deltaTime;

        velocity *= velocityDecay;
        if(velocity.magnitude < minimumSpeed) {
            velocity = Vector3.zero;
        }
    }

    private void UpdateInput() {
        velocity += transform.forward * acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
        velocity -= transform.forward * acceleration * Input.GetAxis("Reverse") * Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * (Vector3.Project(velocity, transform.forward).magnitude != 0 ? 1 : 0) * Time.deltaTime, 0);
    }
}
