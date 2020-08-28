﻿using System;

namespace cdabtesttools.Measurement
{
    internal class ExceptionMetric : IMetric
    {
        private Exception e;

        public MetricName Name => MetricName.exception;

        public string Uom => "#";

        public Exception Exception { get => e; }

        public ExceptionMetric(Exception e)
        {
            this.e = e;
        }
    }
}