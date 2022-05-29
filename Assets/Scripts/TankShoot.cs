using UnityEngine;
using UnityEngine.UI;

public class TankShoot : MonoBehaviour
{
    public int playerNumber = 1;
    public Rigidbody bullet;
    public Transform fireOrigin;
    public Slider aimSlider;
    public AudioSource shootAudioSource;
    public AudioClip chargingAudio;
    public AudioClip fireAudio;
    public float minLaunchForce = 15f;
    public float maxLaunchForce = 30f;
    public float maxChargeTime = 0.75f;

    private string fireButton;
    private float currentLaunchForce;
    private float chargeSpeed;
    private bool fired;


    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }


    private void Start()
    {
        fireButton = "Fire" + playerNumber;

        //v=d/t aunque no sean distancias las fuerzas representan a que distancia caera la bala asi que al restarlas da la distancia que recorrera la bala al ser disparada al maximo
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }


    private void Update()
    {
        aimSlider.value = minLaunchForce;

        if (Input.GetButtonDown(fireButton))
        {
            fired = false;
            currentLaunchForce = minLaunchForce;

            shootAudioSource.clip = chargingAudio;
            shootAudioSource.Play();
        }

        if (Input.GetButton(fireButton) && !fired)
        {
            //Va aumentando la fuerza del disparo
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            aimSlider.value = currentLaunchForce;
        }

        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            //Estas dos instrucciones indican que al llegar al maximo de fuerza de lanzamiento se disparara en automatico
            currentLaunchForce = maxLaunchForce;
            Fire();
        }

        if (Input.GetButtonUp(fireButton) && !fired)
        {
            Fire();
        }
    }


    private void Fire()
    {
        fired = true;

        Rigidbody bulletInstance = Instantiate(bullet, fireOrigin.position, fireOrigin.rotation);

        //velocity es vector3 asi que para calcularla es con fuerza x direccion, no como la formula normal v=d/t
        bulletInstance.velocity = currentLaunchForce * fireOrigin.forward;

        shootAudioSource.clip = fireAudio;
        shootAudioSource.Play();

        currentLaunchForce = minLaunchForce;
    }
}