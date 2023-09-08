using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DA{

    public class CharacterSoundFXManager : MonoBehaviour
    {
        //Attacking Grunts

        //Taking Damage

        //Taking Damage Sounds
        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private List<AudioClip> potentioalDamageSounds;
        private AudioClip lastDamageSoundPlayed;



        //Foot step sounds

        public void PlayRandomDamageSOundFX()
        {
            potentioalDamageSounds= newList<AudioClip>();
            foreach (var (damageSound) in takingDamageSounds)
            {
                
            }
        }
    }

}

