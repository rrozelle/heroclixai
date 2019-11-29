using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    class Character
    {
        private string _CharacterName = "";
        private int _CurrentClick = 1;
        private int _TotalClicks = 0;
        private int _ActionToken = 0;
        private Boolean _LightWeapon = false;
        private Boolean _HeavyWeapon = false;

        public CharacterClick[] clicks = new CharacterClick[12];
        public CharacterAbility[] abilities = new CharacterAbility[12];


        public Boolean _heavyWeapon { get; set; }

        public Boolean _lightWeapon { get; set; }

        public int _totalClicks { get; set; }

        public string _characterName { get; set; }
        public int UseMeleeWeapon(int mAttackDamage)
        {
            if (_LightWeapon == true)
            {
                mAttackDamage = clicks[_CurrentClick].AttackValue + 1;
            }

            if (_HeavyWeapon == true)
            {
                mAttackDamage = clicks[_CurrentClick].AttackValue + 2;
            }

            return mAttackDamage;
        }

        public int UseRangeWeapon(int rAttackDamage)
        {
            if (_LightWeapon == true)
            {
                rAttackDamage = clicks[_CurrentClick].AttackValue + 2;
            }

            if (_HeavyWeapon == true)
            {
                rAttackDamage = clicks[_CurrentClick].AttackValue + 3;
            }

            return rAttackDamage;
        }

        public void AddClick(int clickNumber, int speedValue, int attackValue, int defenseValue, int damageValue, int rangeValue, CharacterAbility ability)
        {
            //Create a CharacterClick class
            CharacterClick Assign = new CharacterClick();

            //Assign the values to that class
            Assign.SpeedValue = speedValue;
            Assign.AttackValue = attackValue;
            Assign.DefenseValue = defenseValue;
            Assign.DamageValue = damageValue;
            Assign.RangeValue = rangeValue;

            //Then input it into the clicks array which accepts only CharacterClick class objects
            clicks[clickNumber] = Assign;

            //Create a CharacterAbility class
            CharacterAbility Transfer = new CharacterAbility();

            //Transfer the data from ability argument to the transfer CharacterAbility class
            Transfer.LightningSmash = ability.LightningSmash;
            Transfer.Charge = ability.Charge;
            Transfer.RunningShot = ability.RunningShot;
            Transfer.SideStep = ability.SideStep;
            Transfer.SuperStrength = ability.SuperStrength;
            Transfer.EnergyExplosion = ability.EnergyExplosion;
            Transfer.Impervious = ability.Impervious;
            Transfer.Invulnerability = ability.Invulnerability;
            Transfer.WillPower = ability.WillPower;
            Transfer.Avengers = ability.Avengers;
            Transfer.Deflection = ability.Deflection;
            Transfer.CombatReflexes = ability.CombatReflexes;
            Transfer.Leadership = ability.Leadership;
            Transfer.CloseCombatExpert = ability.CloseCombatExpert;
            Transfer.Toughness = ability.Toughness;
            Transfer.RangedCombatExpert = ability.RangedCombatExpert;

            //Assign the CharacterAbility class ability to the Character's ability array
            abilities[clickNumber] = Transfer;

            //Increment the _TotalClicks
            _totalClicks = _totalClicks + 1;



        }

        public void AddActionToken()
        {
            _ActionToken = _ActionToken + 1;
            if (abilities[_CurrentClick].WillPower)
            {
                return;
            }
            if (_ActionToken >= 2)
            {
                TakeDamage(1);
            }
        }

        public void ClearActionTokens()
        {
            _ActionToken = 0;
        }

        public void TakeDamage(int damage)
        {
            _CurrentClick = _CurrentClick + damage;
        }

        public bool IsKnockedOut()
        {
            if (_CurrentClick > _TotalClicks)
            {
                return true;
            }

            return false;
        }

    }
}
