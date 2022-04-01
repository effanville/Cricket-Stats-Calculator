using System.Xml.Serialization;
using Common.Structure.NamingStructures;

namespace CricketStructures.Player
{
    public class PlayerName : Name
    {
        [XmlAttribute(AttributeName = "F")]
        public string Forename
        {
            get
            {
                return SecondaryName;
            }
            set
            {
                SecondaryName = value;
            }
        }

        [XmlAttribute(AttributeName = "S")]
        public string Surname
        {
            get
            {
                return PrimaryName;
            }
            set
            {
                PrimaryName = value;
            }
        }

        public PlayerName(string surname, string forename)
            : base(surname, forename)
        {
        }

        private PlayerName(PlayerName otherName)
        {
            Surname = otherName.Surname;
            Forename = otherName.Forename;
        }

        public PlayerName()
        {
        }

        public override string ToString()
        {
            return Forename + " " + Surname;
        }


        public new PlayerName Copy()
        {
            return new PlayerName(this);
        }
    }
}
