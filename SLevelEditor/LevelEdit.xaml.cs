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
        public JObject m_currentRound, m_currentChallenge;
        public int m_selectedRoundIndex, m_selectedChallengeIndex;

//        public JArray opponents_array = new JArray();
//        public JArray challenges_array = new JArray();
        public JObject cueBall = new JObject();
        public JArray balls_array = new JArray();

        private List<Image> m_ballsList = new List<Image>();

        /*private int m_pressType = 0;
        private int m_selectedBallType = 4;
        private Point pressedPosition = new Point(0, 0);*/

        private bool m_isMovingSampleBall { set; get; }
        private bool m_isEditMode;
        private Image m_draggedImage, m_whiteBall, m_tempDraggingImage;

        private Point m_mousePos;
        private string m_challengeName;

        public LevelEdit()
        {
            InitializeComponent();
            m_selectedRoundIndex = 0;
            m_isEditMode = false;
            m_challengeName = "";

            Image whiteBall = (Image)FindName("WhiteBall");
            m_whiteBall = new Image() { Source = ((Image)whiteBall).Source, Width = 15, Height = 15 };
            canvasTable.Children.Add(m_whiteBall);
        }

        public void initRoundInfo(bool isEditMode, JObject obj, List<string> challengesList)
        {
            m_isEditMode = isEditMode;

            // Round Edit
            if (!isEditMode) // New
            {
                m_currentRound = new JObject((JObject)MainWindow.round_array[0]);
                foreach (JProperty prop in m_currentRound.Properties())
                {
                    if (prop.Name == "position")
                    {
                        JObject posObj = (JObject)prop.Value;
                        foreach (JProperty posProp in posObj.Properties())
                        {
                            if (posProp.Name == "x")
                                txtPositionX.Text = "0";
                            if (posProp.Name == "y")
                                txtPositionY.Text = "0";
                        }
                    }
/*                  else if (prop.Name == "minMyTeamMembers")
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
                    }*/
                    else if (prop.Name == "gameParams")
                    {
                        JObject gpObj = (JObject)prop.Value;
                        foreach (JProperty gpProp in gpObj.Properties())
                        {
                            if (gpProp.Name == "challengeName")
                            {
                                m_challengeName = "1";
//                                challenge_name_combobox.SelectedIndex = 0;
                            }
                            /*else if (gpProp.Name == "pointDifferenceOneStars")
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
                            }*/
                            else if (gpProp.Name == "coins1Star")
                            {
                                txtOneStar.Text = "0";
                            }
                            else if (gpProp.Name == "coins2Star")
                            {
                                txtTwoStars.Text = "0";
                            }
                            else if (gpProp.Name == "coins3Star")
                            {
                                txtThreeStars.Text = "0";
                            }
/*                            else if (gpProp.Name == "energyRequired")
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
                            }*/
                        }
                    }
                }
            }
            else
            {
                m_currentRound = new JObject(obj);
                foreach (JProperty prop in m_currentRound.Properties())
                {
                    if (prop.Name == "position")
                    {
                        JObject posObj = (JObject)prop.Value;
                        foreach (JProperty posProp in posObj.Properties())
                        {
                            if (posProp.Name == "x")
                                txtPositionX.Text = posProp.Value.ToString();
                            if (posProp.Name == "y")
                                txtPositionY.Text = posProp.Value.ToString();
                        }
                    }
/*                    else if (prop.Name == "description")
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
                    else */if (prop.Name == "gameParams")
                    {
                        JObject gpObj = (JObject)prop.Value;
                        foreach (JProperty gpProp in gpObj.Properties())
                        {
                            if (gpProp.Name == "challengeName")
                            {
                                int i;
                                for (i = 0; i < challengesList.Count; i++)
                                {
                                    if (challengesList[i].ToString() == gpProp.Value.ToString())
                                        break;
                                }
                                m_challengeName = challengesList[i];
                                //challenge_name_combobox.SelectedIndex = i;
                            }
                            /*else if (gpProp.Name == "pointDifferenceOneStars")
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
                            }*/
                            else if (gpProp.Name == "coins1Star")
                            {
                                txtOneStar.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "coins2Star")
                            {
                                txtTwoStars.Text = gpProp.Value.ToString();
                            }
                            else if (gpProp.Name == "coins3Star")
                            {
                                txtThreeStars.Text = gpProp.Value.ToString();
                            }
/*                            else if (gpProp.Name == "energyRequired")
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
                            }*/
                        }
                    }
                }
            }
        }
        public void initChallengeInfo(bool isEditMode, JObject obj)
        {
            if( !isEditMode) // New
            {
                m_currentChallenge = new JObject((JObject)MainWindow.challenge_array[140]);
                foreach (JProperty prop in m_currentChallenge.Properties())
                {
/*                    if (prop.Name == "aimLengthMult")
                    {
                        aim_length_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "challengeType")
                    {
                        challenge_type_value.SelectedIndex = 0;
                    }
                    else if (prop.Name == "useBaseState")
                    {
                        use_base_state_value.Checked = true;
                    }
                    else if (prop.Name == "minScore")
                    {
                        min_score_value.Text = "0";
                    }
                    else if (prop.Name == "player2Flag")
                    {
                        player2_flag_value.SelectedIndex = 0;
                    }
                    else if (prop.Name == "player2Score")
                    {
                        player2_score_value.Text = "0";
                    }
                    else if (prop.Name == "shotsTillSpinDisappear")
                    {
                        shots_till_spin_disappear_value.Text = "2";
                    }
                    else if (prop.Name == "minShotsTillRewind")
                    {
                        min_shots_till_rewind_value.Text = "0";
                    }
                    else if (prop.Name == "numRewinds")
                    {
                        num_rewinds_value.Text = "0";
                    }
                    else*/ if (prop.Name == "cueBall")
                    {
                        cueBall = new JObject((JObject)prop.Value);
                        foreach (JProperty property in cueBall.Properties())
                        {
                            if (property.Name == "x")
                            {
                                property.Value = 0;
                            }
                            else if (property.Name == "z")
                            {
                                property.Value = 0;
                            }
                        }
                    }
                }
//                opponents_array = new JArray();
//                challenges_array = new JArray();
                balls_array = new JArray();
            }
            else // Edit
            {
                m_currentChallenge = new JObject(obj);
                foreach (JProperty prop in m_currentChallenge.Properties())
                {
    /*              if (prop.Name == "name")
                    {
                        name_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "aimLengthMult")
                    {
                        aim_length_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "challengeType")
                    {
                        challenge_type_value.SelectedIndex = (int)prop.Value;
                    }
                    else if (prop.Name == "useMinScore")
                    {
                        use_min_score_value.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "minScore")
                    {
                        min_score_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "player2Name")
                    {
                        player2_name_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "player2Flag")
                    {
                        player2_flag_value.SelectedIndex = (int)prop.Value;
                    }
                    else if (prop.Name == "player2Score")
                    {
                        player2_score_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "useBaseState")
                    {
                        use_base_state_value.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "spinText")
                    {
                        spin_text_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "shotsTillSpinDisappear")
                    {
                        shots_till_spin_disappear_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "challengeName")
                    {
                        challenge_name_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "introDescription")
                    {
                        description_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "introExplanation")
                    {
                        explanation_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "useCoachDialog")
                    {
                        use_coach_dialog_value.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "skipFirstPart")
                    {
                        skip_first_part_value.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "skipSecondPart")
                    {
                        skip_second_part_value.Checked = (bool)prop.Value;
                    }
                    else if (prop.Name == "playingAgainstPlayer")
                    {
                        playing_against_player_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "minShotsTillRewind")
                    {
                        min_shots_till_rewind_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "numRewinds")
                    {
                        num_rewinds_value.Text = prop.Value.ToString();
                    }
                    else if (prop.Name == "leadOpponents")
                    {
                        opponents_array = (JArray)prop.Value;
                        for (int i = 0; i < opponents_array.Count; i++)
                        {
                            JObject jObject = (JObject)opponents_array[i];
                            foreach (JProperty property in jObject.Properties())
                            {
                                if (property.Name == "opponentName")
                                {
                                    opponents_list.Items.Add(property.Value.ToString());
                                }
                            }
                        }
                    }
                    else if (prop.Name == "challenges")
                    {
                        challenges_array = (JArray)prop.Value;
                        for (int i = 0; i < challenges_array.Count; i++)
                        {
                            JObject jObject = (JObject)challenges_array[i];
                            foreach (JProperty property in jObject.Properties())
                            {
                                if (property.Name == "name")
                                {
                                    challenges_list.Items.Add(property.Value.ToString());
                                }
                            }
                        }
                    }
                    else*/
                    if (prop.Name == "cueBall")
                    {
                        cueBall = new JObject((JObject)prop.Value);
                    }
                    else
                    if (prop.Name == "balls")
                    {
                        balls_array = new JArray((JArray)prop.Value);
                    }
                }
            }

        }

        public void initTableInfo()
        {
            foreach (JProperty prop in cueBall.Properties())
            {
                if (prop.Name == "position")
                {
                    Canvas.SetLeft(m_whiteBall, getLocation((JObject)prop.Value).X);
                    Canvas.SetTop(m_whiteBall, getLocation((JObject)prop.Value).Y);
                    
                }
                else if (prop.Name == "type")
                {
                    //m_whiteBall.Name = prop.Value.ToString();
                    m_whiteBall.Tag = prop.Value.ToString();
                }
            }
            for (int i = 0; i < balls_array.Count; i++)
            {
                JObject obj = (JObject)balls_array[i];
                Image ballImage = new Image() { Width = 15, Height = 15};
                foreach (JProperty prop in obj.Properties())
                {
                    if (prop.Name == "position")
                    {
                        Canvas.SetLeft(ballImage, getLocation((JObject)prop.Value).X);
                        Canvas.SetTop(ballImage, getLocation((JObject)prop.Value).Y);
                    }
                    else if (prop.Name == "type")
                    {
                        ballImage.Source = getImageFromType((int)prop.Value);
                        //ballImage.Name = prop.Value.ToString();
                        ballImage.Tag = prop.Value.ToString();
                        m_ballsList.Add(ballImage);
                        canvasTable.Children.Add(ballImage);
                    }
                }
            }
        }

        private ImageSource getImageFromType(int type)
        {
            string ballName = "";
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                case 11:
                default:
                    ballName = "NoBall";
                    break;
                case 3:
                    ballName = "WhiteBall";
                    break;
                case 4:
                    ballName = "RedBall";
                    break;
                case 5:
                    ballName = "YellowBall";
                    break;
                case 6:
                    ballName = "GreenBall";
                    break;
                case 7:
                    ballName = "BrownBall";
                    break;
                case 8:
                    ballName = "BlueBall";
                    break;
                case 9:
                    ballName = "PinkBall";
                    break;
                case 10:
                    ballName = "BlackBall";
                    break;
            }

            Image ball = (Image)FindName(ballName);
            return ball.Source;
        }

