using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;          
    public Slider slider;                        
    public Image fillImage;                      
    public Color fullHealthColor = Color.green;  
    //public Color warningHealthColor = Color.yellow;  
    public Color zeroHealthColor = Color.red;    
    public GameObject explosionPrefab;
    

    private AudioSource explosionAudio;          
    private ParticleSystem explosionParticles;   
    private float currentHealth;  
    private bool dead;            


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();

        explosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        currentHealth = startingHealth;
        dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        SetHealthUI();
        
        if (currentHealth <= 0f && !dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        slider.value = currentHealth;

        /*if (slider.value >= 51f)
        {
            fillImage.color = fullHealthColor;
        }
        else if (slider.value <= 50f)
        {
            fillImage.color = warningHealthColor;
        }
        else if (slider.value <= 20f)
        {
            fillImage.color = zeroHealthColor;
        }*/

        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
    }


    private void OnDeath()
    {
        dead = true;

        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);

        explosionParticles.Play();
        explosionAudio.Play();

        gameObject.SetActive(false);
    }
}