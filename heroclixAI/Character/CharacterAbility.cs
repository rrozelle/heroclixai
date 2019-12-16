using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    [Serializable]
    class CharacterAbility
    {
        public CharacterAbility()
        {
            Boolean _LightningSmash = false;
            Boolean _Charge = false; // thor and cap abilities
            Boolean _RunningShot = false; // thor and iron man
            Boolean _SideStep = false;// thor and iron man
            Boolean _SuperStrength = false; // thor and cap
            Boolean _EnergyExplosion = false; //thor
            Boolean _Impervious = false; //thor
            Boolean _Invulnerability = false; //thor and iron man
            Boolean _WillPower = false; //thor and iron man
            Boolean _Avengers = false; // iron man, cap, and thor
            Boolean _Deflection = false; //cap
            Boolean _CombatReflexes = false; //cap
            Boolean _Leadership = false; //cap
            Boolean _CloseCombatExpert = false; //cap
            Boolean _Toughness = false; //iron man
            Boolean _RangedCombatExpert = false; //iron man
        }

        public void ClearAbilites()
        {
            _LightningSmash = false;
            _Charge = false; // thor and cap abilities
            _RunningShot = false; // thor and iron man
            _SideStep = false;// thor and iron man
            _SuperStrength = false; // thor and cap
            _EnergyExplosion = false; //thor
            _Impervious = false; //thor
            _Invulnerability = false; //thor and iron man
            _WillPower = false; //thor and iron man
            _Avengers = false; // iron man, cap, and thor
            _Deflection = false; //cap
            _CombatReflexes = false; //cap
            _Leadership = false; //cap
            _CloseCombatExpert = false; //cap
            _Toughness = false; //iron man
            _RangedCombatExpert = false; //iron man
        }

        public Boolean _LightningSmash { get; set; }
        public Boolean _Charge { get; set; }
        public Boolean _RunningShot { get; set; }
        public Boolean _SideStep { get; set; }
        public Boolean _SuperStrength { get; set; }
        public Boolean _EnergyExplosion { get; set; }
        public Boolean _Impervious { get; set; }
        public Boolean _Invulnerability { get; set; }
        public Boolean _WillPower { get; set; }
        public Boolean _Avengers { get; set; }
        public Boolean _Deflection { get; set; }
        public Boolean _CombatReflexes { get; set; }
        public Boolean _Leadership { get; set; }
        public Boolean _CloseCombatExpert { get; set; }
        public Boolean _Toughness { get; set; }
        public Boolean _RangedCombatExpert { get; set; }


    }
}
