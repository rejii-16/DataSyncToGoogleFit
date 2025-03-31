using Google.Apis.Fitness.v1;
using Google.Apis.Fitness.v1.Data;
using System;

namespace DataSyncToGoogleFit
{
    internal class FitnessQuery
    {
        protected FitnessService _service;
        private string _dataSourceId;
        private string _dataType;

        public FitnessQuery(FitnessService service, string dataSourceId, string dataType)
        {
            _service = service;
            _dataSourceId = dataSourceId;
            _dataType = dataType;
        }

        protected AggregateRequest CreateRequest(DateTime start, DateTime end, TimeSpan? bucketDuration = null)
        {
            var bucketTimeSpan = bucketDuration.GetValueOrDefault(TimeSpan.FromDays(1));
            return new AggregateRequest
            {
                AggregateBy = new AggregateBy[] { new AggregateBy { DataSourceId = _dataSourceId, DataTypeName = _dataType } },
                BucketByTime = new BucketByTime { DurationMillis = (long)bucketTimeSpan.TotalMilliseconds },
                StartTimeMillis = GoogleTime.FromDateTime(start).TotalMilliseconds,
                EndTimeMillis = GoogleTime.FromDateTime(end).TotalMilliseconds
            };
        }

        protected virtual AggregateResponse ExecuteRequest(AggregateRequest request, string userId = "me")
        {
            var agg = _service.Users.Dataset.Aggregate(request, userId);
            return agg.Execute();
        }
    }
}
