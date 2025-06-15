using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKScript : MonoBehaviour
{

    [SerializeField] private float RotationSpeed = 180f;
    [SerializeField] private Transform Base;
    [SerializeField] private float BaseTarget, Joint1Target, Joint2Target, Joint3Target;
    [SerializeField] private Transform Joint1, Joint2, Joint3;
    [SerializeField] private Transform EndEffector;

    [SerializeField] private Transform TargetTransform;
    [SerializeField] private int CCDIterations = 5;
    [SerializeField] private float CCDDamping = 0.5f;

  // public void BaseEqual(float equal) => BaseTarget = equal;
  // public void Joint1Equal(float equal) => Joint1Target = equal;
  // public void Joint2Equal(float equal) => Joint2Target = equal;
  // public void Joint3Equal(float equal) => Joint3Target = equal;


    private void Update()
    {
        CalculateAnglesByCCD();

        Base.localEulerAngles = new Vector3(0, NewBaseAngle(BaseTarget, Base), 0);
        Joint1.localEulerAngles = new Vector3(NewJointAngle(Joint1Target, Joint1),0,0);
        Joint2.localEulerAngles = new Vector3(NewJointAngle(Joint2Target, Joint2),0,0);
        Joint3.localEulerAngles = new Vector3(NewJointAngle(Joint3Target, Joint3),0,0);
    }

    private void CalculateAnglesByCCD() {

        for (int i = 0; i < CCDIterations; i++)
        {
            UpdateTargetAngle(Joint3, ref Joint3Target, Axis.X);
            UpdateTargetAngle(Joint2, ref Joint2Target, Axis.X);
            UpdateTargetAngle(Joint1, ref Joint1Target, Axis.X);
            UpdateTargetAngle(Base, ref BaseTarget, Axis.Y);

        }
    }

    private void UpdateTargetAngle(Transform joint, ref float targetAngle, Axis axis)
    {
        Vector3 toEffector = EndEffector.position - joint.position;
        Vector3 toTarget = TargetTransform.position - joint.position;

        Vector3 rotationAxis = axis == Axis.X ? joint.right : joint.up;

        float angle = Vector3.SignedAngle(toEffector, toTarget, rotationAxis);
        angle *= CCDDamping;


        targetAngle = Mathf.Clamp(targetAngle + angle, -90, 90);

             

      //
      //  Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
      //  Vector3 newDirection = rotation * toEffector;
      //  float deltaAngle = (axis == Axis.X) ? 
      //      Vector3.SignedAngle(joint.right, newDirection, joint.forward) :
      //      Vector3.SignedAngle(joint.up, newDirection, Vector3.up);
      //
      //  targetAngle += deltaAngle;

    }



   
    private float NewJointAngle(float angleTarget,Transform joint)
    {
        float currentAngle = joint.localEulerAngles.x;
        float angleDiff = Mathf.DeltaAngle(currentAngle, angleTarget);
        float step = RotationSpeed * Time.deltaTime;
        return currentAngle + Mathf.Clamp(angleDiff, -step, +step);
    }
    private float NewBaseAngle(float angleTarget, Transform joint)
    {
        float currentAngle = joint.localEulerAngles.y;
        float angleDiff = Mathf.DeltaAngle(currentAngle, angleTarget);
        float step = RotationSpeed * Time.deltaTime;
        return currentAngle + Mathf.Clamp(angleDiff, -step, +step);
    }





    private enum Axis { X, Y }
}

