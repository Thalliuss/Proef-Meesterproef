using System;
using System.Collections;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
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
        RaycastHit t_hit;
        if (Physics.Raycast(p_ray, out t_hit))
        {
            Vector3 targetPostition = new Vector3(t_hit.point.x, transform.localPosition.y, t_hit.point.z);
            transform.LookAt(targetPostition);
            yield return new WaitForSeconds(.1f);

            while (true)
            {
                transform.Translate(transform.forward / 2, Space.World);

                Ray t_ray = new Ray(transform.position, transform.forward);
                RaycastHit t_hitJump;
                if (Physics.Raycast(t_ray, out t_hitJump, 1.5f)) {
                    GetComponent<Rigidbody>().AddForce(transform.up * 2000);
                }

                yield return new WaitForSeconds(.01f);

                if ((int)transform.position.x == (int)t_hit.point.x && (int)transform.position.z == (int)t_hit.point.z) break;
                if (Input.GetKeyDown(KeyCode.Mouse0)) break;
            }
        }
    }
}