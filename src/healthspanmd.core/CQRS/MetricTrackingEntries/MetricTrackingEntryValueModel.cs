﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTrackingEntries
{
    public class MetricTrackingEntryValueModel
    {
        public long MetricTrackingEntryValueId { get; set; }
        public long MetricTrackingEntryId { get; set; }
        public double? ValueAsNumber { get; set; }
        public double? Value2AsNumber { get; set; }
        public bool? ValueAsBoolean { get; set; }
        public string ValueAsString { get; set; }
        public DateTime? ValueAsDate { get; set; }
    }
}
