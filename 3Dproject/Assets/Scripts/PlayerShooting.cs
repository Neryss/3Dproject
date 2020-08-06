using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float projSpeed = 20000;
    private Rigidbody bulletRb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Shoot(bulletPrefab);
    }

    private void Shoot(GameObject prefab)
    {
        GameObject bulletInstance = Instantiate(prefab, firePoint.position, Quaternion.identity);
        bulletRb = bulletInstance.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * Time.deltaTime * projSpeed, ForceMode.Impulse);
    }
}
