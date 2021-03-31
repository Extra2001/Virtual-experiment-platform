using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class MoveRuler : MonoBehaviour {
    private GameObject head_obj, body_obj, cube_obj;
    private Rigidbody head_rigid, body_rigid;
    private BoxCollider head_collide, body_collide;
    // Start is called before the first frame update
    private bool moving = false;
    private int status = 0;
    private static readonly Vector3 head_pos0 = new Vector3(-8, 0, 0);
    private static readonly Vector3[] positions = new Vector3[] {
        new Vector3(0,5,-1),new Vector3(0,5,-0.5f),new Vector3(0,5,0),
        new Vector3(0,5,0.5f),new Vector3(0,5,1)
    };
    private void Start() {
        body_obj = GameObject.Find("MeasureBody");
        head_obj = GameObject.Find("MeasureHead");
        cube_obj = GameObject.Find("book_0001b");
        body_rigid = body_obj.GetComponent<Rigidbody>();
        head_rigid = head_obj.GetComponent<Rigidbody>();
        body_collide = body_obj.GetComponent<BoxCollider>();
        head_collide = head_obj.GetComponent<BoxCollider>();
        Debug.Log(string.Join("\r\n", cube_obj.transform.position, cube_obj.transform.rotation));
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if(!moving && Input.GetKeyDown(KeyCode.P)) {
            moving = true;
            body_rigid.velocity = Vector3.right;
            head_rigid.velocity = Vector3.left;
        }
        else if(Input.GetKeyDown(KeyCode.O)) {
            Debug.Log(Mathf.Abs(body_collide.transform.position.x - head_collide.transform.position.x).ToString("0.000"));
            Debug.Log(moving);
        }
        else if(moving && Input.GetKeyDown(KeyCode.L)) {
            head_rigid.velocity = Vector3.zero;
            body_rigid.velocity = Vector3.zero;
            moving = false;
        }
        else {
            if(!moving) {
                if(Input.GetKeyDown(KeyCode.R)) {//resetposition
                    ResetRulerPosition();
                    status = (status + 1) % 3;
                    switch(status) {
                    case 1:
                    cube_obj.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                    case 2:
                    cube_obj.transform.rotation = Quaternion.Euler(0, 0, 90);
                    cube_obj.transform.position = new Vector3(-2.42f, 25.0f, 0);
                    break;
                    default:
                    cube_obj.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                    }
                    Debug.Log(string.Join("\r\n", cube_obj.transform.position, cube_obj.transform.rotation));
                }
                else if(Input.GetKeyDown(KeyCode.Alpha1)) {
                    ResetRulerPosition();
                    transform.position = positions[0];
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                    ResetRulerPosition();
                    transform.position = positions[1];
                }
                else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                    ResetRulerPosition();
                    transform.position = positions[2];
                }
                else if(Input.GetKeyDown(KeyCode.Alpha4)) {
                    ResetRulerPosition();
                    transform.position = positions[3];
                }
                else if(Input.GetKeyDown(KeyCode.Alpha5)) {
                    ResetRulerPosition();
                    transform.position = positions[4];
                }
            }

        }
    }
    private void ResetRulerPosition() {
        body_rigid.transform.localPosition = Vector3.zero;
        head_rigid.transform.localPosition = head_pos0;
        head_rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
        body_rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.attachedRigidbody == head_rigid) {
            head_rigid.constraints |= RigidbodyConstraints.FreezePositionX;
            head_rigid.velocity = Vector3.zero;
        }
        else if(collision.collider.attachedRigidbody == body_rigid) {
            body_rigid.constraints |= RigidbodyConstraints.FreezePositionX;
            body_rigid.velocity = Vector3.zero;
        }
    }
}
