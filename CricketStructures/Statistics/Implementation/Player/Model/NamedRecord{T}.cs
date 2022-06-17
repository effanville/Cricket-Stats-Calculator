using System;

using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    public sealed class NamedRecord<T>
    {
        private readonly Func<T, T, T> fAggregation;
        private readonly string fRecordName;
        public PlayerName Name
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            private set;
        }

        public NamedRecord(string recordName, PlayerName name, T value, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Value = value;
        }

        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        public string RecordName()
        {
            return fRecordName;
        }

        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Value}";
        }
    }
}
