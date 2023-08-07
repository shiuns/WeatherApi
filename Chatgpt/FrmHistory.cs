using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chatgpt
{
    public partial class FrmHistory : Form
    {
        public FrmHistory()
        {
            InitializeComponent();
            WeatherColumn();
        }
        private List<Location> historyData = new List<Location>();


        //天氣DataGridView欄位
        private void WeatherColumn()
        {
            //不要自動生成欄位
            Weatherdata_dg.AutoGenerateColumns = false;

            //建立每個自定義資料欄位
            DataGridViewTextBoxColumn locationName = new DataGridViewTextBoxColumn();
            locationName.DataPropertyName = "LocationName";
            locationName.HeaderText = "地點名稱";
            Weatherdata_dg.Columns.Add(locationName);

            DataGridViewTextBoxColumn weatherAll = new DataGridViewTextBoxColumn();
            weatherAll.DataPropertyName = "weatherAll";
            weatherAll.HeaderText = "天氣";
            Weatherdata_dg.Columns.Add(weatherAll);

            DataGridViewTextBoxColumn tempRange = new DataGridViewTextBoxColumn();
            tempRange.DataPropertyName = "TempRange";
            tempRange.HeaderText = "溫度";
            Weatherdata_dg.Columns.Add(tempRange);

            DataGridViewTextBoxColumn pop = new DataGridViewTextBoxColumn();
            pop.DataPropertyName = "PoP";
            pop.HeaderText = "降雨機率";
            Weatherdata_dg.Columns.Add(pop);
        }

        private void FrmHistory_Load(object sender, EventArgs e)
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


            LoadHistoryDates();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("地區");
            //dt.Columns.Add("天氣");
            //dt.Columns.Add("溫度");
            //dt.Columns.Add("降雨機率");
            //dt.Rows.Add("123", "456", "789", "0000");
            //dataGridView1.DataSource = dt;
        }


        private void Date_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistoryAll();
        }
 
        //歷史紀錄的日期
        private void LoadHistoryDates()
        {
            string historyFolder = "History";

            //檢查資料夾是否存在
            if (!Directory.Exists(historyFolder))
            {
                return;
            }

            //取得資料夾json檔
            string[] historyFiles = Directory.GetFiles(historyFolder, "*.json");

            List<DateTime> historyDates = new List<DateTime>();

            //取得日期 並顯示在comboBox
            foreach (string file in historyFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (DateTime.TryParseExact(fileName, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    historyDates.Add(date);
                }
            }

            Date_cb.DataSource = historyDates;
        }

        private void LoadHistoryAll()
        {
            //dataGridView1.Rows.Clear();

            //開啟歷史紀錄時的日期
            DateTime selectedDate = (DateTime)Date_cb.SelectedItem;

            string historyFolder = "History";

            //檔案路徑
            string historyFilePath = Path.Combine(historyFolder, selectedDate.ToString("yyyyMMdd") + ".json");

            //檔案是否存在
            if (!File.Exists(historyFilePath))
            {
                return;
            }

            //讀取檔案內容
            string jsonData = File.ReadAllText(historyFilePath);

            //將讀到的內容反序列化
            historyData = JsonConvert.DeserializeObject<List<Location>>(jsonData);

            List<LocationRecord> locationRecords = new List<LocationRecord>();

            //處理歷史紀錄 並顯示在dataGridView
            foreach (var location in historyData)
            {
                string locationName = location.LocationName;
                string weather = string.Empty;
                string pop = string.Empty;
                string tempeMax = string.Empty;
                string tempMin = string.Empty;
                foreach (var weatherElement in location.WeatherElement)
                {
                    string type = weatherElement.ElementName;
                    var time = weatherElement.Time[0];

                    if (type == "Wx")
                    {
                        weather = time.Parameter.ParameterName;
                    }
                    else if (type == "PoP")
                    {
                        pop = time.Parameter.ParameterName;
                    }
                    else if (type == "MinT")
                    {
                        tempMin = time.Parameter.ParameterName;
                    }
                    else if (type == "MaxT")
                    {
                        tempeMax = time.Parameter.ParameterName;
                    }
                    //dataGridView1.Rows.Add(location.LocationName, parameterName, $"{minTemp}℃~{maxTemp}℃", $"{pop}%");
                }
                LocationRecord locationRecord = new LocationRecord()
                {
                    LocationName = locationName,
                    weatherAll = weather,
                    TempRange = $"{tempMin}℃~{tempeMax}℃",
                    PoP = $"{pop}%"
                };
                locationRecords.Add(locationRecord);
            }
            Weatherdata_dg.DataSource = locationRecords;
        }

        
    }
}
