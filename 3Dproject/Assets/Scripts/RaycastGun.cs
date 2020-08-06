using UnityEngine;

public class RaycastGun : MonoBehaviour
{
	public ParticleSystem muzzleFlash;
	public Camera fpsCam;
	float damage = 50f;
	float range = 100f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        	Shoot();
    }

    void Shoot()
	{
		muzzleFlash.Play();
		RaycastHit hit;
		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			Debug.Log(hit.transform.name);
			TargetScript targ = hit.transform.GetComponent<TargetScript>();
			if(targ != null)
				targ.TakeDamage(damage);
		}
	}
}