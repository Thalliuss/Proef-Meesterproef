using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    public MassPoint massPoint;

	[SerializeField]
    private float _gravitationalForce;
    [SerializeField]
    private float _gravitationalField;

    private void Start()
    {
        Rigidbody t_rigidbody = GetComponent<Rigidbody>();
        t_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        t_rigidbody.useGravity = false;
    }

	private void Update ()
    {
        Rigidbody t_rigidbody = GetComponent<Rigidbody>();
        var t_massPoints = MassPointManager.Instance.massPointHandler.massPoints;

        massPoint = null;
		_gravitationalForce = 0;
        _gravitationalField = 0;

        for (int i = 0; i < t_massPoints.Count; i++)
        {
            if (Vector3.Distance(transform.position, t_massPoints[i].massPoint.transform.position) <= t_massPoints[i].massPoint.transform.localScale.y * 20 + t_massPoints[i].gravitationalField) {
                massPoint = t_massPoints[i].massPoint;
                _gravitationalForce = t_massPoints[i].gravitationalForce;
                _gravitationalField = t_massPoints[i].gravitationalField;
            }
        }

        if (massPoint != null) {
            transform.parent = massPoint.transform;
            massPoint.Attract(transform, _gravitationalForce, _gravitationalField);
        } else t_rigidbody.velocity = Vector3.zero;
    }
}
