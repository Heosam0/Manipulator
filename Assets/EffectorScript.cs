using System;
using UnityEngine;

public class EffectorScript : MonoBehaviour
{
    [SerializeField] private Claw[] Claws;

    public void UpdateClawPosition(float value)
    {
        float angle = Mathf.Clamp(20, -20, value);
        for(int i = 0; i < Claws.Length; i++) Claws[i].TargetAngle = angle;
    }
    
    private void Update()
    {
        for (int i = 0; i < Claws.Length; i++)
            Claws[i].UpdateAngles();
    }

}

[Serializable]
public struct Claw
{
    public Transform Transform;
    public float TargetAngle;

    public void UpdateAngles()
    {
        float currentAngle = Transform.localEulerAngles.z;
        float angleDiff = Mathf.DeltaAngle(currentAngle, TargetAngle);
        float step = 180f * Time.deltaTime;
        Transform.localEulerAngles = new Vector3(0, Transform.localEulerAngles.y, currentAngle + Mathf.Clamp(angleDiff, -step, +step));
    }

}

