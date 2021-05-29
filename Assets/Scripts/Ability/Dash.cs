using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Dash : Ability
{
    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _maxDashDistance;

    [SerializeField]
    private float _dashSpeed;

    [SerializeField, Tooltip("This is only for debugging")]
    private Vector2 _dashDestination;
    public override void ActivateAbility()
    {
        base.ActivateAbility();

        if(_abilityData.IsAbilityActivated)
            StartCoroutine(DashToPoint());
    }

    protected override void PrePerform()
    {
        base.PrePerform();
        PlayerTracker.PlayerMovementController.EnableMovement(false);
        PlayerTracker.PlayerMovementController.EnableGravity(false);
    }

    protected override void PostPerform()
    {
        base.PostPerform();

        PlayerTracker.PlayerMovementController.EnableMovement(true);
        PlayerTracker.PlayerMovementController.EnableGravity(true);
    }
    private Vector2 GetDestination()
    {
        Vector2 mousePosition = GetMousePosition();

        Vector2 direction = new Vector2(mousePosition.x - gameObject.transform.position.x < 0 ? -1 : 1, gameObject.transform.position.y).normalized * Vector2.right;

        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, direction, _maxDashDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                return hits[i].point;
            }
        }

        Vector2 destination = new Vector2(gameObject.transform.position.x + direction.x * _maxDashDistance, gameObject.transform.position.y);
        return destination;
    }
    private IEnumerator DashToPoint()
    {
        WaitForFixedUpdate waitForEndOfFrame = new WaitForFixedUpdate();
        _dashDestination = GetDestination();

        Vector2 startPosition = gameObject.transform.position;
        float currentDistance = Vector2.Distance(startPosition, _dashDestination);

        while(currentDistance > 0.1f)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _dashDestination, _dashSpeed * Time.fixedDeltaTime);
            currentDistance = Vector2.Distance(gameObject.transform.position, _dashDestination);
            yield return waitForEndOfFrame;
        }

        PostPerform();
    }
    private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    //private void LateUpdate()
    //{
    //    Debug.LogError("Mouse Position : " + GetMousePosition());
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_abilityData.IsAbilityActivated)
        {
            Health otherHealth = collision.gameObject.GetComponent<Health>();
            if (otherHealth)
            {
                otherHealth.ModifyHealth(-_damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_abilityData.IsAbilityActivated)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gameObject.transform.position, _dashDestination);
        }
    }
}
