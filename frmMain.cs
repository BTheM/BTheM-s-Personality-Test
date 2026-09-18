using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using PersonalityTest;

using System.IO;
using System.Security.Policy;
using PersonalityTest.Properties;

namespace PersonalityTest
{
    public partial class frmMain : Form
    {
        private SoundPlayer player = new SoundPlayer(Resources.Webinar___12_Zetta_禅);

        List<Question> questions;
        int[] questionAnswer;
        int currentQuestion = 0;
        

        public void UpdateQuestionID()
        {
            lblQuestionID.Text = (currentQuestion + 1) + "/" + questions.Count;
        }

        public void UpdateQuestionTextFace()
        {
            lblQuestionText.Text = questions[currentQuestion].QuestionTextFace;
        }

        public void UpdateTrackBar()
        {
            tbMainSlider.Value = questions[currentQuestion].ScoreTrackBar;
        }

        public void UpdateTrackBarValue()
        {
            questions[currentQuestion].ScoreTrackBar = tbMainSlider.Value;
        }

        public void EnableButtons()
        {
            btnNext.Enabled = currentQuestion < questions.Count - 1;
            btnBack.Enabled = currentQuestion > 0;
            btnFinishtest.Enabled = currentQuestion == questions.Count - 1;
        }

        public void UpdatePersonalityScore()
        {
            questionAnswer[currentQuestion] = tbMainSlider.Value;
        }

