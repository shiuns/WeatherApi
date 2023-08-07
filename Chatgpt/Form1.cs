using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Chatgpt
{
    public partial class ApiTest : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private records weatherData;
        public ApiTest()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 設定程式在螢幕正中間
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);

            // 防止程式重覆執行
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] myProcess = Process.GetProcessesByName(processName);
            if (myProcess.Length > 1) { Environment.Exit(0); }
        }


        private async void Send_btn_Click(object sender, EventArgs e)
        {
            Show_txt.Text = "";

            string apiUrl = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-E5AE9AD2-A2D9-4152-8EF3-4195DF68C9EB";

            //API回傳結果
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();

            //取得json字串
            string jsonString = await response.Content.ReadAsStringAsync();

            //反序列化為 JObject 物件
            JObject objString = JsonConvert.DeserializeObject<JObject>(jsonString);

            //records 屬性的 JSON 反序列化為 records 物件
            weatherData = JsonConvert.DeserializeObject<records>(objString["records"].ToString());

            //string locationName = objString["records"]["location"][0]["locationName"].ToString();

            ShowWeather();

        }

        //顯示天氣
        private void ShowWeather()
        {
            string[] City = {"臺北市", "新北市", "桃園市", "臺中市", "臺南市", "高雄市", "新竹縣", "苗栗縣", "彰化縣", "南投縣",
            "雲林縣", "嘉義縣", "屏東縣", "宜蘭縣", "臺東縣", "澎湖縣", "金門縣", "連江縣", "基隆市", "新竹市", "嘉義市", "花蓮縣"};

            string input = Search_txt.Text;
            List<Location> LocationNameArr = new List<Location>();
            StringBuilder jsonResult = new StringBuilder();

            //Location LocationName = weatherData.Location.FirstOrDefault(x => City.Any(city => x.LocationName.Contains(city) && x.LocationName.Contains(input)));

            //輸入內容
            if (input == "" || input == "全部" || input == "全部縣市")
            {
                LocationNameArr = weatherData.Location.ToList();
            }
            else
            {
                //LocationNameArr = weatherData.Location.Where(x => x.LocationName .Contains(input)).ToList();
                LocationNameArr = weatherData.Location.Where(x => x.LocationName.Contains(input)).ToList();

            }

            //處理天氣內容，依序列出
            foreach (Location LocationName in LocationNameArr)
            {
                jsonResult.AppendLine(JsonConvert.SerializeObject(LocationName));
                Show_txt.Text += $"區域: {LocationName.LocationName} 天氣: {LocationName.WeatherElement[0].Time[0].Parameter.ParameterName} 溫度: {LocationName.WeatherElement[2].Time[0].Parameter.ParameterName}℃~{LocationName.WeatherElement[4].Time[0].Parameter.ParameterName}℃ 降雨機率: {LocationName.WeatherElement[1].Time[0].Parameter.ParameterName}%" + Environment.NewLine;
            }
            SaveJsonPath(JsonConvert.SerializeObject(LocationNameArr));
        }

        //儲存json檔案
        private void SaveJsonPath(string jsonData)
        {
            string historyFolder = "History";

            //檢查歷史資料是否存在，不存在就創建
            if (!Directory.Exists(historyFolder))
            {
                Directory.CreateDirectory(historyFolder);
            }

            string DataDate = DateTime.Now.ToString("yyyyMMdd");

            //歷史檔案的完整路徑
            string historyFilePath = Path.Combine(historyFolder, $"{DataDate}.json");
            List<Location> data = new List<Location>();

            //歷史檔案存在
            if (File.Exists(historyFilePath))
            {
                //讀取json檔案內容
                string existData = File.ReadAllText(historyFilePath);
                //如果反序列化失敗，建立 new List<Location>()
                data = JsonConvert.DeserializeObject<List<Location>>(existData) ?? new List<Location>();
            }

            //新的搜尋紀錄
            var locations = JsonConvert.DeserializeObject<List<Location>>(jsonData);


            if (locations != null)
            {
                //原有檔案內容有值，加入新的搜尋紀錄
                if (data.Count > 0)
                {
                    data.AddRange(locations);
                }
                else
                {
                    //如沒有，直接相等
                    data = locations;
                }
            }
            //將合併後紀錄序列化，寫入歷史檔案
            string jsonDataToWrite = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(historyFilePath, jsonDataToWrite);
        }

        //private void textBox1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        ShowWeather();
        //    }
        //}

        //歷史紀錄Btn
        private void History_btn_Click(object sender, EventArgs e)
        {
            FrmHistory frmHistory = new FrmHistory();
            frmHistory.ShowDialog();
        }
    }
}
