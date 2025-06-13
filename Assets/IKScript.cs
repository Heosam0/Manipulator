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

    public void BaseEqual(float equal) => BaseTarget = equal;
    public void Joint1Equal(float equal) => Joint1Target = equal;
    public void Joint2Equal(float equal) => Joint2Target = equal;
    public void Joint3Equal(float equal) => Joint3Target = equal;


    private void Update()
    {
        Base.localEulerAngles = new Vector3(0, NewBaseAngle(BaseTarget, Base), 0);
        Joint1.localEulerAngles = new Vector3(NewJointAngle(Joint1Target, Joint1),0,0);
        Joint2.localEulerAngles = new Vector3(NewJointAngle(Joint2Target, Joint2),0,0);
        Joint3.localEulerAngles = new Vector3(NewJointAngle(Joint3Target, Joint3),0,0);


    }


    private float NormalizeAngle(float angle)
    {
        float newAngle = angle % 360;
        if (newAngle > 180) newAngle -= 360;
        return newAngle;
    }

   
    private float NewJointAngle(float angleTarget,Transform joint)
    {
        float currentAngle = NormalizeAngle(joint.localEulerAngles.x);
        float angleDiff = Mathf.DeltaAngle(currentAngle, angleTarget);
        float step = RotationSpeed * Time.deltaTime;
        return currentAngle + Mathf.Clamp(angleDiff, -step, +step);
    }
    private float NewBaseAngle(float angleTarget, Transform joint)
    {
        float currentAngle = NormalizeAngle(joint.localEulerAngles.y);
        float angleDiff = Mathf.DeltaAngle(currentAngle, angleTarget);
        float step = RotationSpeed * Time.deltaTime;
        return currentAngle + Mathf.Clamp(angleDiff, -step, +step);
    }






}
