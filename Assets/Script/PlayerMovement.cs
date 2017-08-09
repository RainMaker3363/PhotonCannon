using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour {

    private PhotonView _PhotonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    public float Health;

    private void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update () {

        if(_PhotonView.isMine)
        {
            CheckInput();
        }
        else
        {
            SmoothMove();
        }
            
	}

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }

    
    private void CheckInput()
    {
        float moveSpeed = 100f;
        float rotationSpeed = 500f;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, horizontal * rotationSpeed * Time.deltaTime, 0));

    }
}
