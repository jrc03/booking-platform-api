using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateRange(DateTime start, DateTime end)
        {
            if (start.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date cannot be in the past");

            if (start >= end)
                throw new ArgumentException("End date must be after start  date");

            Start = start;
            End = end;
        }
        public bool Overlaps(DateRange other)
        {
            return Start < other.End && other.Start < End;
        }
    }
}