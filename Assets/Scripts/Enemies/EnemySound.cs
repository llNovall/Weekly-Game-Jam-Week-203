using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : SoundPlayer
{
    [SerializeField]
    private string _eventTakeDamage, _eventDash;
    public void Initialize()
    {

    }

    public void PlayTakeDamageSound()
    {
        PlaySound(_eventTakeDamage);
    }

    public void PlayDashSound()
    {
        PlaySound(_eventDash);
    }
}
