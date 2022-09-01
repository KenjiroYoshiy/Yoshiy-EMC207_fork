using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorBehaviorBetter : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _accuracy;
    [SerializeField]
    private float _turnSpeed;
    
    //In Degrees
    [SerializeField][Tooltip("In Degrees")]
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
        
    }

    private void FixedUpdate() {
        //MoveTowardsObject();
        //ObjectRotation();
        HitTarget();
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

    // private void ObjectRotation()
    // {
    //     var turn = Input.GetAxis("Horizontal") * _turnSpeed;
    //     turn *= Time.deltaTime;
    //     this.transform.Rotate(0,0,-turn);
    // }

    private void HitTarget()
    {
        // (step 1) We first need to get our target's direction
        var targetDirection = _target.position - this.transform.position;

        //var dot = Vector3.Dot(this.transform.up, targetDirection);
        //var targetDetectionCone = dot * Mathf.Rad2Deg;
        
        // (step 2) Then we need to get the relative angle between our object's "eyes" and the target
        // This returns a degree
        var angle = Vector2.Angle(this.transform.up, targetDirection.normalized);

        //  (step 3) Next, we get the cross product of our object's "eyes" and the target's direction
        //  This helps us determine which side the target is, if the cross.z is positive then the target is on the right and vice versa
        var cross = Vector3.Cross(this.transform.up, targetDirection);

        //  (step 4) We then get the distance between our object and target to determine if it is in range
        var distance = Vector3.Distance(this.transform.position, _target.position);
        
        // (step 5) We then need to determing if the angle we should rotate to is clockwise (positive) or counter-clockwise (negative)
        var angleToRotate = (angle * (cross.z > 0 ? 1 : -1));

        // Finally we then check if the angle and distance between the two objects are below our specified angles and range
        if(angle <= _detectionAngle && distance <= _detectionRange)
        {
            RotateTowardsObject(angleToRotate);
        }


        
        // Drey
        //var dotAngle = DotAngle(this.transform.up, targetDirection.normalized);
        
        
        // Rad2Deg needed to get angle degrees. Originally computes rad
        // same function with Vector2.Angle * Deg2Rad
        //var refinedAngle = (dotAngle * (cross.z > 0 ? 1 : -1)) * Mathf.Rad2Deg;

        //Debug.Log($"Dot Product: {targetDetectionCone}");
        //These two will display radians
        // Debug.Log($"RefinedAngle Between Objects: {refinedAngle}");
        // Debug.Log($"DotAngle Between Objects: {dotAngle}");
    }

    // Drey's Function
    // private float DotAngle(Vector2 point1, Vector2 point2)
    // {
    //     return Mathf.Acos((Vector3.Dot(point1, point2) / point1.magnitude * point2.magnitude));
    // }

    private void RotateTowardsObject(float p_angleToRotate)
    {     
        var turn = p_angleToRotate * _turnSpeed;

        turn *= Time.deltaTime;
        this.transform.Rotate(0,0, turn);
    }
}
