using System;

using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    public sealed class NameDatedRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;
        public PlayerName Name
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            private set;
        }

        public NameDatedRecord(string recordName, PlayerName name, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Date = start;
            Value = firstValue;
        }

        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Date}-{Value}";
        }
    }
}
