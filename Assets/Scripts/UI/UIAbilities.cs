using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiAbilityTemplate;

    [SerializeField]
    private Transform _parentUIAbilitiesContent;

    private void Start()
    {
        PlayerAbilitiesController abilitiesController = PlayerTracker.PlayerAbilitiesController;
        List<Ability> abilities = abilitiesController.GetAbilities();

        abilities.ForEach(c => CreateUIAbilityObject(c));
    }

    private void CreateUIAbilityObject(Ability ability)
    {
        if (ability)
        {
            GameObject objAbility = Instantiate(_uiAbilityTemplate, _parentUIAbilitiesContent);
            UIAbility controller = objAbility.GetComponent<UIAbility>();

            if (controller)
            {
                controller.Initialize(ability);
            }
            else
                Debug.LogError($"{GetType().FullName} : Failed to find controller.");
        }
    }
}
