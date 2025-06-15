using System;
using UnityEngine;

public class IKScript : MonoBehaviour
{

    [SerializeField] private float RotationSpeed = 180f;
    public ManipulatorJoint[] Joints;
   
    [SerializeField] private Transform EndEffector, TargetTransform;
    [SerializeField] private int CCDIterations = 5;
    [SerializeField] private float CCDDamping = 0.5f,
        Threshold = 0.001f;
    [SerializeField] private bool IsFollowingTarget = true;

    public void BaseEqual(float angle) => Joints[0].SetTargetAngle(angle);
    public void SetFollowingState(bool state) => IsFollowingTarget = state;
    public void Joint1Equal(float angle) => Joints[1].SetTargetAngle(angle);
    public void Joint2Equal(float angle) => Joints[2].SetTargetAngle(angle);
    public void Joint3Equal(float angle) => Joints[3].SetTargetAngle(angle);

    private void Update()
    {
        if (IsFollowingTarget)
            CalculateAnglesByCCD();

        
            foreach(var joint in Joints)
                joint.Transform.localEulerAngles = joint.Axis == Axis.X
                ? new Vector3(NewJointAngle(joint), 0, 0)
                : new Vector3(0, NewJointAngle(joint), 0);
            
    }

    private void CalculateAnglesByCCD() {

        for (int i = 0; i < CCDIterations; i++)
            for (int j = 0; j < Joints.Length; j++)
                UpdateTargetAngle(ref Joints[j]);
    }

    private void UpdateTargetAngle(ref ManipulatorJoint joint)
    {
        Vector3 toEffector = EndEffector.position - joint.Transform.position;
        Vector3 toTarget = TargetTransform.position - joint.Transform.position;

        Vector3 rotationAxis = joint.Axis == Axis.X 
            ? joint.Transform.right 
            : joint.Transform.up;

        float angle = Vector3.SignedAngle(toEffector, toTarget, rotationAxis);
        angle *= CCDDamping;

        if (Mathf.Abs(angle) < Threshold)
            return;

        joint.TargetAngle = Mathf.Clamp(joint.TargetAngle + angle, -90, 90);

    }
       
    private float NewJointAngle(ManipulatorJoint joint)
    {
        float currentAngle = joint.Axis == Axis.X ? joint.Transform.localEulerAngles.x : joint.Transform.localEulerAngles.y;
        float angleDiff = Mathf.DeltaAngle(currentAngle, joint.TargetAngle);
        float step = RotationSpeed * Time.deltaTime;
        return currentAngle + Mathf.Clamp(angleDiff, -step, +step);
    }
  
}

public enum Axis { X, Y }

[Serializable]
public class ManipulatorJoint
{
    public Transform Transform;
    public float TargetAngle;
    public Axis Axis;

    public void SetTargetAngle(float targetAngle) => TargetAngle = targetAngle;
}

