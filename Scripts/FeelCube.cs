using UnityEngine;
using MoreMountains.Feedbacks;

public class FeelCube : MonoBehaviour
{
    public KeyCode actionKey = KeyCode.Space;
    public float jumpForce = 8f;
    public MMFeedbacks jumpFeedback;
    public MMFeedbacks landingFeedback;

    private Rigidbody rb;
    private bool isJump;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKey) && !isJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJump = true;
        jumpFeedback?.PlayFeedbacks();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isJump) return;
        isJump = false;
        landingFeedback?.PlayFeedbacks();
    }
}