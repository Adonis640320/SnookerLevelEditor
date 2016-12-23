using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SLevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public JToken round_json;
        public JToken challenge_json;
        public static JArray round_array;
        public static JArray challenge_array;
        private string roundPath;
        private string challengePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnImportInfo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "round info|round.txt";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == false)
                return;
            try
            {
                roundPath = dialog.FileName;
                StreamReader stream = File.OpenText(dialog.FileName);
                string str = stream.ReadLine();
                stream.Close();
                round_json = (JToken)JsonConvert.DeserializeObject(str, typeof(JToken));
                JProperty stageInfo = null;
                foreach (JProperty item in ((JObject)round_json).Properties())
                {
                    if (item.Name == "stages")
                    {
                        stageInfo = item;
                        break;
                    }
                }

                if (stageInfo == null)
                    return;

                round_array = (JArray)stageInfo.Value;
                for (int i = 0; i < round_array.Count; i++)
                {
                    JObject jOb = (JObject)round_array[i];
                    foreach (JProperty prop in jOb.Properties())
                    {
                        if (prop.Name == "stageId")
                        {
                            cbLevels.Items.Add(((int)prop.Value + 1).ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fail to read round info.");
                return;
            }

            dialog.Filter = "challenge info|challenge.txt";
            result = dialog.ShowDialog();

            if (result == false)
                return;
            try
            {
                challengePath = dialog.FileName;
                StreamReader stream = File.OpenText(dialog.FileName);
                string str = stream.ReadLine();
                stream.Close();
                challenge_json = (JToken)JsonConvert.DeserializeObject(str, typeof(JToken));
                JProperty stageInfo = null;
                foreach (JProperty item in ((JObject)challenge_json).Properties())
                {
                    if (item.Name == "challenges")
                    {
                        stageInfo = item;
                        break;
                    }
                }

                if (stageInfo == null)
                    return;

                challenge_array = (JArray)stageInfo.Value;
                for (int i = 0; i < challenge_array.Count; i++)
                {
                    JObject jOb = (JObject)challenge_array[i];
                    foreach (JProperty prop in jOb.Properties())
                    {
                        if (prop.Name == "name")
                        {
//                            Challenge_List.Items.Add(prop.Value.ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fail to read challenge info.");
                return;
            }

            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
