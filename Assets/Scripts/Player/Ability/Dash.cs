using System.Collections;
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

    [SerializeField]
    private float _minDistanceFromWall;

    [SerializeField, Tooltip("This is only for debugging")]
    private Vector3 _dashDestination;
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
    }

    protected override void PostPerform()
    {
        base.PostPerform();

        PlayerTracker.PlayerMovementController.EnableMovement(true);
    }
    private Vector3 GetDestination()
    {
        Vector3 mousePosition = GetMousePosition();
        mousePosition.y = 0;
        Vector3 direction = (mousePosition - gameObject.transform.position).normalized;

        int layerMask = LayerMask.GetMask("Walls");
        Ray ray = new Ray(gameObject.transform.position, direction);
        RaycastHit hit;
        Vector3 destination = Vector3.zero;
        if(Physics.Raycast(ray, out hit, _maxDashDistance, layerMask))
        {
            destination = hit.point + (gameObject.transform.position - hit.point).normalized * _minDistanceFromWall;
            destination.y = 0;
            return destination;
        }

        destination = gameObject.transform.position + direction * _maxDashDistance;
        destination.y = 0;
        return destination;
    }
    private IEnumerator DashToPoint()
    {
        WaitForFixedUpdate waitForEndOfFrame = new WaitForFixedUpdate();
        //Vector3 destination = GetDestination();
        _dashDestination = GetDestination();

        Vector3 startPosition = gameObject.transform.position;
        float currentDistance = Vector3.Distance(startPosition, _dashDestination);

        while(currentDistance > 0.1f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _dashDestination, _dashSpeed * Time.fixedDeltaTime);
            currentDistance = Vector3.Distance(gameObject.transform.position, _dashDestination);
            yield return waitForEndOfFrame;
        }

        PostPerform();
    }
    private Vector3 GetMousePosition() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    private void OnCollisionEnter(Collision collision)
    {
        if (_abilityData.IsAbilityActivated)
        {
            Debug.LogError("Enemy Detected");

            Health otherHealth = collision.gameObject.GetComponent<Health>();
            if (otherHealth)
            {
                Debug.LogError("Enemy Found");
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
        Gizmos.color = Color.red;
        //_dashDestination = GetDestination();
        //Gizmos.DrawCube(GetDestination(), Vector3.one * 2);
    }
}
