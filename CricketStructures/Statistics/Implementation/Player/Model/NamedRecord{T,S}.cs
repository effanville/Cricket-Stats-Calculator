using System;

using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    public sealed class NamedRecord<T, S>
    {
        private readonly Func<T, T, T> fTAggregation;
        private readonly Func<S, S, S> fSAggregation;
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

        public S SecondValue
        {
            get;
            private set;
        }

        public NamedRecord(
            string recordName,
            PlayerName name,
            T value,
            S secondValue,
            Func<T, T, T> TAggregation,
            Func<S, S, S> SAggregation)
        {
            fRecordName = recordName;
            fTAggregation = TAggregation;
            fSAggregation = SAggregation;
            Name = name;
            Value = value;
            SecondValue = secondValue;
        }
        public NamedRecord(
            string recordName,
            PlayerName name,
            T value,
            S secondValue)
        {
            fRecordName = recordName;
            Name = name;
            Value = value;
            SecondValue = secondValue;
        }

        public void UpdateValue(T additionalTValue, S additionalSValue)
        {
            if (fTAggregation != null)
            {
                Value = fTAggregation(Value, additionalTValue);
            }
            if (fSAggregation != null)
            {
                SecondValue = fSAggregation(SecondValue, additionalSValue);
            }
        }

        public string RecordName()
        {
            return fRecordName;
        }
        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Value}-{SecondValue}";
        }
    }
}
