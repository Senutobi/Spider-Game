using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpiderWalk : MonoBehaviour
{
    [SerializeField] private GameObject Spider;
    private Vector3 STP;
    private Vector3 movement;
    private bool isGrounded = true;
    private float Distance_0; private float Distance_1;
    private float Distance_2; private float Distance_3;
    
    [SerializeField] private GameObject Leg0; [SerializeField] private GameObject Leg0Target;
    [SerializeField] private GameObject Leg1; [SerializeField] private GameObject Leg1Target;
    [SerializeField] private GameObject Leg2; [SerializeField] private GameObject Leg2Target;
    [SerializeField] private GameObject Leg3; [SerializeField] private GameObject Leg3Target;
    
    private Vector3 _leg0LocalPosition; private Vector3 _rayOrigin0; private Ray ray0;
    private Vector3 _leg1LocalPosition; private Vector3 _rayOrigin1; private Ray ray1;
    private Vector3 _leg2LocalPosition; private Vector3 _rayOrigin2; private Ray ray2;
    private Vector3 _leg3LocalPosition; private Vector3 _rayOrigin3; private Ray ray3;

    private Vector3 _DisCon0; private Vector3 _DisCon1; private Vector3 _DisCon2; private Vector3 _DisCon3;

    [SerializeField] private float speed = 5f;
    public Rigidbody rb;
    void Start()
    {
        newDefaultPosition();
        rb = GetComponent<Rigidbody>();
    }

    ///Position relative to body
    void newDefaultPosition()
    {
        _leg0LocalPosition = Leg0.transform.position - STP;
        _leg1LocalPosition = Leg1.transform.position - STP;
        _leg2LocalPosition = Leg2.transform.position - STP;
        _leg3LocalPosition = Leg3.transform.position - STP;
        
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal"), z = Input.GetAxis("Vertical");
        movement = new Vector3(x, 0.0f, z).normalized;
        Jump();
        transform.Translate(movement * (speed * Time.deltaTime));
    }

    void WalkAnimation()
    {
        _rayOrigin0 = STP + _leg0LocalPosition; _rayOrigin0.y += 0.3f; // Determines Where Ray will come from
        _rayOrigin1 = STP + _leg1LocalPosition; _rayOrigin1.y += 0.3f;
        _rayOrigin2 = STP + _leg2LocalPosition; _rayOrigin2.y += 0.3f;
        _rayOrigin3 = STP + _leg3LocalPosition; _rayOrigin3.y += 0.3f;

        _DisCon0 = STP + _leg0LocalPosition; _DisCon0.y = Leg0Target.transform.position.y;
        _DisCon1 = STP + _leg1LocalPosition; _DisCon1.y = Leg1Target.transform.position.y;
        _DisCon2 = STP + _leg2LocalPosition; _DisCon2.y = Leg2Target.transform.position.y;
        _DisCon3 = STP + _leg3LocalPosition; _DisCon3.y = Leg3Target.transform.position.y;

        Distance_0 = Vector3.Distance(_DisCon0, Leg0Target.transform.position);
        Distance_1 = Vector3.Distance(_DisCon1, Leg1Target.transform.position);
        Distance_2 = Vector3.Distance(_DisCon2, Leg2Target.transform.position);
        Distance_3 = Vector3.Distance(_DisCon3, Leg3Target.transform.position);

        ray0 = new Ray(_rayOrigin0, transform.TransformDirection(Vector3.down)); // Ray for foot height.
        ray1 = new Ray(_rayOrigin1, transform.TransformDirection(Vector3.down));
        ray2 = new Ray(_rayOrigin2, transform.TransformDirection(Vector3.down));
        ray3 = new Ray(_rayOrigin3, transform.TransformDirection(Vector3.down));
        if (Distance_0 > 0.15f)
        {
            //Leg0Target.transform.position = (movement*0.15f) + ray0); //Step position = (Body direction * Step Area Constraint) + (Spiders current position + Leg position relative to body)
            //Physics.Raycast(_ray0, transform.TransformDirection(Vector3.down));
            Debug.Log(Distance_1);
            /*if (Physics.Raycast(ray0, out RaycastHit info, 1.5f))
            { 
                Leg0Target.transform.position = Vector3.Lerp(Leg0Target.transform.position, 
                    ((movement * 0.15f) + info.point), time);
                time += Time.deltaTime * speed;
                //if (Leg0Target.transform.position == ((movement * 0.15f) + info.point))
                //{
                //    transform.Translate(movement * (speed * Time.deltaTime));
                //}
            }*/
            if (Physics.Raycast(ray0, out RaycastHit info, 6f)) { 
                Leg0Target.transform.position = (movement * 0.15f) + info.point; 
            }
        }

        if (Distance_1 > 0.15f) {
            //Leg2Target.transform.position = (movement*0.15f) + (Spider.transform.position + _leg2LocalPosition);
            /*if (Physics.Raycast(ray1, out RaycastHit info, 1.5f)) {
                Leg1Target.transform.position = Vector3.MoveTowards(Leg1Target.transform.position,
                    ((movement * 0.15f) + info.point), 0.01f);
                if (Leg1Target.transform.position == (movement * 0.15f) + info.point) { 
                    transform.Translate(movement * (speed * Time.deltaTime));
                }
            }*/
            if (Physics.Raycast(ray1, out RaycastHit info, 6f)) { 
                Leg1Target.transform.position = (movement * 0.15f) + info.point; 
            }
        }
        if (Distance_3 > 0.15f) { 
            //Leg1Target.transform.position = (movement*0.15f) + (Spider.transform.position + _leg1LocalPosition);
            if (Physics.Raycast(ray3, out RaycastHit info, 6f)) { 
                Leg3Target.transform.position = (movement * 0.15f) + info.point; 
            }
            
        }

        if (Distance_2 > 0.15f) {
            //Leg3Target.transform.position = (movement*0.15f) + (Spider.transform.position + _leg3LocalPosition);
            if (Physics.Raycast(ray2, out RaycastHit info, 6f)) { 
                Leg2Target.transform.position = (movement * 0.15f) + info.point;
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name is "Ground" or "Log")
        {
            isGrounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        STP = Spider.transform.position;
        
        Movement();
        WalkAnimation();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_DisCon0, 0.05f);
        Gizmos.DrawSphere(_DisCon1, 0.05f);
        Gizmos.DrawSphere(_DisCon2, 0.05f);
        Gizmos.DrawSphere(_DisCon3, 0.05f);
    }
}
