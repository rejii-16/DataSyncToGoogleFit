using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Fitness.v1;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;

namespace DataSyncToGoogleFit
{
    public partial class Form1 : Form
    {
        private const string CLIENT_ID = "hogehoge-hogeraccho.apps.googleusercontent.com";
        private const string JSON_FILE_PATH = @"どこかにあるclient_secret_hogehoge.apps.googleusercontent.com.jsonのパス";
        
        private const string WEIGHT_FILE_PATH = @"からだログ データ_Read.emlへのパス";
        private const string STEP_FILE_PATH = @"CAccupedo毎日記録_Read.htmlへのパス";

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 歩数取得ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetStep_Click(object sender, EventArgs e)
        {
            ICredential credential = await GetUserCredential();
            var service = new FitnessService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Get Fitness Step"
            });

            var step = new ReadStepQuery(service);
            DateTime starttime = Calendar.SelectionStart;
            DateTime endtime = starttime.AddDays(7);
            var results = step.CreateQuery(starttime, endtime);
            TxtboxResult.Text = "*-*-*-*-*-*-* 歩数取得結果 *-*-*-*-*-*-*\r\n";
            foreach (var result in results)
            {
                TxtboxResult.Text += result.Stamp.ToString() + " = " + result.Step + "\r\n";
            }
        }

        /// <summary>
        /// OAuth認証を用いてCredentialを取得する。
        /// </summary>
        private Task<UserCredential> GetUserCredential()
        {
            // 歩数と体重のRead/Write
            var scopes = new[] {
                FitnessService.Scope.FitnessActivityRead,
                FitnessService.Scope.FitnessActivityWrite,
                FitnessService.Scope.FitnessBodyRead,
                FitnessService.Scope.FitnessBodyWrite,
            };

            // ファイル名は先ほど取得した認証情報のjson
            using (var stream = new FileStream(JSON_FILE_PATH, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                  GoogleClientSecrets.Load(stream).Secrets,
                  scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true));
            }
        }

        /// <summary>
        /// 体重取得ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetWeight_Click(object sender, EventArgs e)
        {
            ICredential credential = await GetUserCredential();
            var service = new FitnessService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Get Fitness Weight"
            });

            var query = new ReadWeightQuery(service);
            DateTime starttime = Calendar.SelectionStart;
            DateTime endtime = starttime.AddDays(7);
            var results = query.CreateQuery(starttime, endtime);
            TxtboxResult.Text = "*-*-*-*-*-*-* 体重取得結果 *-*-*-*-*-*-*\r\n";
            foreach (var result in results)
            {
                TxtboxResult.Text += result.Stamp.ToString() + " = " + result.Weight + "\r\n";
            }
        }

        /// <summary>
        /// 体重書き込みボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSetWeight_Click(object sender, EventArgs e)
        {
            ICredential credential = await GetUserCredential();
            var service = new FitnessService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Write Fitness Weight"
            });
            var query = new WriteWeightQuery(service);

            // ファイルから記録するデータを取得
            List<KeyValuePair<DateTime, float>> measures = new List<KeyValuePair<DateTime, float>>();
            string[] lines = File.ReadAllLines(WEIGHT_FILE_PATH);
            for(int i = 0;i < lines.Length; i++)
            {
                string[] item = lines[i].Split(',');
                float.TryParse(item[3], out float floatval);
                // 記録がない場合は除外
                if (floatval == 0) continue;
                string[] dateitem = item[1].Split('/');
                DateTime date = new DateTime(int.Parse(dateitem[0]), int.Parse(dateitem[1]), int.Parse(dateitem[2]));
                measures.Add(new KeyValuePair<DateTime, float>(date, floatval));
            }
            query.CreateQuery(measures, CLIENT_ID);
        }

        /// <summary>
        /// 歩数登録ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSetStep_Click(object sender, EventArgs e)
        {
            ICredential credential = await GetUserCredential();
            var service = new FitnessService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Write Fitness Step"
            });
            var query = new WriteStepQuery(service);
            List<KeyValuePair<DateTime, int>> measures = new List<KeyValuePair<DateTime, int>>();
            string[] lines = File.ReadAllLines(STEP_FILE_PATH);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] item = lines[i].Split(',');
                int.TryParse(item[3], out int intval);
                DateTime date = new DateTime(int.Parse(item[0].Trim()), int.Parse(item[1].Trim()), int.Parse(item[2].Trim()));
                measures.Add(new KeyValuePair<DateTime, int>(date, intval));
            }
            query.CreateQuery(measures, CLIENT_ID);
        }
    }
}
