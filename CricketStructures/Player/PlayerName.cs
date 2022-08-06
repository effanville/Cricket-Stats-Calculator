using System.Xml.Serialization;
using Common.Structure.NamingStructures;

namespace CricketStructures.Player
{
    public class PlayerName : Name
    {
        [XmlAttribute(AttributeName = "F")]
        public string Forename
        {
            get => SecondaryName;
            set => SecondaryName = value;
        }

        [XmlAttribute(AttributeName = "S")]
        public string Surname
        {
            get => PrimaryName;
            set => PrimaryName = value;
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

        private static readonly string StringSeparator = " ";
        public static PlayerName FromString(string playerNameAsString)
        {
            string[] splitted = playerNameAsString.Split(StringSeparator);
            if (splitted.Length == 2)
            {
                return new PlayerName(splitted[1], splitted[0]);
            }
            if (splitted.Length == 1)
            {
                return new PlayerName(splitted[0], "");
            }

            return new PlayerName();
        }

        public override string ToString()
        {
            return $"{Forename}{StringSeparator}{Surname}";
        }

        public new PlayerName Copy()
        {
            return new PlayerName(this);
        }
    }
}
