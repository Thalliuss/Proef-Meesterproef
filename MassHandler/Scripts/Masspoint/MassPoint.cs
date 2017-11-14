using UnityEngine;

public class MassPoint : MonoBehaviour
{
    /*private void Update()
    {
        var _sun = MassPointManager.Instance.sun;
        if (_sun != null) transform.RotateAround(_sun.position, Vector3.up, 5f * Time.deltaTime);
        transform.Rotate(0, .2f, .2f);
    }*/
     
    public void Attract(Transform p_body, float p_gravity, float _gravitationalField)
    { 
        Vector3 t_bodyUp = p_body.up;
        Vector3 t_gravityUp = (p_body.position - transform.position);
        t_gravityUp.Normalize();

        p_body.GetComponent<Rigidbody>().AddForce(t_gravityUp * p_gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(t_bodyUp, t_gravityUp) * p_body.rotation;
        p_body.rotation = Quaternion.Slerp(p_body.rotation, targetRotation, 500f * Time.deltaTime);
    }
}
