using Validation;
using System.Linq;
using System.Collections.Generic;
using ExtensionMethods;

namespace Cricket.Player
{
    public class PlayerName : IValidity
    {
        public static bool IsNullOrEmpty(PlayerName name)
        {
            if (name != null)
            {
                if (!string.IsNullOrEmpty(name.Forename) && !string.IsNullOrEmpty(name.Surname))
                {
                    return false;
                }
            }

            return true;
        }

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

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var output = new List<ValidationResult>();
            output.AddIfNotNull(Validating.IsNotNullOrEmpty(Surname, nameof(Surname)));
            output.AddIfNotNull(Validating.IsNotNullOrEmpty(Forename, nameof(Forename)));
            return output;
        }
    }
}
