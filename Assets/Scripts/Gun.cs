using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public int damage = 10;
    public float rayDistance = 100f;
    public float rayDuration = 0.1f;
    public LineRenderer lineRendererPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Vector3 rayEnd;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            AIHandler comp = hit.collider.GetComponent<AIHandler>();
            if (comp)
            {
                comp.DealDamage(damage);
                Debug.Log("Dealt damage! D:");
            }
            rayEnd = hit.point;
        }
        else
        {
            rayEnd = transform.position + transform.forward * rayDistance;
        }

        StartCoroutine(ShowLaser(transform.position, rayEnd));
    }

    IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        LineRenderer laser = Instantiate(lineRendererPrefab, start, Quaternion.identity);
        laser.SetPosition(0, start);
        laser.SetPosition(1, end);

        yield return new WaitForSeconds(rayDuration);

        Destroy(laser.gameObject);
    }
}
