using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
	[Header("References")]
    [SerializeField] Health health;
    [SerializeField] Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        if(IsClient == false)
            return;

        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient == false)
            return;

        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        healthBarImage.fillAmount = newHealth / (float)health.MaxHealth;
    }
}
