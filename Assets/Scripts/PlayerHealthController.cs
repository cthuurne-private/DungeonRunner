using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float _invincCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        UIController.Instance.healthSlider.maxValue = maxHealth;
        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void Update()
    {
        if (_invincCount > 0)
        {
            _invincCount -= Time.deltaTime;

            if (_invincCount <= 0)
            {
                PlayerController.Instance.bodySpriteRenderer.color = new Color(PlayerController.Instance.bodySpriteRenderer.color.r,
                    PlayerController.Instance.bodySpriteRenderer.color.g, PlayerController.Instance.bodySpriteRenderer.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (_invincCount <= 0)
        {
            currentHealth--;

            AudioManager.Instance.PlaySFX(11);

            _invincCount = damageInvincLength;

            PlayerController.Instance.bodySpriteRenderer.color = new Color(PlayerController.Instance.bodySpriteRenderer.color.r,
                PlayerController.Instance.bodySpriteRenderer.color.g, PlayerController.Instance.bodySpriteRenderer.color.b, 0.5f);

            if (currentHealth <= 0)
            {
                AudioManager.Instance.PlaySFX(9);

                PlayerController.Instance.gameObject.SetActive(false);

                UIController.Instance.deathScreen.SetActive(true);

                AudioManager.Instance.PlayGameOver();
            }

            UIController.Instance.healthSlider.value = currentHealth;
            UIController.Instance.healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void MakeInvincible(float invincibleTime)
    {
        _invincCount = invincibleTime;
        PlayerController.Instance.bodySpriteRenderer.color = new Color(PlayerController.Instance.bodySpriteRenderer.color.r,
            PlayerController.Instance.bodySpriteRenderer.color.g, PlayerController.Instance.bodySpriteRenderer.color.b, 0.5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = $"{currentHealth} / {maxHealth}";
    }
}
