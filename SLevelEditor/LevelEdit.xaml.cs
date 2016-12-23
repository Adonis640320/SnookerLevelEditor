using Newtonsoft.Json.Linq;
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
using System.Windows.Shapes;

namespace SLevelEditor
{
    /// <summary>
    /// Interaction logic for LevelEdit.xaml
    /// </summary>
    public partial class LevelEdit : Window
    {
        public JObject editorObject;
        public int m_selectedRound;

        private bool m_isMovingSampleBall { set; get; }

        private Image m_draggedImage;
        private Point m_mousePos;

        public LevelEdit()
        {
            InitializeComponent();
            m_selectedRound = -1;
        }

        public void initBoard(bool isEditMode, JObject obj, List<string> challengesList)
        {
            if (!isEditMode) // New
            {
                editorObject = new JObject((JObject)MainWindow.round_array[0]);
                foreach (JProperty prop in editorObject.Properties())
                {
                    if (prop.Name == "position")
                    {
                        JObject posObj = (JObject)prop.Value;
                        foreach (JProperty posProp in posObj.Properties())
                        {
                            if (posProp.Name == "x")
                                xpos_value.Text = "0";
                            if (posProp.Name == "y")
                                ypos_value.Text = "0";
                        }
                    }
                    else if (prop.Name == "minMyTeamMembers")
                    {
                        min_my_team_members_value.Text = "0";
                    }
                    else if (prop.Name == "prevGroup")
                    {
                        JObject grObj = (JObject)prop.Value;
                        foreach (JProperty grProp in grObj.Properties())
                        {
                            if (grProp.Name == "index")
                                prev_group_index_value.Text = "-1";
                        }
                    }
                    else if (prop.Name == "nextGroup")
                    {
                        JObject grObj = (JObject)prop.Value;
                        foreach (JProperty grProp in grObj.Properties())
                        {
                            if (grProp.Name == "index")
                                next_group_index_value.Text = "1";
                        }
                    }
                    else if (prop.Name == "prevStage")
                    {
                        JObject stObj = (JObject)prop.Value;
                        foreach (JProperty stProp in stObj.Properties())
                        {
                            if (stProp.Name == "index")
                                prev_stage_index_value.Text = "0";
                        }
                    }
                    else if (prop.Name == "type")
                    {
                        type_combobox.SelectedIndex = 0;
                    }
                    else if (prop.Name == "flag")
                    {
                        flag_combobox.SelectedIndex = 0;
                    }
                    else if (prop.Name == "minAvgDifference")
                    {
                        min_avg_difference_value.Text = "10";
                    }
                    else if (prop.Name == "gameParams")
                    {
                        JObject gpObj = (JObject)prop.Value;
                        foreach (JProperty gpProp in gpObj.Properties())
                        {
                            if (gpProp.Name == "challengeName")
                            {
                                challenge_name_combobox.SelectedIndex = 0;
                            }
                            else if (gpProp.Name == "pointDifferenceOneStars")
                            {
                                one_stars_value.Text = "0";
                            }
                            else if (gpProp.Name == "pointDifferenceTwoStars")
                            {
                                two_stars_value.Text = "0";
                            }
                            else if (gpProp.Name == "pointDifferenceTreeStars")
                            {
                                three_stars_value.Text = "0";
                            }
                            else if (gpProp.Name == "coins1Star")
                            {
                                star1_value.Text = "0";
                            }
                            else if (gpProp.Name == "coins2Star")
                            {
                                star2_value.Text = "0";
                            }
                            else if (gpProp.Name == "coins3Star")
                            {
                                star3_value.Text = "0";
                            }
                            else if (gpProp.Name == "energyRequired")
                            {
                                energy_required_value.Text = "0";
                            }
                            else if (gpProp.Name == "rewindCost")
                            {
                                rewind_cost_value.Text = "0";
                            }
                            else if (gpProp.Name == "timeoutMinutes")
                            {
                                minutes_value.Text = "0";
                            }
                            else if (gpProp.Name == "timeoutSeconds")
                            {
                                seconds_value.Text = "0";
                            }
                            else if (gpProp.Name == "expGainWin")
                            {
                                exp_gain_win_value.Text = "20";
                            }
                            else if (gpProp.Name == "expGainLose")
                            {
                                exp_gain_lose_value.Text = "0";
                            }
                        }
                    }
                }
            }
            else
            {
                create_button.Text = "Edit";
                editorObject = new JObject(obj);
                foreach (JProperty prop in editorObject.Properties())
                {
                    if (prop.Name == "position")
                    {
                        JObject posObj = (JObject)prop.Value;
                        foreach (JProperty posProp in posObj.Properties())
                        {
                            if (posProp.Name == "x")
                                xpos_value.Text = posProp.Value.ToString();
                            if (posProp.Name == "y")
                                ypos_value.Text = posProp.Value.ToString();
                        }
                    }
                    else if (prop.Name == "description")
                    {
                        description_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "pointsDescrption")
                    {
                        point_description_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "arenaName")
                    {
                        arena_name_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "isOptional")
                    {
                        isoptional_checkbox.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "requiresMyTeam")
                    {
                        require_my_team_checkbox.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "minMyTeamMembers")
                    {
                        min_my_team_members_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "showIntro")
                    {
                        show_intro_checkbox.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "showDelayedIntro")
                    {
                        show_delayed_intro_checkbox.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "startsWithTimeout")
                    {
                        starts_with_timeout_checkbox.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "prevGroup")
                    {
                        JObject grObj = (JObject)prop.Value;
                        foreach (JProperty grProp in grObj.Properties())
                        {
                            if (grProp.Name == "index")
                                prev_group_index_value.Text = grProp.Value.ToString();
                        }
                    }
                    else if (prop.Name == "nextGroup")
                    {
                        JObject grObj = (JObject)prop.Value;
                        foreach (JProperty grProp in grObj.Properties())
                        {
                            if (grProp.Name == "index")
                                next_group_index_value.Text = grProp.Value.ToString();
                        }
                    }
                    else if (prop.Name == "prevStage")
                    {
                        JObject stObj = (JObject)prop.Value;
                        foreach (JProperty stProp in stObj.Properties())
                        {
                            if (stProp.Name == "index")
                                prev_stage_index_value.Text = stProp.Value.ToString();
                        }
                    }
                    else if (prop.Name == "type")
                    {
                        type_combobox.SelectedIndex = (int)prop.Value;
                    }
                    else if (prop.Name == "flag")
                    {
                        flag_combobox.SelectedIndex = (int)prop.Value;
                    }
                    else if (prop.Name == "minAvgDifference")
                    {
                        min_avg_difference_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "gameParams")
                    {
                        JObject gpObj = (JObject)prop.Value;
                        foreach (JProperty gpProp in gpObj.Properties())
                        {
                            if (gpProp.Name == "challengeName")
                            {
                                int i;
                                for (i = 0; i < challenges.Items.Count; i++)
                                {
                                    if (challenges.Items[i].ToString() == gpProp.Value.ToString())
                                        break;
                                }
                                challenge_name_combobox.SelectedIndex = i;
                            }
                            else if (gpProp.Name == "pointDifferenceOneStars")
                            {
                                one_stars_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "pointDifferenceTwoStars")
                            {
                                two_stars_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "pointDifferenceTreeStars")
                            {
                                three_stars_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "coins1Star")
                            {
                                star1_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "coins2Star")
                            {
                                star2_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "coins3Star")
                            {
                                star3_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "energyRequired")
                            {
                                energy_required_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "rewindCost")
                            {
                                rewind_cost_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "timeoutMinutes")
                            {
                                minutes_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "timeoutSeconds")
                            {
                                seconds_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "expGainWin")
                            {
                                exp_gain_win_value.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "expGainLose")
                            {
                                exp_gain_lose_value.Text = gpProp.Value.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void btnBall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_isMovingSampleBall = true;
            
        }

        private void canvasTable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;

            if (image != null && canvasTable.CaptureMouse())
            {
                m_mousePos = e.GetPosition(canvasTable);
                m_draggedImage = image;
                Panel.SetZIndex(m_draggedImage, 1); // in case of multiple images
            }
        }

        private void canvasTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (m_draggedImage != null)
            {
                canvasTable.ReleaseMouseCapture();
                Panel.SetZIndex(m_draggedImage, 0);
                m_draggedImage = null;
            }
        }

        private void canvasTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_draggedImage != null)
            {
                var position = e.GetPosition(canvasTable);
                var offset = position - m_mousePos;
                m_mousePos = position;
                Canvas.SetLeft(m_draggedImage, Canvas.GetLeft(m_draggedImage) + offset.X);
                Canvas.SetTop(m_draggedImage, Canvas.GetTop(m_draggedImage) + offset.Y);
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_isMovingSampleBall = false;
        }

        private void btnAddBall_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label btnBall = sender as Label;
            Image btnImage = (Image)FindName(btnBall.Name.Remove(0, 6));
            Image image = new Image() { Source = ((Image)btnImage).Source, Width =25, Height=25 };
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            canvasTable.Children.Add(image);
        }
    }
}