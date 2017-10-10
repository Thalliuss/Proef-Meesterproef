using System;
using System.Collections;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField]
    private bool _isMoving = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            StartCoroutine(MoveTowards(t_ray));
        }
    }

    private IEnumerator MoveTowards(Ray p_ray)
    {
        MeshRenderer t_meshRenderer = GetComponent<MeshRenderer>();
        RaycastHit t_hit;
        if (Physics.Raycast(p_ray, out t_hit))
        {
            Vector3 targetPostition = new Vector3(t_hit.point.x, transform.position.y, t_hit.point.z);
            transform.LookAt(targetPostition);

            while (true)
            {
                GetComponent<GravityHandler>().enabled = false;
                var lol = t_hit.point.y + t_meshRenderer.bounds.size.y / 2 - transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(t_hit.point.x, t_hit.point.y + t_meshRenderer.bounds.size.y / 2 - lol, t_hit.point.z), Time.deltaTime * 15f);

                Ray t_ray = new Ray(transform.position, transform.forward);
                RaycastHit t_hitJump;
                if (Physics.Raycast(t_ray, out t_hitJump, 1f))
                    transform.Translate(Vector3.up * 1, Space.World);

                yield return new WaitForSeconds(.01f);

                if ((int)transform.position.x == (int)t_hit.point.x && (int)transform.position.z == (int)t_hit.point.z) break;
                if (Input.GetKeyDown(KeyCode.Mouse0)) break;

                /*Ray t_rayDown = new Ray(transform.position, -transform.up);
                RaycastHit t_hitDown;
                if (!Physics.Raycast(t_rayDown, out t_hitDown, 1.5f)) break;*/
            }
            print("LOL");
            GetComponent<GravityHandler>().enabled = true;
        }
    }
}