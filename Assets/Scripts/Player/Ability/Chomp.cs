using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chomp : Ability
{
    [SerializeField]
    private float _biteRange;

    [SerializeField]
    private float _attackDuration;

    [SerializeField]
    private float _currentDuration;

    [SerializeField]
    private float _attackDamage;

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        if (_abilityData.IsAbilityActivated)
            StartCoroutine(AttemptToEatEnemy());
    }
    protected override void PrePerform()
    {
        base.PrePerform();
    }

    protected override void PostPerform()
    {
        base.PostPerform();
    }

    private IEnumerator AttemptToEatEnemy()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        _currentDuration = 0;

        while(_currentDuration < _attackDuration)
        {
            _currentDuration += Time.deltaTime;
            yield return waitForEndOfFrame;
        }

        EnemyHealth enemyHealth = GetEnemyInFront();

        if (enemyHealth)
        {
            enemyHealth.ReduceHealth(_attackDamage);
        }

        PostPerform();
    }

    private EnemyHealth GetEnemyInFront()
    {
        Vector3 mousePosition = GetMousePosition();
        mousePosition.y = 0;
        Vector3 direction = (mousePosition - gameObject.transform.position).normalized;

        RaycastHit[] hits = Physics.RaycastAll(gameObject.transform.position, direction, _biteRange);

        if(hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                EnemyHealth enemyHealth = hits[i].collider.GetComponent<EnemyHealth>();
                if (enemyHealth)
                    return enemyHealth;
            }
        }

        return null;
    }

     private Vector3 GetMousePosition() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 mousePosition = GetMousePosition();
        mousePosition.y = 0;
        Vector3 direction = (mousePosition - gameObject.transform.position).normalized;
        Gizmos.DrawRay(gameObject.transform.position, direction);
    }
}
