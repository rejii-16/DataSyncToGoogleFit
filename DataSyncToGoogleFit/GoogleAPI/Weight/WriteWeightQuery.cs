using Google.Apis.Fitness.v1;
using Google.Apis.Fitness.v1.Data;
using System;
using System.Collections.Generic;

namespace DataSyncToGoogleFit
{
    internal class WriteWeightQuery : FitnessQuery
    {
        public WriteWeightQuery(FitnessService service)
            : base(service, "raw:com.google.weight:com.google.android.apps.fitness:user_input", "com.google.weight.summary")
        {
        }

        public void CreateQuery(List<KeyValuePair<DateTime, float>> measures, string clientId)
        {
            DataSource dataSource = new DataSource()
            {
                Type = "raw",
                Application = new Application() { Name = "maweightimport" },
                DataType = new DataType()
                {
                    Name = "com.google.weight",
                    Field = new List<DataTypeField>() { new DataTypeField() { Name = "weight", Format = "floatPoint" } }
                },
                Device = new Device() { Type = "scale", Manufacturer = "unknown", Model = "unknown", Uid = "maweightimport", Version = "1.0" }
            };

            string dataSourceId = $"{dataSource.Type}:{dataSource.DataType.Name}:{clientId.Split('-')[0]}:{dataSource.Device.Manufacturer}:{dataSource.Device.Model}:{dataSource.Device.Uid}";
            try
            {
                DataSource googleDataSource = _service.Users.DataSources.Get("me", dataSourceId).Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                DataSource googleDataSource = _service.Users.DataSources.Create(dataSource, "me").Execute();
            }

            Dataset weightsDataSource = new Dataset()
            {
                DataSourceId = dataSourceId,
                Point = new List<DataPoint>()
            };

            DateTime minDateTime = DateTime.MaxValue;
            DateTime maxDateTime = DateTime.MinValue;
            foreach (var weight in measures)
            {
                GoogleTime ts = GoogleTime.FromDateTime(weight.Key);
                weightsDataSource.Point.Add
                (
                    new DataPoint()
                    {
                        DataTypeName = "com.google.weight",
                        StartTimeNanos = ts.TotalNanoSeconds,
                        EndTimeNanos = ts.TotalNanoSeconds,
                        Value = new List<Value>() { new Value() { FpVal = weight.Value } }
                    }
                );

                if (minDateTime > weight.Key) minDateTime = weight.Key;
                if (maxDateTime < weight.Key) maxDateTime = weight.Key;
            }

            weightsDataSource.MinStartTimeNs = GoogleTime.FromDateTime(minDateTime).TotalNanoSeconds;
            weightsDataSource.MaxEndTimeNs = GoogleTime.FromDateTime(maxDateTime).TotalNanoSeconds;
            string dataSetId = weightsDataSource.MinStartTimeNs.ToString() + "-" + weightsDataSource.MaxEndTimeNs.ToString();
            var save = _service.Users.DataSources.Datasets.Patch(weightsDataSource, "me", dataSourceId, dataSetId).Execute();
        }
    }
}
