using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKScript : MonoBehaviour
{
    [SerializeField] Transform Base;
    [SerializeField] Transform Joint1, Joint2, Joint3;
    [SerializeField] Transform EndEffector;

    public void BaseEqual(float equal) => Base.localEulerAngles = new Vector3 (0, equal, 0);
    public void Joint1Equal(float equal) => Joint1.localEulerAngles = new Vector3(equal, 0, 0);
    public void Joint2Equal(float equal) => Joint2.localEulerAngles = new Vector3(equal, 0, 0);
    public void Joint3Equal(float equal) => Joint3.localEulerAngles = new Vector3(equal, 0, 0);
    

}
