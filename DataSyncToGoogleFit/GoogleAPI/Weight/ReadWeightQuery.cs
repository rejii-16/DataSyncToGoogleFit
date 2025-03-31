using Google.Apis.Fitness.v1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSyncToGoogleFit
{
    internal class ReadWeightQuery : FitnessQuery
    {
        internal class WeightDataPoint
        {
            public double? Weight { get; set; }
            public DateTime Stamp { get; set; }
        }
        public ReadWeightQuery(FitnessService service)
            : base(service, "derived:com.google.weight:com.google.android.gms:merge_weight", "com.google.weight.summary")
        {
        }

        public IList<ReadWeightQuery.WeightDataPoint> CreateQuery(DateTime start, DateTime end)
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
                        new WeightDataPoint
                        {
                            Weight = v.FpVal.GetValueOrDefault(),
                            Stamp = GoogleTime.FromNanoseconds(p.StartTimeNanos).ToDateTime()
                        });
                }).ToList();
        }
    }
}
