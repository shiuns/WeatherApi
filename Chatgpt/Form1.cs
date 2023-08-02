using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Chatgpt
{
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private records weatherData;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string apiUrl = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-E5AE9AD2-A2D9-4152-8EF3-4195DF68C9EB";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();

            JObject objString = JsonConvert.DeserializeObject<JObject>(jsonString);

            weatherData = JsonConvert.DeserializeObject<records>(objString["records"].ToString());

            //string locationName = objString["records"]["location"][0]["locationName"].ToString();

            ShowWeather();


        }

        


        private void ShowWeather()
        {
            string[] City = {"�O�_��", "�s�_��", "��饫", "�O����", "�O�n��", "������", "�s�˿�", "�]�߿�", "���ƿ�", "�n�뿤",
            "���L��", "�Ÿq��", "�̪F��", "�y����", "�O�F��", "���", "������", "�s����", "�򶩥�", "�s�˥�", "�Ÿq��", "�Ὤ��"};

            string input = textBox1.Text;
            List<Location> LocationNameArr = weatherData.Location.Where(x => x.LocationName.Contains(input)).ToList();
            StringBuilder jsonResult = new StringBuilder();
            //Location LocationName = weatherData.Location.FirstOrDefault(x => City.Any(city => x.LocationName.Contains(city) && x.LocationName.Contains(input)));

            if (input == "" || input == "����" || input == "��������")
            {
                textBox2.Text = weatherData.ToString();
                SaveJsonPath(JsonConvert.SerializeObject(weatherData));
            }
            else if (input != null && LocationNameArr != null)
            {

                foreach (Location LocationName in LocationNameArr)
                {
                    jsonResult.AppendLine(JsonConvert.SerializeObject(LocationName));
                    textBox2.Text += $"�ϰ�: {LocationName.LocationName} �Ѯ�: {LocationName.WeatherElement[0].Time[0].Parameter.ParameterName} �ū�: {LocationName.WeatherElement[2].Time[0].Parameter.ParameterName}�J~{LocationName.WeatherElement[4].Time[0].Parameter.ParameterName}�J ���B���v: {LocationName.WeatherElement[1].Time[0].Parameter.ParameterName}%" + Environment.NewLine;
                }
                SaveJsonPath(JsonConvert.SerializeObject(LocationNameArr));

                //LocationNameArr.ForEach(location =>
                //{
                //    textBox2.Text += $"�ϰ�: {location.LocationName} �Ѯ�: {location.WeatherElement[0].Time[0].Parameter.ParameterName} �ū�: {location.WeatherElement[2].Time[0].Parameter.ParameterName}�J~{location.WeatherElement[4].Time[0].Parameter.ParameterName}�J ���B���v: {location.WeatherElement[1].Time[0].Parameter.ParameterName}%" + Environment.NewLine;
                //});
            }
            else
            {
                textBox2.Text = "";
                MessageBox.Show("�п�J���T����");
                textBox1.Text = "";
            }
        }


        private void SaveJsonPath(string jsonData)
        {
            string historyFolder = "History";
            if (!Directory.Exists(historyFolder))
            {
                Directory.CreateDirectory(historyFolder);
            }

            string DataDate = DateTime.Now.ToString("yyyyMMdd");
            string historyFilePath = Path.Combine(historyFolder, $"{DataDate}.json");
            List<Location> data = new List<Location>();

            if (File.Exists(historyFilePath))
            {
                string existData = File.ReadAllText(historyFilePath);
                data = JsonConvert.DeserializeObject<List<Location>>(existData) ?? new List<Location>();
            }
            var locations = JsonConvert.DeserializeObject<List<Location>>(jsonData);


            if (locations != null)
            {
                if (data.Count > 0)
                {
                    data.AddRange(locations);
                }
                else
                {
                    data = locations;
                }
            }
            
            string jsonDataToWrite = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(historyFilePath, jsonDataToWrite);
        }




        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowWeather();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmHistory frmHistory = new FrmHistory();
            frmHistory.ShowDialog();
        }
    }
}