private Point getLocation(JObject obj)
        {
            Point point = new Point(0, 0);
            foreach (JProperty prop in obj.Properties())
            {
                if (prop.Name == "x")
                    point.X = getPosition((float)prop.Value, 0);
                else if (prop.Name == "z")
                    point.Y = getPosition((float)prop.Value, 1);
            }
            return point;
        }


        private int getPosition(float point, int type)
        {
            if (type == 0)
            {
                return (int)((point + 2.1) * 900 / 4.2 - 7.5);
            }
            else
            {
                return (int)((point + 1.05) * 450 / 2.1 - 7.5);
            }
        }

        private float setPosition(double point, int type)
        {
            if (type == 0)
            {
                return (float)((point + 7.5) * 4.2 / 900 - 2.1);
            }
            else
            {
                return (float)((point + 7.5) * 2.1 / 450 - 1.05);
            }
        }

        private void setLocation(JObject obj, Point point)
        {
            foreach (JProperty prop in obj.Properties())
            {
                if (prop.Name == "x")
                    prop.Value = setPosition(point.X, 0);
                else if (prop.Name == "z")
                    prop.Value = setPosition(point.Y, 1);
            }
        }

        private void configureJsonObject()
        {
            foreach (JProperty prop in m_currentChallenge.Properties())
            {
/*                if (prop.Name == "name")
                {
                    prop.Value = name_value.Text;
                }
                else if (prop.Name == "aimLengthMult")
                {
                    prop.Value = int.Parse(aim_length_value.Text);
                }
                else */if (prop.Name == "cueBall")
                {
                    prop.Value = cueBall;
                }
                else if (prop.Name == "balls")
                {
                    prop.Value = balls_array;
                }
/*                else if (prop.Name == "challengeType")
                {
                    prop.Value = challenge_type_value.SelectedIndex;
                }
                else if (prop.Name == "useMinScore")
                {
                    prop.Value = use_min_score_value.Checked;
                }
                else if (prop.Name == "minScore")
                {
                    prop.Value = int.Parse(min_score_value.Text);
                }
                else if (prop.Name == "player2Name")
                {
                    prop.Value = player2_name_value.Text;
                }
                else if (prop.Name == "player2Flag")
                {
                    prop.Value = player2_flag_value.SelectedIndex;
                }
                else if (prop.Name == "player2Score")
                {
                    prop.Value = int.Parse(player2_score_value.Text);
                }
                else if (prop.Name == "useBaseState")
                {
                    prop.Value = use_base_state_value.Checked;
                }
                else if (prop.Name == "spinText")
                {
                    prop.Value = spin_text_value.Text;
                }
                else if (prop.Name == "shotsTillSpinDisappear")
                {
                    prop.Value = int.Parse(shots_till_spin_disappear_value.Text);
                }
                else if (prop.Name == "challengeName")
                {
                    prop.Value = challenge_name_value.Text;
                }
                else if (prop.Name == "introDescription")
                {
                    prop.Value = description_value.Text;
                }
                else if (prop.Name == "introExplanation")
                {
                    prop.Value = explanation_value.Text;
                }
                else if (prop.Name == "useCoachDialog")
                {
                    prop.Value = use_coach_dialog_value.Checked;
                }
                else if (prop.Name == "skipFirstPart")
                {
                    prop.Value = skip_first_part_value.Checked;
                }
                else if (prop.Name == "skipSecondPart")
                {
                    prop.Value = skip_second_part_value.Checked;
                }
                else if (prop.Name == "playingAgainstPlayer")
                {
                    prop.Value = playing_against_player_value.Text;
                }
                else if (prop.Name == "minShotsTillRewind")
                {
                    prop.Value = int.Parse(min_shots_till_rewind_value.Text);
                }
                else if (prop.Name == "numRewinds")
                {
                    prop.Value = int.Parse(num_rewinds_value.Text);
                }
                else if (prop.Name == "leadOpponents")
                {
                    prop.Value = opponents_array;
                }
                else if (prop.Name == "challenges")
                {
                    prop.Value = challenges_array;
                }*/
            }

            foreach (JProperty prop in m_currentRound.Properties())
            {
                if (prop.Name == "position")
                {
                    JObject posObj = (JObject)prop.Value;
                    foreach (JProperty posProp in posObj.Properties())
                    {
                        if (posProp.Name == "x")
                            posProp.Value = float.Parse(txtPositionX.Text);
                        else if (posProp.Name == "y")
                            posProp.Value = float.Parse(txtPositionX.Text);
                    }
                }
/*                else if (prop.Name == "description")
                {
                    prop.Value = description_value.Text;
                }
                else if (prop.Name == "pointsDescrption")
                {
                    prop.Value = point_description_value.Text;
                }
                else if (prop.Name == "arenaName")
                {
                    prop.Value = arena_name_value.Text;
                }
                else if (prop.Name == "isOptional")
                {
                    prop.Value = isoptional_checkbox.Checked;
                }
                else if (prop.Name == "requiresMyTeam")
                {
                    prop.Value = require_my_team_checkbox.Checked;
                }
                else if (prop.Name == "minMyTeamMembers")
                {
                    prop.Value = int.Parse(min_my_team_members_value.Text);
                }
                else if (prop.Name == "showIntro")
                {
                    prop.Value = show_intro_checkbox.Checked;
                }
                else if (prop.Name == "showDelayedIntro")
                {
                    prop.Value = show_delayed_intro_checkbox.Checked;
                }
                else if (prop.Name == "startsWithTimeout")
                {
                    prop.Value = starts_with_timeout_checkbox.Checked;
                }
                else if (prop.Name == "prevGroup")
                {
                    JObject grObj = (JObject)prop.Value;
                    foreach (JProperty grProp in grObj.Properties())
                    {
                        if (grProp.Name == "index")
                            grProp.Value = int.Parse(prev_group_index_value.Text);
                    }
                }
                else if (prop.Name == "nextGroup")
                {
                    JObject grObj = (JObject)prop.Value;
                    foreach (JProperty grProp in grObj.Properties())
                    {
                        if (grProp.Name == "index")
                            grProp.Value = int.Parse(next_group_index_value.Text);
                    }
                }
                else if (prop.Name == "prevStage")
                {
                    JObject stObj = (JObject)prop.Value;
                    foreach (JProperty stProp in stObj.Properties())
                    {
                        if (stProp.Name == "index")
                            stProp.Value = int.Parse(prev_stage_index_value.Text);
                    }
                }
                else if (prop.Name == "type")
                {
                    prop.Value = type_combobox.SelectedIndex;
                }
                else if (prop.Name == "flag")
                {
                    prop.Value = flag_combobox.SelectedIndex;
                }
                else if (prop.Name == "minAvgDifference")
                {
                    prop.Value = float.Parse(min_avg_difference_value.Text);
                }
                else */if (prop.Name == "gameParams")
                {
                    JObject gpObj = (JObject)prop.Value;
                    foreach (JProperty gpProp in gpObj.Properties())
                    {
/*                        if (gpProp.Name == "challengeName")
                        {
                            gpProp.Value = challenge_name_combobox.SelectedItem.ToString();
                        }
                        else if (gpProp.Name == "pointDifferenceOneStars")
                        {
                            gpProp.Value = int.Parse(one_stars_value.Text);
                        }
                        else if (gpProp.Name == "pointDifferenceTwoStars")
                        {
                            gpProp.Value = int.Parse(two_stars_value.Text);
                        }
                        else if (gpProp.Name == "pointDifferenceTreeStars")
                        {
                            gpProp.Value = int.Parse(three_stars_value.Text);
                        }
                        else */if (gpProp.Name == "coins1Star")
                        {
                            gpProp.Value = int.Parse(txtOneStar.Text);
                        }
                        else if (gpProp.Name == "coins2Star")
                        {
                            gpProp.Value = int.Parse(txtTwoStars.Text);
                        }
                        else if (gpProp.Name == "coins3Star")
                        {
                            gpProp.Value = int.Parse(txtThreeStars.Text);
                        }
/*                        else if (gpProp.Name == "energyRequired")
                        {
                            gpProp.Value = int.Parse(energy_required_value.Text);
                        }
                        else if (gpProp.Name == "rewindCost")
                        {
                            gpProp.Value = int.Parse(rewind_cost_value.Text);
                        }
                        else if (gpProp.Name == "timeoutMinutes")
                        {
                            gpProp.Value = int.Parse(minutes_value.Text);
                        }
                        else if (gpProp.Name == "timeoutSeconds")
                        {
                            gpProp.Value = int.Parse(seconds_value.Text);
                        }
                        else if (gpProp.Name == "expGainWin")
                        {
                            gpProp.Value = int.Parse(exp_gain_win_value.Text);
                        }
                        else if (gpProp.Name == "expGainLose")
                        {
                            gpProp.Value = int.Parse(exp_gain_lose_value.Text);
                        }*/
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
//            if (canvasTable.CaptureMouse())
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
                m_tempDraggingImage = m_draggedImage;
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
            Image image = new Image() { Source = ((Image)btnImage).Source, Width =15, Height=15 };
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            canvasTable.Children.Add(image);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                if(m_tempDraggingImage != null)
                {
                    m_ballsList.Remove(m_tempDraggingImage);
                    canvasTable.Children.Remove(m_tempDraggingImage);
                    m_tempDraggingImage = null;
                }
                return;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save Table & Balls Info
            balls_array.Clear();
            for (int i = 0; i < m_ballsList.Count; i++)
            {
                balls_array.Add(new JObject(cueBall));
            }
            
            

            savePositionFromJsonToJObject(cueBall, new Point() { X = Canvas.GetLeft(m_whiteBall), Y = Canvas.GetTop(m_whiteBall) }, int.Parse((string)m_whiteBall.Tag));

            for (int i = 0; i < m_ballsList.Count; i++)
            {
                Point ballPos = new Point() { X = Canvas.GetLeft(m_ballsList[i]), Y = Canvas.GetTop(m_ballsList[i]) };
                savePositionFromJsonToJObject((JObject)balls_array[i], ballPos, int.Parse((string)m_ballsList[i].Tag));
            }

            // Round Info
            configureJsonObject();
            if(!m_isEditMode) // New
            {
                foreach (JProperty prop in m_currentRound.Properties())
                {
                    if (prop.Name == "stageId")
                    {
                        prop.Value = MainWindow.round_array.Count.ToString();
                    }
                    else if (prop.Name == "name")
                    {
                        prop.Value = "Level " + (MainWindow.round_array.Count + 1).ToString();
                    }
                }
                MainWindow.round_array.Add(m_currentRound);
            }
            else // Edit
            {
                MainWindow.round_array[m_selectedRoundIndex] = m_currentRound;
                //MainWindow.challenge_array[m_selectedChallengeIndex] = m_currentChallenge;
            }

            // Challenge Info
            MainWindow.challenge_array[m_selectedChallengeIndex] = m_currentChallenge;

            Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void savePositionFromJsonToJObject(JObject obj, Point position, int type)
        {
            foreach (JProperty prop in obj.Properties())
            {
                if (prop.Name == "type")
                {
                    prop.Value = type;
                }
                else if (prop.Name == "position")
                {
                    setLocation((JObject)prop.Value, position);
                }
            }
        }
    }
}