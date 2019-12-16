using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    [Serializable]
    class Character
    {
        public Character()
        {
            string _CharacterName = "";
            int _CurrentClick = 1;
            int _TotalClicks = 0;
            int _ActionToken = 0;
            int threatLevel = 0;
            Boolean _LightWeapon = false;
            Boolean _HeavyWeapon = false;
            Boolean SpecialAbilityUsed = false;

            //Need to add one null at the beginning to line up the clicks with the index number
            abilities.Add(null);
            clicks.Add(null);
        }

        public List<CharacterAbility> abilities = new List<CharacterAbility>();
        public List<CharacterClick> clicks = new List<CharacterClick>();

        public Boolean SpecialAbilityUsed { get; set; }
        public int threatLevel { get; set; }
        public int _ActionToken { get; set; }
        public int _CurrentClick { get; set; }
        public Boolean _HeavyWeapon { get; set; }

        public Boolean _LightWeapon { get; set; }

        public int _TotalClicks { get; set; }

        public string _CharacterName { get; set; }
        public int UseMeleeWeaponDamage()
        {
            int mAttackDamage;

            if (_LightWeapon == true)
            {
                mAttackDamage = clicks[_CurrentClick].AttackValue + 1;
            }
            else if (_HeavyWeapon == true)
            {
                mAttackDamage = clicks[_CurrentClick].AttackValue + 2;
            }
            else
            {
                mAttackDamage = clicks[_CurrentClick].AttackValue;
            }

            return mAttackDamage;
        }

        public int UseRangeWeaponDamage()
        {
            int rAttackDamage;

            if (_LightWeapon == true)
            {
                rAttackDamage = clicks[_CurrentClick].AttackValue + 2;
            }
            else if (_HeavyWeapon == true)
            {
                rAttackDamage = clicks[_CurrentClick].AttackValue + 3;
            }
            else
            {
                rAttackDamage = clicks[_CurrentClick].AttackValue;
            }

            return rAttackDamage;
        }

        public void AddClick(int clickNumber, int speedValue, int attackValue, int defenseValue, int damageValue, int rangeValue, CharacterAbility chability)
        {
            //Create a CharacterClick class
            CharacterClick Assign = new CharacterClick();
            CharacterAbility Transfer = new CharacterAbility();

            //Assign the values to both classes
            Assign.SpeedValue = speedValue;
            Assign.AttackValue = attackValue;
            Assign.DefenseValue = defenseValue;
            Assign.DamageValue = damageValue;
            Assign.RangeValue = rangeValue;

            Transfer._LightningSmash = chability._LightningSmash;
            Transfer._Charge = chability._Charge;
            Transfer._RunningShot = chability._RunningShot;
            Transfer._SideStep = chability._SideStep;
            Transfer._SuperStrength = chability._SuperStrength;
            Transfer._EnergyExplosion = chability._EnergyExplosion;
            Transfer._Impervious = chability._Impervious;
            Transfer._Invulnerability = chability._Invulnerability;
            Transfer._WillPower = chability._WillPower;
            Transfer._Avengers = chability._Avengers;
            Transfer._Deflection = chability._Deflection;
            Transfer._CombatReflexes = chability._CombatReflexes;
            Transfer._Leadership = chability._Leadership;
            Transfer._CloseCombatExpert = chability._CloseCombatExpert;
            Transfer._Toughness = chability._CloseCombatExpert;
            Transfer._RangedCombatExpert = chability._RangedCombatExpert;

            //Then input it into the clicks array which accepts only CharacterClick class objects
            clicks.Add(Assign);

            //Assign the CharacterAbility class ability to the Character's ability array
            abilities.Add(Transfer);            

            //Increment the _TotalClicks
            _TotalClicks = _TotalClicks + 1;



        }

        public void AddActionToken()
        {
            _ActionToken = _ActionToken + 1;
            if (abilities[_CurrentClick]._WillPower)
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
