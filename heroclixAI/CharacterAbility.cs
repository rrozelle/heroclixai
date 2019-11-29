using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    class CharacterAbility
    {
        private bool _LightningSmash = false;
        private bool _Charge = false; // thor and cap abilities
        private bool _RunningShot = false; // thor and iron man
        private bool _SideStep = false;// thor and iron man
        private bool _SuperStrength = false; // thor and cap
        private bool _EnergyExplosion = false; //thor
        private bool _Impervious = false; //thor
        private bool _Invulnerability = false; //thor and iron man
        private bool _WillPower = false; //thor and iron man
        private bool _Avengers = false; // iron man, cap, and thor
        private bool _Deflection = false; //cap
        private bool _CombatReflexes = false; //cap
        private bool _Leadership = false; //cap
        private bool _CloseCombatExpert = false; //cap
        private bool _Toughness = false; //iron man
        private bool _RangedCombatExpert = false; //iron man

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

        public bool WillPower
        {
            get
            {
                return _WillPower;
            }
            set
            {
                _WillPower = value;
            }
        }

        public bool LightningSmash
        {
            get
            {
                return _LightningSmash;
            }
            set
            {
                _LightningSmash = value;
            }
        }
        public bool Charge
        {
            get
            {
                return _Charge;
            }
            set
            {
                _Charge = value;
            }
        }
        public bool RunningShot
        {
            get
            {
                return _RunningShot;
            }
            set
            {
                _RunningShot = value;
            }
        }

        public bool SideStep
        {
            get
            {
                return _SideStep;
            }
            set
            {
                _SideStep = value;
            }
        }

        public bool SuperStrength
        {
            get
            {
                return _SuperStrength;
            }
            set
            {
                _SuperStrength = value;
            }
        }
        public bool EnergyExplosion
        {
            get
            {
                return _EnergyExplosion;
            }
            set
            {
                _EnergyExplosion = value;
            }
        }

        public bool Impervious
        {
            get
            {
                return _Impervious;
            }
            set
            {
                _Impervious = value;
            }
        }

        public bool Invulnerability
        {
            get
            {
                return _Invulnerability;
            }
            set
            {
                _Invulnerability = value;
            }
        }

        public bool Avengers
        {
            get
            {
                return _Avengers;
            }
            set
            {
                _Avengers = value;
            }
        }

        public bool Deflection
        {
            get
            {
                return _Deflection;
            }
            set
            {
                _Deflection = value;
            }
        }

        public bool CombatReflexes
        {
            get
            {
                return _CombatReflexes;
            }
            set
            {
                _CombatReflexes = value;
            }
        }

        public bool Leadership
        {
            get
            {
                return _Leadership;
            }
            set
            {
                _Leadership = value;
            }
        }

        public bool CloseCombatExpert
        {
            get
            {
                return _CloseCombatExpert;
            }
            set
            {
                _CloseCombatExpert = value;
            }
        }

        public bool Toughness
        {
            get
            {
                return _Toughness;
            }
            set
            {
                _Toughness = value;
            }
        }
        public bool RangedCombatExpert
        {
            get
            {
                return _RangedCombatExpert;
            }
            set
            {
                _RangedCombatExpert = value;
            }
        }







    }
}