        public void GetTotalPersonalitisScore(ref float Personality1, ref float Personality2, ref float Personality3)
        {

            for (int i = 0; i < questionAnswer.Length; i++)
            {
                int answer = questionAnswer[i];
                switch (answer)
                {
                    case 0:
                        {
                            Personality1 += questions[i].AnswerDisagreeP1;
                            Personality2 += questions[i].AnswerDisagreeP2;
                            Personality3 += questions[i].AnswerDisagreeP3;
                            break;
                        }

                    case 1:
                        {
                            Personality1 += questions[i].AnswerNaturalP1;
                            Personality2 += questions[i].AnswerNaturalP2;
                            Personality3 += questions[i].AnswerNaturalP3;
                            break;
                        }

                    case 2:
                        {
                            Personality1 += questions[i].AnswerAgreeP1;
                            Personality2 += questions[i].AnswerAgreeP2;
                            Personality3 += questions[i].AnswerAgreeP3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            
        }

        public void UpdateImage()
        {
            pbMainImage.ImageLocation = questions[currentQuestion].QuestionImagePath;
        }

        public void ShowTestResultsScreen()
        {
            //Hide items
            tbMainSlider.Visible = false;
            lblQuestionID.Visible = false;
            lblAgree.Visible = false;
            lblNatural.Visible = false;
            lbldisagree.Visible = false;
            btnBack.Visible = false;
            btnFinishtest.Visible = false;
            btnNext.Visible = false;

            //Show items
            btnReturnToTest.Visible = true;
            lblTextResult.Visible = true;
            lblPersonenjoy.Visible = true;
        }

        public void ReturnToTestScreen()
        {
            //Hide items
            btnReturnToTest.Visible = false;
            lblTextResult.Visible = false;
            lblPersonenjoy.Visible = false;

            //Show items
            tbMainSlider.Visible = true;
            lblQuestionID.Visible=true;
            lblAgree.Visible = true;
            lblNatural.Visible = true;
            lbldisagree.Visible = true;
            btnBack.Visible = true;
            btnFinishtest.Visible = true;
            btnNext.Visible = true;
        }

        public int ReturnHighestAnsewr(int Score1, int Score2, int Score3)
        {
            int HighestScore = Math.Max(Math.Max(Score1, Score2),Score3);

            return HighestScore;
        }

        public void CalculateMaxScore(ref float P1MaxScore, ref float P2MaxScore, ref float P3MaxScore)
        {
            //personality1 percentage total
            foreach(Question Q1 in questions)
            {
                P1MaxScore += ReturnHighestAnsewr(Q1.AnswerAgreeP1, Q1.AnswerNaturalP1, Q1.AnswerDisagreeP1);
            }

            //personality2 percentage total
            foreach (Question Q1 in questions)
            {
                P2MaxScore += ReturnHighestAnsewr(Q1.AnswerAgreeP2, Q1.AnswerNaturalP2, Q1.AnswerDisagreeP2);
            }

            //personality3 percentage total
            foreach (Question Q1 in questions)
            {
                P3MaxScore += ReturnHighestAnsewr(Q1.AnswerAgreeP3, Q1.AnswerNaturalP3, Q1.AnswerDisagreeP3);
            }
        }

        public bool isDifferenceHigherThan(float Num1, float Num2, int higherthanbyNum)
        {
            return (higherthanbyNum >= Math.Abs(Num1 - Num2));
        }

        public short ReturnTestScore()
        {

            float Personality1 = 0;
            float Personality2 = 0;
            float Personality3 = 0;

            float P1MaxScore = 0;
            float P2MaxScore = 0;
            float P3MaxScore = 0;

            CalculateMaxScore(ref P1MaxScore, ref P2MaxScore, ref P3MaxScore);
            GetTotalPersonalitisScore(ref Personality1, ref Personality2, ref Personality3);

            float HighestScore = (Math.Max(Math.Max(Personality1, Personality2), Personality3));
            float LowestScore = (Math.Min(Math.Min(Personality1, Personality2), Personality3));
            float MiddleScore = Personality1 + Personality2 + Personality3 - HighestScore - LowestScore;



            //float P1ScorePercentage = (Personality1 / P1MaxScore) * 100;
            //float P2ScorePercentage = (Personality2 / P2MaxScore) * 100;
            //float P3ScorePercentage = (Personality3 / P3MaxScore) * 100;

            //use this when debugging only
            MessageBox.Show("P1: " + Personality1.ToString() + "\nP2: " + Personality2.ToString() + "\nP3: " + Personality3.ToString() + "\n" + P1MaxScore + " " + P2MaxScore + " " + P3MaxScore);


            if (isDifferenceHigherThan(HighestScore, MiddleScore, 10) && isDifferenceHigherThan(HighestScore, LowestScore, 10))
            {
                return 6;
            }

            if (HighestScore == Personality1 && !isDifferenceHigherThan(Personality1, Personality2, 20) && !isDifferenceHigherThan(Personality1, Personality3, 20))
            {
                return 0;
            }

            if(HighestScore == Personality2 && !isDifferenceHigherThan(Personality2, Personality1, 20) && !isDifferenceHigherThan(Personality2, Personality3, 20))
            {
                return 1;
            }

            if (HighestScore == Personality3 && !isDifferenceHigherThan(Personality3, Personality1, 20) && !isDifferenceHigherThan(Personality3, Personality2, 20))
            {
                return 2;
            }

            if ((HighestScore == Personality1 || HighestScore == Personality3) && isDifferenceHigherThan(Personality1,Personality3, 20))
            {
                return 3;
            }

            if ((HighestScore == Personality2 || HighestScore == Personality1) && isDifferenceHigherThan(Personality2, Personality1, 20))
            {
                return 4;
            }

            if ((HighestScore == Personality2 || HighestScore == Personality3) && isDifferenceHigherThan(Personality2, Personality3, 20))
            {
                return 5;
            }

                return 7;
        }

        public void TestResults()
        {
            switch(ReturnTestScore())
            {
                //good
                case 0:
                    {

                        lblTextResult.Text = "Purely good" + "\nYou are the good guy." +
                            " Love and peace is how you wired" +
                            "\nThere is no place for hatred in your heart as you only wish a good life to others";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nHelping others \nAdopting orphans of a war you started\nGiving money to the homeless\nNot being a gnome";
                        pbMainImage.Image = Resources.superman_meme;
                        break;
                    }
                    //natural
                case 1:
                    {
                        lblTextResult.Text = "Natrual" + "\nYou are very normal. Too normal to the point where even a normal person would feel uncomfortable around you." +
                            "\nEven the word normie doesn't apply to you as you are more normal than a normie";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nSleeping at exactly same hour and second everyday" +
                            "\nEating a green apple" +
                            "Going to a random office and having a very normal chat at the water cooler with others.";
                        pbMainImage.Image = Resources.normalzuck;
                        break;
                    }
                    //evil
                case 2:
                    {
                        lblTextResult.Text = "Purely evil." + "\nYou know no peace. No love. Only hatred is at your heart." +
                            "\nYou want to watch others hurt and places ruined" +
                            "\nWatching others in pain and suffering is the only thing you enjoy in this world.";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nKilling others\nShouting at your wife\nThrowing rocks at the homeless" +
                            "\nBeing a Gnome";
                        pbMainImage.Image = Resources.maxresdefault;
                        break;
                    }
                    //good + evil
                case 3:
                    {
                        lblTextResult.Text = "Normal" + "\nYou are a normal guy. " +
                            "\nSometimes you do good things and sometimes you do bad things" +
                            "\nYour kinda boring ngl";
                        lblPersonenjoy.Text = "Things a person like you would like to do: /nLeave work early \nGoing to the gym\nEating a pizza\nTaking a walk.\nOpening social media and scrolling for hours";
                        pbMainImage.Image = Resources.whiteguystanding;
                        break;
                    }
                    //good + natural
                case 4:
                    {
                        lblTextResult.Text = "Good" + "\nClose to natural but lean more into being a good person" +
                            "\nYou don't go out of your way to do good things but when you have the chance to do something good you would do it";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nGiving a coin to the homeless guy on your way to work\nBuying a box of chocolate for your mom" +
                            "\nRespectful of others desicions" +
                            "\nChating with the random guy at your work who only come to stand at the water cooler";
                        pbMainImage.Image = Resources.ryan_gosling;
                        break;
                    }
                    //evil + natural
                case 5:
                    {
                        lblTextResult.Text = "Bad" + "\nClose to natural but lean more into being a bad person" +
                            "\nYou don't go out of your way to harm others but you might do something bad too many times while refusing to accept that you are being bad without realizing it";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nLittering\nSaying slurs because you think it is funny in a Counter-Strike match\nBefriending a Gnome";
                        pbMainImage.Image = Resources.emodog;
                        break;
                    }
                //everything above 50%
                case 6:
                    {
                        lblTextResult.Text = "Amalgamation" + "\nYou have many conflicting personalities and none of them are in dominant power" +
                            "\nThey will fight for eternity over who will control your body";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nA lot of things";
                        pbMainImage.Image = Resources.amalgamationpepefrog;
                        break;
                    }
                case 7:
                    {
                        lblTextResult.Text = "Void" +
                            "\nYou somehow managed to score very low on every personality" +
                            "\nMaking you void";
                        lblPersonenjoy.Text = "Things a person like you would like to do: \nNothing.";
                        pbMainImage.Image = Resources.humanityphantom;
                        break;
                    }
                default:
                    {
                        lblTextResult.Text = "ERROR";
                        lblPersonenjoy.Text = "ERROR";
                        break;
                    }
            }

        }

        public frmMain()
        {
            InitializeComponent();

            string json = File.ReadAllText("PersonalityTestQuestionsList.json");

            questions = JsonConvert.DeserializeObject<List<Question>>(json);


            int nrOfQuestions = questions.Count;
            questionAnswer = new int[nrOfQuestions];

            //use this when you want to debugg questions
            //foreach (Question question in questions)
            //{
            //    Console.WriteLine(question.ID);
            //    Console.WriteLine(question.QuestionTextFace);
            //    Console.WriteLine(question.QuestionImagePath);
            //}

            player.Play();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            UpdateQuestionID();
            UpdateQuestionTextFace();
            UpdateTrackBar();
            EnableButtons();
            UpdateImage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //leave this at start before changing the value of currentQuestion
            UpdateTrackBarValue();
            UpdatePersonalityScore();
            currentQuestion++;
            if (currentQuestion >= questions.Count - 1)
            {
                currentQuestion = questions.Count - 1;
            }
            UpdateQuestionID();
            UpdateQuestionTextFace();
            UpdateImage();

            EnableButtons();
            //leave this at the end
            UpdateTrackBar();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //leave this at start before changing the value of currentQuestion
            UpdateTrackBarValue();
            UpdatePersonalityScore();
            currentQuestion--;
            if (currentQuestion < 0)
            {
                currentQuestion = 0;
            }
            UpdateQuestionID();
            UpdateQuestionTextFace();
            UpdateImage();

            EnableButtons();
            UpdateTrackBar();
        }

        private void btnFinishtest_Click(object sender, EventArgs e)
        {
            UpdatePersonalityScore();
            ShowTestResultsScreen();
            TestResults();
            //MessageBox.Show("P1: " + Personality1.ToString() + "\nP2: " + Personality2.ToString() + "\nP3: " + Personality3.ToString() +"\n" + P1MaxScore+ " " + P2MaxScore + " " + P3MaxScore);
        }

        private void btnReturnToTest_Click(object sender, EventArgs e)
        {
            ReturnToTestScreen();
            UpdateQuestionTextFace();
            UpdateImage();
        }
    }
}