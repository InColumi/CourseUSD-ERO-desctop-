using getCourseUSD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace getCourseUSD
{
    public partial class StartMenu : Form
    {
        private string _apiPathForUsdAndEUR = "https://www.cbr-xml-daily.ru/daily_json.js";
        private Course _courseUSD;
        private Course _courseEUR;
        private Course _courseMonero;
        private Course _courseBitcoin;

        public StartMenu()
        {
            InitializeComponent();
            _courseUSD = new Course();
            _courseEUR = new Course();
            _courseMonero = new Course();
            _courseBitcoin = new Course();
        }

        private void UpdateLabelValue(Label labelValue,  Course course)
        {
            labelValue.Text = course.Current.ToString("N2");
        }

        private void UpdateLabelPercent(Label labelPercent, Course course)
        {
            char sign;
            bool isPossitive;

            isPossitive = course.Current > course.Previous;
            sign = (isPossitive) ? '+' : '-';

            labelPercent.ForeColor = (isPossitive) ? Color.Green : Color.Red;

            labelPercent.Text = $"{sign}{course.Difference.ToString("N1")}%";
        }

        private Course GetCourseByName(string name)
        {
            using (WebClient client = new WebClient())
            {
                string courses = client.DownloadString(_apiPathForUsdAndEUR);
                var decodeCourses = Json.Decode(courses);

                var info = decodeCourses["Valute"][name];

                float current = Single.Parse(info["Value"].ToString());
                float previous = Single.Parse(info["Previous"].ToString());

                return new Course(current, previous);
            }
        }

        private void UpdateAllCurrency()
        {
            try
            {
                _courseUSD = GetCourseByName("USD");
                UpdateLabelValue(labelDollarUSValue, _courseUSD);
                UpdateLabelPercent(labelDollarUSPerсent, _courseUSD);

                _courseEUR = GetCourseByName("EUR");
                UpdateLabelValue(labelEuroValue, _courseEUR);
                UpdateLabelPercent(labelEuroPerсent, _courseEUR);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void StartMenu_Shown(object sender, EventArgs e)
        {
            UpdateAllCurrency();
        }

        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
