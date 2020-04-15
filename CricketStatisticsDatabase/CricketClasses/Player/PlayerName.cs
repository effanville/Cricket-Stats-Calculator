namespace Cricket.Player
{
    public class PlayerName
    {
        public PlayerName(string surname, string forename)
        {
            Surname = surname;
            Forename = forename;
        }

        private string fForename;
        public string Forename
        {
            get { return fForename; }
            private set { fForename = value; }
        }

        private string fSurname;
        public string Surname
        {
            get { return fSurname; }
            private set { fSurname = value; }
        }

        public override string ToString()
        {
            return Forename + " " + Surname;
        }

        public override bool Equals(object obj)
        {
            if (obj is PlayerName otherName)
            {
                if (Surname.Equals(otherName.Surname) && Forename.Equals(otherName.Forename))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Surname.GetHashCode() + 10^12 * Forename.GetHashCode();
        }
    }
}
