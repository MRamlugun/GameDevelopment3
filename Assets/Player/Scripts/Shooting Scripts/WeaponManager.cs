using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("FireRate")]
    [SerializeField] float fireRate;
    [SerializeField] bool semiAuto;
    float fireRateTimer;
    public float range = 100f;
    public float impactForce = 30f;
    public float damage = 5f;
    private float nextTimeToFire = 0f;
    [SerializeField] GameObject hitEffect;

    [Header("BulletProperties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    AimStateManager aim;

    public ParticleSystem muzzleFlash;

    #region SFX
    AudioSource audioSource;
    public AudioClip reload;
    public AudioClip fire;

    #endregion

    public Animator anim;


    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;


    // Start is called before the first frame update
    void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate;
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        
    }

    void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldFire()) Fire();

        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
    }

    bool ShouldFire()
    {
        
        // Check if the player is reloading; if so, don't allow firing
        if (isReloading)
        {
            return false;
        }



        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        PlayMuzzleFlash();
        audioSource.PlayOneShot(fire);
        currentAmmo--;
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        for (int i=0; i < bulletsPerShot; i++)
        {
            GameObject currentbullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentbullet.GetComponent<Bullet>();
            bulletScript.weapon = this;

            Rigidbody rb = currentbullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }

    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }


    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact =Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));       
        Destroy(impact, 0.1f);

        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        muzzleFlash.Stop();
        anim.SetBool("Reloading", true);
        audioSource.PlayOneShot(reload);
        yield return new WaitForSeconds(reloadTime - .25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(reloadTime - .25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        muzzleFlash.Play();
    }
}
