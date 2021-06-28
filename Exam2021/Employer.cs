using System;

namespace Exam2021
{
    public class Employer
    {
        public Int32 Number { get; set; }
        public String Name { get; set; }
        public DateTime BirthTime { get; set; }
        public Place Work { get; set; }

        public Employer(Int32 number, String name, DateTime birthTime, Place work)
        {
            Number = number;
            Name = name;
            BirthTime = birthTime;
            Work = work;
        }
    }

    public enum Place : Int32
    {
        Работник,
        CEO
    }
}
