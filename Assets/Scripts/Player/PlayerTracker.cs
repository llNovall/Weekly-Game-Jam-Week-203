using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public static PlayerTracker Current;
    public static Health PlayerHealth;
    public static PlayerMovementController PlayerMovementController;
    public static PlayerInputController PlayerInputController;
    public static PlayerAbilitiesController PlayerAbilitiesController;
    public static PlayerSound PlayerSound;
    private void Awake()
    {
        Current = this;
        PlayerHealth = gameObject.GetComponent<Health>();
        PlayerMovementController = gameObject.GetComponent<PlayerMovementController>();
        PlayerInputController = gameObject.GetComponent<PlayerInputController>();
        PlayerAbilitiesController = gameObject.GetComponent<PlayerAbilitiesController>();
        PlayerSound = gameObject.GetComponent<PlayerSound>();
    }
}
