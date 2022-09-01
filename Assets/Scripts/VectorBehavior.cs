using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _accuracy;
    [SerializeField]
    private float _turnSpeed;
    [SerializeField]
    private float _detectionAngle;
    [SerializeField]
    private float _detectionRange;

    public Vector2 direction = new Vector2(0,0);
    public float magnitude;

    // Start is called before the first frame update
    void Start()
    {
        direction = this.transform.position - _target.position;
        magnitude = direction.magnitude;

        var xdir = this.transform.position.x - _target.position.x;
        var ydir = this.transform.position.y - _target.position.y;
        Debug.Log("x axis: " + this.transform.position.x +" - "+ _target.position.x + " = " + xdir );
        Debug.Log("y axis: " + this.transform.position.y +" - "+ _target.position.y + " = " + ydir );

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            HitTarget();
        }
    }

    private void FixedUpdate() {
        //MoveTowardsObject();
        //RotateTowardsObject();
        ObjectRotation();
        
    }

    private void MoveTowardsObject()
    {
        direction = _target.position - this.transform.position;
        magnitude = direction.magnitude;

        if(direction.magnitude > _accuracy)
        {
            this.transform.Translate(direction.normalized * _speed * Time.deltaTime);
        }
            
    }

    private void ObjectRotation()
    {
        var turn = Input.GetAxis("Horizontal") * _turnSpeed;
        turn *= Time.deltaTime;
        this.transform.Rotate(0,0,-turn);
    }

    private void HitTarget()
    {
        // var dot = Vector2.Dot(this.transform.right, _target.position);
        // var targetDetectionCone = dot * Mathf.Rad2Deg;
        // var distance = _target.position - this.transform.position;
        // var angle = Vector2.Angle(this.transform.position, _target.position);
        // if(!(Mathf.Abs(targetDetectionCone) <= _detectionAngle) || !(distance.magnitude <= _detectionRange))
        // {
        //     return;
        // } 

        // Debug.Log("Detected Target");
        // Debug.Log($"Dot Product: {targetDetectionCone}");
        // Debug.Log($"Angle Between Objects: {angle}");

#region Math Section
        //Step 1
        var targetDirection = _target.position - this.transform.position;

        //Step 2 
        //returns degrees
        var angle = Vector2.Angle(this.transform.up, targetDirection.normalized);

        //Step 3
        var cross = Vector3.Cross(this.transform.up, targetDirection);

        //Step 4
        var distance = Vector3.Distance(this.transform.position, _target.position);

        //Step 5
        var angleToRotate = (angle * (cross.z > 0 ? 1 : -1));
#endregion

        //Step 6
        if( angle <= _detectionAngle && distance <= _detectionRange)
        {
            this.transform.Rotate(0, 0, angleToRotate);
        }



    }

    // private void RotateTowardsObject()
    // {     
    //     var turn = (Vector2.Dot(this.transform.up, _target.position) > 0) ? 1f * _turnSpeed : -1f * _turnSpeed;

    //     turn *= Time.deltaTime;
    //     this.transform.Rotate(0,0, -turn);

    //     Debug.Log(Vector2.Dot(this.transform.up, _target.position));
    // }
}
