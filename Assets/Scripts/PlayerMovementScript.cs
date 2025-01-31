using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick3D : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickHandle;
    public RectTransform joystickBackground;
    public float moveSpeed = 5f;
    public Rigidbody playerRigidbody;
    public float jumpHeight = 3f;
    public float pushForce = 10f;
    public Transform grabPoint; 

    private Vector2 lastDirection;
    private GameObject grabbedObject = null;
    private float throwForce = 10f;

    private IEnumerator MakeKinematicAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = true;
    }


    private void Start()
    {
        joystickHandle.anchoredPosition = Vector2.zero;
        lastDirection = Vector2.zero;
    }

    private void Update()
    {
        MovePlayer(lastDirection);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position);
        Vector2 direction = position / (joystickBackground.sizeDelta / 2);
        direction = Vector2.ClampMagnitude(direction, 1);
        joystickHandle.anchoredPosition = direction * (joystickBackground.sizeDelta.x / 2);
        lastDirection = direction;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickHandle.anchoredPosition = Vector2.zero;
        lastDirection = Vector2.zero;
    }

    private void MovePlayer(Vector2 direction)
    {
        if (direction.magnitude > 0)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y) * moveSpeed;
            playerRigidbody.linearVelocity = new Vector3(moveDirection.x, playerRigidbody.linearVelocity.y, moveDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void Jump()
    {
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, jumpHeight, playerRigidbody.linearVelocity.z);
    }

    public void Push(Vector3 direction)
    {
        Vector3 targetVelocity = direction.normalized * pushForce;
        Vector3 velocityChange = targetVelocity - playerRigidbody.linearVelocity;
        playerRigidbody.linearVelocity += velocityChange * Time.deltaTime * 3f;
    }

    public void GrabObject(GameObject objectToGrab)
    {
        if (grabbedObject != null) return;

        grabbedObject = objectToGrab;
        Rigidbody objectRigidbody = grabbedObject.GetComponent<Rigidbody>();

        if (objectRigidbody != null)
        {
            objectRigidbody.linearVelocity = Vector3.zero; 
            objectRigidbody.angularVelocity = Vector3.zero;
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, grabPoint.position, Time.deltaTime * 10f);
            grabbedObject.transform.SetParent(grabPoint);
            StartCoroutine(MakeKinematicAfterDelay(objectRigidbody)); 
        }
    }


    public void ReleaseObject()
    {
        if (grabbedObject == null) return;

        Rigidbody objectRigidbody = grabbedObject.GetComponent<Rigidbody>();

        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = false;
            grabbedObject.transform.SetParent(null);
            objectRigidbody.AddForce(playerRigidbody.transform.forward * throwForce, ForceMode.Impulse); 
        }

        grabbedObject = null;
    }



    public void PushForward()
    {
        Push(Vector3.forward);
    }
}
