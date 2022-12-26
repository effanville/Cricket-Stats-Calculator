using System;
using System.Xml.Serialization;

namespace CricketStructures.Match.Innings
{
    public struct Over : IComparable
        , IComparable<Over>, IEquatable<Over>
    {
        public static readonly int NumberOverBalls = 6;
        public static readonly Over Min = new Over(0, 0);

        [XmlAttribute(AttributeName = "Overs")]
        public int NumberOvers;

        [XmlAttribute(AttributeName = "Balls")]
        public int NumberBalls;

        public Over(int wholeOvers, int numberBalls)
        {
            NumberOvers = wholeOvers;
            NumberBalls = numberBalls;
        }

        public Over(int wholeOvers)
        {
            NumberOvers = wholeOvers;
            NumberBalls = 0;
        }

        public static Over FromString(string serialised)
        {
            if (serialised.Contains('.'))
            {
                string[] parts = serialised.Split('.');
                int wholeOvers = string.IsNullOrWhiteSpace(parts[0]) ? 0 : int.Parse(parts[0]);
                int numberBalls = int.Parse(parts[1]);
                return new Over(wholeOvers, numberBalls);
            }
            else
            {
                int wholeOvers = int.Parse(serialised);
                return new Over(wholeOvers);
            }
        }

        public override string ToString()
        {
            if (NumberBalls == 0)
            {
                return $"{NumberOvers}";
            }

            return $"{NumberOvers}.{NumberBalls}";
        }

        public int CompareTo(object obj)
        {
            if (obj is Over otherOver)
            {
                return CompareTo(otherOver);
            }

            return 0;
        }

        public int CompareTo(Over other)
        {
            if (NumberOvers == other.NumberOvers)
            {
                return NumberBalls.CompareTo(other.NumberBalls);
            }

            return NumberOvers.CompareTo(other.NumberOvers);
        }

        public bool Equals(Over other)
        {
            return NumberOvers.Equals(other.NumberOvers)
                && NumberBalls.Equals(other.NumberBalls);
        }

        public override bool Equals(object obj)
        {
            return obj is Over over && Equals(over);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NumberOvers, NumberBalls);
        }

        public static bool operator ==(Over left, Over right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Over left, Over right)
        {
            return !(left == right);
        }

        public static bool operator <(Over left, Over right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Over left, Over right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Over left, Over right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Over left, Over right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static Over operator +(Over left, Over right)
        {
            int extraOvers = (left.NumberBalls + right.NumberBalls) / NumberOverBalls;
            int extraBalls = (left.NumberBalls + right.NumberBalls) % NumberOverBalls;
            int numberOvers = left.NumberOvers + right.NumberOvers + extraOvers;
            return new Over(numberOvers, extraBalls);
        }

        /*public static implicit operator double(Over over)
        {
            return over.NumberOvers + (double)over.NumberBalls / NumberOverBalls;
        }*/

        public static implicit operator Over(int overs)
        {
            return new Over(overs);
        }

        public static explicit operator double(Over over)
        {
            return over.NumberOvers + (double)over.NumberBalls / NumberOverBalls;
        }

        public static explicit operator Over(string value)
        {
            return Over.FromString(value);
        }

        public static explicit operator Over(double overs)
        {
            double numberBalls = overs * 6;
            double wholeOvers = Math.Floor(overs);
            int wholeOversInt = Convert.ToInt32(wholeOvers);
            double numberBallPart = overs - wholeOvers;
            double stuff = numberBallPart * 6;
            double nearestNumBalls = Math.Round(stuff);
            int nearestNumBallsInt = Convert.ToInt32(nearestNumBalls);
            return new Over(wholeOversInt, nearestNumBallsInt);
        }
    }
}
