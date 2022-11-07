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
            if (string.IsNullOrWhiteSpace(playerNameAsString))
            {
                return null;
            }

            int forenameEndIndex = playerNameAsString.IndexOf(StringSeparator);
            if (forenameEndIndex > 0)
            {
                string forename = playerNameAsString.Substring(0, forenameEndIndex);
                string surname = playerNameAsString.Substring(forenameEndIndex);
                return new PlayerName(surname, forename);
            }
            if (forenameEndIndex == -1)
            {
                return new PlayerName(playerNameAsString, "");
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
