using System;

using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    public sealed class NameDurationRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;
        public PlayerName Name
        {
            get;
            private set;
        }

        public DateTime Start
        {
            get;
            private set;
        }

        public DateTime End
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            private set;
        }

        public NameDurationRecord(string recordName, PlayerName name, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Start = start;
            End = start;
            Value = firstValue;
        }

        public void UpdateValue(DateTime newDate, T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
            End = End < newDate ? newDate : End;
            Start = Start > newDate ? newDate : Start;
        }

        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Start}-{End}-{Value}";
        }
    }
}
