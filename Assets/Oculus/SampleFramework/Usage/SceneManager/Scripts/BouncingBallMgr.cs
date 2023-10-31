using UnityEngine;

public class BouncingBallMgr : MonoBehaviour
{
    [SerializeField] private Transform trackingspace;
    [SerializeField] private GameObject rightControllerPivot;
    [SerializeField] private OVRInput.RawButton actionBtn;
    [SerializeField] private GameObject ball;

    private GameObject currentBall;
    private bool ballGrabbed = false;

    private void Update()
    {
        if (!ballGrabbed && OVRInput.GetDown(actionBtn))
        {
           actionDown();
        }

        if (ballGrabbed && OVRInput.GetUp(actionBtn))
        {
            actionUp();
        }
    }

    public void actionDown()
    {
        Debug.Log("actionDown" + ballGrabbed);
        if (!ballGrabbed)
        {
            currentBall = Instantiate(ball, rightControllerPivot.transform.position, Quaternion.identity);
            currentBall.transform.parent = rightControllerPivot.transform;
            ballGrabbed = true;
        }
    }
    public void actionUp()
    {
        Debug.Log("actionUp" + ballGrabbed);
        if (ballGrabbed)
        {
            currentBall.transform.parent = null;
            var ballPos = currentBall.transform.position;
            var vel = trackingspace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand);
            var angVel = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RHand);
            currentBall.GetComponent<BouncingBallLogic>().Release(ballPos, vel, angVel);
            ballGrabbed = false;
        }
    }
}
