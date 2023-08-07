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
            // �]�w�{���b�ù�������
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);

            // ����{�����а���
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] myProcess = Process.GetProcessesByName(processName);
            if (myProcess.Length > 1) { Environment.Exit(0); }
        }


        private async void Send_btn_Click(object sender, EventArgs e)
        {
            Show_txt.Text = "";

            string apiUrl = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-E5AE9AD2-A2D9-4152-8EF3-4195DF68C9EB";

            //API�^�ǵ��G
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            response.EnsureSuccessStatusCode();

            //���ojson�r��
            string jsonString = await response.Content.ReadAsStringAsync();

            //�ϧǦC�Ƭ� JObject ����
            JObject objString = JsonConvert.DeserializeObject<JObject>(jsonString);

            //records �ݩʪ� JSON �ϧǦC�Ƭ� records ����
            weatherData = JsonConvert.DeserializeObject<records>(objString["records"].ToString());

            //string locationName = objString["records"]["location"][0]["locationName"].ToString();

            ShowWeather();

        }

        //��ܤѮ�
        private void ShowWeather()
        {
            string[] City = {"�O�_��", "�s�_��", "��饫", "�O����", "�O�n��", "������", "�s�˿�", "�]�߿�", "���ƿ�", "�n�뿤",
            "���L��", "�Ÿq��", "�̪F��", "�y����", "�O�F��", "���", "������", "�s����", "�򶩥�", "�s�˥�", "�Ÿq��", "�Ὤ��"};

            string input = Search_txt.Text;
            List<Location> LocationNameArr = new List<Location>();
            StringBuilder jsonResult = new StringBuilder();

            //Location LocationName = weatherData.Location.FirstOrDefault(x => City.Any(city => x.LocationName.Contains(city) && x.LocationName.Contains(input)));

            //��J���e
            if (input == "" || input == "����" || input == "��������")
            {
                LocationNameArr = weatherData.Location.ToList();
            }
            else
            {
                //LocationNameArr = weatherData.Location.Where(x => x.LocationName .Contains(input)).ToList();
                LocationNameArr = weatherData.Location.Where(x => x.LocationName.Contains(input)).ToList();

            }

            //�B�z�Ѯ𤺮e�A�̧ǦC�X
            foreach (Location LocationName in LocationNameArr)
            {
                jsonResult.AppendLine(JsonConvert.SerializeObject(LocationName));
                Show_txt.Text += $"�ϰ�: {LocationName.LocationName} �Ѯ�: {LocationName.WeatherElement[0].Time[0].Parameter.ParameterName} �ū�: {LocationName.WeatherElement[2].Time[0].Parameter.ParameterName}�J~{LocationName.WeatherElement[4].Time[0].Parameter.ParameterName}�J ���B���v: {LocationName.WeatherElement[1].Time[0].Parameter.ParameterName}%" + Environment.NewLine;
            }
            SaveJsonPath(JsonConvert.SerializeObject(LocationNameArr));
        }

        //�x�sjson�ɮ�
        private void SaveJsonPath(string jsonData)
        {
            string historyFolder = "History";

            //�ˬd���v��ƬO�_�s�b�A���s�b�N�Ы�
            if (!Directory.Exists(historyFolder))
            {
                Directory.CreateDirectory(historyFolder);
            }

            string DataDate = DateTime.Now.ToString("yyyyMMdd");

            //���v�ɮת�������|
            string historyFilePath = Path.Combine(historyFolder, $"{DataDate}.json");
            List<Location> data = new List<Location>();

            //���v�ɮצs�b
            if (File.Exists(historyFilePath))
            {
                //Ū��json�ɮפ��e
                string existData = File.ReadAllText(historyFilePath);
                //�p�G�ϧǦC�ƥ��ѡA�إ� new List<Location>()
                data = JsonConvert.DeserializeObject<List<Location>>(existData) ?? new List<Location>();
            }

            //�s���j�M����
            var locations = JsonConvert.DeserializeObject<List<Location>>(jsonData);


            if (locations != null)
            {
                //�즳�ɮפ��e���ȡA�[�J�s���j�M����
                if (data.Count > 0)
                {
                    data.AddRange(locations);
                }
                else
                {
                    //�p�S���A�����۵�
                    data = locations;
                }
            }
            //�N�X�֫�����ǦC�ơA�g�J���v�ɮ�
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

        //���v����Btn
        private void History_btn_Click(object sender, EventArgs e)
        {
            FrmHistory frmHistory = new FrmHistory();
            frmHistory.ShowDialog();
        }
    }
}
