using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : SoundPlayer
{
    [SerializeField]
    private string _evtDash, _evtTakeHit, _evtChomp;
    public void PlayDashSound() => PlaySound(_evtDash);
    public void PlayTakeHitSound() => PlaySound(_evtTakeHit);
    public void PlayChompSound() => PlaySound(_evtChomp);
}
