using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    [Serializable]
    class CharacterClick
    {
        private int _SpeedValue = 0;
        private int _AttackValue = 0;
        private int _DefenseValue = 0;
        private int _DamageValue = 0;
        private int _RangeValue = 0;


        public int SpeedValue
        {
            get
            {
                return _SpeedValue;
            }
            set
            {
                _SpeedValue = value;
            }
        }

        public int AttackValue
        {

            get
            {
                return _AttackValue;
            }
            set
            {
                _AttackValue = value;
            }
        }
        public int DefenseValue
        {

            get
            {
                return _DefenseValue;
            }
            set
            {
                _DefenseValue = value;
            }
        }
        public int DamageValue
        {

            get
            {
                return _DamageValue;
            }
            set
            {
                _DamageValue = value;
            }
        }
        public int RangeValue
        {

            get
            {
                return _RangeValue;
            }
            set
            {
                _RangeValue = value;
            }
        }







    }
}
