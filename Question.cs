using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityTest
{
    public class Question
    {
        public int ID { get; set; }
        public string QuestionTextFace { get; set; }
        public int AnswerAgreeP1 { get; set; }
        public int AnswerNaturalP1 { get; set; }
        public int AnswerDisagreeP1 { get; set; }

        public int AnswerAgreeP2 { get; set; }
        public int AnswerNaturalP2 { get; set; }
        public int AnswerDisagreeP2 { get; set; }

        public int AnswerAgreeP3 { get; set; }
        public int AnswerNaturalP3 { get; set; }
        public int AnswerDisagreeP3 { get; set; }

        public int ScoreTrackBar { get; set; }

        public string QuestionImagePath { get; set; }
    }
}
