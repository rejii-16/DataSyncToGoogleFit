using Google.Apis.Fitness.v1.Data;
using Google.Apis.Fitness.v1;
using System;
using System.Collections.Generic;

namespace DataSyncToGoogleFit
{
    internal class WriteStepQuery : FitnessQuery
    {
        public WriteStepQuery(FitnessService service)
            : base(service, "raw:com.google.step_count.cumulativ:com.google.android.apps.fitness:user_input", "com.google.step_count.delta")
        {
        }

        public void CreateQuery(List<KeyValuePair<DateTime, int>> measures, string clientId)
        {
            DataSource dataSource = new DataSource()
            {
                Type = "derived",
                Application = new Application() { Name = "estimated_steps" },
                DataType = new DataType()
                {
                    Name = "com.google.step_count.delta",
                    Field = new List<DataTypeField>() { new DataTypeField() { Name = "steps", Format = "integer" } }
                },
                Device = new Device() { Type = "tablet", Manufacturer = "unknown", Model = "unknown", Uid = "unknown", Version = "1.0" }
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

            Dataset stepDataSource = new Dataset()
            {
                DataSourceId = dataSourceId,
                Point = new List<DataPoint>()
            };

            DateTime minDateTime = DateTime.MaxValue;
            DateTime maxDateTime = DateTime.MinValue;
            foreach (var step in measures)
            {
                GoogleTime ts = GoogleTime.FromDateTime(step.Key);
                GoogleTime end = GoogleTime.FromDateTime(step.Key.AddHours(23).AddMinutes(59).AddSeconds(59));
                stepDataSource.Point.Add
                (
                    new DataPoint()
                    {
                        DataTypeName = "com.google.step_count.delta",
                        StartTimeNanos = ts.TotalNanoSeconds,
                        EndTimeNanos = end.TotalNanoSeconds,
                        Value = new List<Value>() { new Value() { IntVal = step.Value } }
                    }
                );

                if (minDateTime > step.Key) minDateTime = step.Key;
                if (maxDateTime < step.Key) maxDateTime = step.Key;
            }

            stepDataSource.MinStartTimeNs = GoogleTime.FromDateTime(minDateTime).TotalNanoSeconds;
            stepDataSource.MaxEndTimeNs = GoogleTime.FromDateTime(maxDateTime.AddHours(23).AddMinutes(59).AddSeconds(59)).TotalNanoSeconds;
            string dataSetId = stepDataSource.MinStartTimeNs.ToString() + "-" + stepDataSource.MaxEndTimeNs.ToString();
            var save = _service.Users.DataSources.Datasets.Patch(stepDataSource, "me", dataSourceId, dataSetId).Execute();
        }
    }
}
