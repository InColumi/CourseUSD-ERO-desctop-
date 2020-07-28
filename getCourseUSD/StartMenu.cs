using getCourseUSD.Classes;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace getCourseUSD
{
    public partial class StartMenu : Form
    {
        private Course _courseUSD;
        private Course _courseEUR;

        public StartMenu()
        {
            InitializeComponent();
            _courseUSD = new Course();
            _courseEUR = new Course();
        }

        private void UpdateLabelValue(Label labelValue, Course course)
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

            labelPercent.Text = $"{course.DifferenceValue.ToString("N2")} / {sign}{course.DifferenceInPersent.ToString("N1")}%";
        }

        private Course GetCourseByName(string name, string apiPathForUsdAndEUR = "https://www.cbr-xml-daily.ru/daily_json.js")
        {
            using (WebClient client = new WebClient())
            {
                string courses = client.DownloadString(apiPathForUsdAndEUR);
                var decodeCourses = Json.Decode(courses);

                var info = decodeCourses["Valute"][name];

                float current = Single.Parse(info["Value"].ToString());
                float previous = Single.Parse(info["Previous"].ToString());

                return new Course(current, previous);
            }
        }

        private bool TryUpdateAllCurrency()
        {
            try
            {
                _courseUSD = GetCourseByName("USD");
                UpdateLabelValue(labelDollarUSValue, _courseUSD);
                UpdateLabelPercent(labelDollarUSPerсent, _courseUSD);

                _courseEUR = GetCourseByName("EUR");
                UpdateLabelValue(labelEuroValue, _courseEUR);
                UpdateLabelPercent(labelEuroPerсent, _courseEUR);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void UpdateAllCurrensy()
        {
            if (TryUpdateAllCurrency())
            {
                labelTime.Text = GetCurrentTime();
            }
            else
            {
                Application.Exit();
            }
        }

        private void StartMenu_Shown(object sender, EventArgs e)
        {
            UpdateAllCurrensy();
        }

        private string GetCurrentTime()
        {
            return DateTime.Now.ToString("hh:mm tt");
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateAllCurrensy();
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                        return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
