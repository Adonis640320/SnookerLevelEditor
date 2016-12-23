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
        private List<string> challengesList = new List<string>();
        private LevelEdit m_levelEdit;

        public string m_selectedChallenge; // The selected challenge is decided by Selected Round

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
                            challengesList.Add(prop.Value.ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fail to read challenge info.");
                return;
            }
            cbLevels.SelectedIndex = 0;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;

            m_levelEdit = new LevelEdit();
        }

        public void showEditWindow(bool isEdit)
        {
            JObject obj = null;

            if (isEdit) // Round Edit
            {
                for (int i = 0; i < round_array.Count; i++)
                {
                    JObject jOb = (JObject)round_array[i];
                    foreach (JProperty prop in jOb.Properties())
                    {
                        if (prop.Name == "stageId" && int.Parse(prop.Value.ToString()) == cbLevels.SelectedIndex)
                        {
                            obj = jOb;
                            break;
                        }
                    }
                }

                m_levelEdit.m_selectedRoundIndex = cbLevels.SelectedIndex;
            }
            m_levelEdit.initRoundInfo(isEdit, obj, challengesList);

            obj = null;
//            if (isEdit) // Challenge Edit
 //           {
                for (int i = 0; i < challenge_array.Count; i++)
                {
                    JObject jOb = (JObject)challenge_array[i];
                    foreach (JProperty prop in jOb.Properties())
                    {
                        if (prop.Name == "name" && prop.Value.ToString() == m_selectedChallenge)
                        {
                            obj = jOb;
                            m_levelEdit.m_selectedChallengeIndex = i;
                            break;
                        }
                    }
                }
//            }

            m_levelEdit.initChallengeInfo(true, obj);
            m_levelEdit.initTableInfo();

            m_levelEdit.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            showEditWindow(true);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            showEditWindow(false);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void cbLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLevels.SelectedIndex < 0) return;
            JObject jOb = (JObject)round_array[cbLevels.SelectedIndex];
            foreach (JProperty prop in jOb.Properties())
            {
                if (prop.Name == "gameParams")
                {
                    JObject jVal = (JObject)prop.Value;
                    foreach (JProperty property in jVal.Properties())
                    {
                        if (property.Name == "challengeName")
                        {
                            m_selectedChallenge = property.Value.ToString();
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (JProperty item in ((JObject)round_json).Properties())
            {
                if (item.Name == "stages")
                {
                    item.Value = round_array;
                }
            }
            string jsonString = JsonConvert.SerializeObject(round_json);
            File.WriteAllText(roundPath, jsonString);

            foreach (JProperty item in ((JObject)challenge_json).Properties())
            {
                if (item.Name == "challenges")
                {
                    item.Value = challenge_array;
                }
            }
            jsonString = JsonConvert.SerializeObject(challenge_json);
            File.WriteAllText(challengePath, jsonString);
            MessageBox.Show("Game information saved successfully.");
        }
    }
}
