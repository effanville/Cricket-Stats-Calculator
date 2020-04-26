namespace Cricket.Player
{
    public class PlayerName
    {
        public PlayerName(string surname, string forename)
        {
            Surname = surname;
            Forename = forename;
        }

        private PlayerName(PlayerName otherName)
        {
            Surname = otherName.Surname;
            Forename = otherName.Forename;
        }
        public PlayerName()
        {
        }

        private string fForename;
        public string Forename
        {
            get { return fForename; }
            set { fForename = value; }
        }

        private string fSurname;
        public string Surname
        {
            get { return fSurname; }
            set { fSurname = value; }
        }

        public override string ToString()
        {
            return Forename + " " + Surname;
        }

        public bool EditName(string surname, string forename)
        {
            Surname = surname;
            Forename = forename;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is PlayerName otherName)
            {
                if (otherName == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(Forename))
                {
                    if (string.IsNullOrEmpty(otherName.Surname) && string.IsNullOrEmpty(otherName.Forename))
                    {
                        return true;
                    }

                    return false;
                }
                if (string.IsNullOrEmpty(Surname))
                {
                    if (string.IsNullOrEmpty(otherName.Surname))
                    {
                        return Forename.Equals(otherName.Forename);
                    }

                    return false;
                }

                if (string.IsNullOrEmpty(Forename))
                {
                    if (string.IsNullOrEmpty(otherName.Forename))
                    {
                        return Surname.Equals(otherName.Surname);
                    }

                    return false;
                }

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

        public PlayerName Copy()
        {
            return new PlayerName(this);
        }
    }
}
