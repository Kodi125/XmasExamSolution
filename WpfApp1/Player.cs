using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    //Enum to be used with Player position
    public enum Position { Goalkeeper, Defender, Midfielder, Forward }

    public class Player
    {
        #region Properties

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Position PreferredPosition { get; set; }
        public DateTime DateOfBirth { get; set; }

        private int age;

        public int Age
        {
            get
            {
                //Determine age and account for birthday that has not yet occured
                age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateOfBirth.DayOfYear >= DateTime.Now.DayOfYear)
                    age--;
                return age;
            }

        }


        #endregion Properties

        public override string ToString()
        {
            return $"{FirstName} {Surname} ({Age}) {PreferredPosition.ToString().ToUpper()} ";
        }

        
    }
}
