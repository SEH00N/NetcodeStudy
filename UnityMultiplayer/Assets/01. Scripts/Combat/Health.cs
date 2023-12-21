using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;

    public int MaxHealth => maxHealth;
    // NetworkVariable => 연동이 되는 변수. 서버만 건드릴 수 있도록 하자.
	public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    
    private bool isDead;
    public event Action<Health> OnDieEvent;

    public override void OnNetworkSpawn()
    {
        // IsServer => 현재 클라이언트가 서버인지 즉 호스트인지 확인하는 함수
        if(IsServer == false)
            return;

        CurrentHealth.Value = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void RestoreHealth(int heal)
    {
        ModifyHealth(heal);
    }

    private void ModifyHealth(int value)
    {
        // if(IsServer == false)
        //     return;

        if(isDead)
            return;
        
        CurrentHealth.Value += value;
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0, maxHealth);

        if(CurrentHealth.Value <= 0)
        {
            isDead = true;
            OnDieEvent?.Invoke(this);
        }
    }
}
