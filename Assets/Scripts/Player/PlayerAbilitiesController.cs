using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    [SerializeField]
    private List<Ability> _abilities = new List<Ability>();

    private void Start()
    {
        PlayerInputController playerInputController = PlayerTracker.PlayerInputController;
        playerInputController.SubscribeToOnAbility1(PlayerInputController_Ability1Activated);
        playerInputController.SubscribeToOnAbility2(PlayerInputController_Ability2Activated);
    }

    private void PlayerInputController_Ability1Activated()
    {
        if (_abilities.Count >= 1)
            _abilities[0].ActivateAbility();
    }
    private void PlayerInputController_Ability2Activated()
    {
        if (_abilities.Count >= 2)
            _abilities[1].ActivateAbility();
    }

    public List<Ability> GetAbilities() => _abilities;
}
