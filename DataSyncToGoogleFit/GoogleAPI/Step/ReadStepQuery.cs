using Google.Apis.Fitness.v1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSyncToGoogleFit
{
    internal class ReadStepQuery : FitnessQuery
    {
        internal class StepDataPoint
        {
            public int? Step { get; set; }
            public DateTime Stamp { get; set; }
        }

        public ReadStepQuery(FitnessService service)
            : base(service, "derived:com.google.step_count.delta:com.google.android.gms:merge_step_deltas", "com.google.step_count.delta")
        {
        }

        public IList<ReadStepQuery.StepDataPoint> CreateQuery(DateTime start, DateTime end)
        {
            var request = CreateRequest(start, end);
            var response = ExecuteRequest(request);

            return response
              .Bucket
              .SelectMany(b => b.Dataset)
              .Where(d => d.Point != null)
              .SelectMany(d => d.Point)
              .Where(p => p.Value != null)
              .SelectMany(p =>
              {
                  return p.Value.Select(v =>
                    new StepDataPoint
                    {
                        Step = v.IntVal.GetValueOrDefault(),
                        Stamp = GoogleTime.FromNanoseconds(p.StartTimeNanos).ToDateTime()
                    });
              }).ToList();
        }
    }
}