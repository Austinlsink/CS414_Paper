using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class Question
	{
        [Key]
		public long QuestionId { get; set; } //primary key
		[Required]
		public int Index      { get; set; }
		public int PointValue { get; set; }
        [Required]
		public string Content { get; set; }
        [Required]
        public string QuestionType { get; set; }

        //foreign keys

        public long TestSectionId  { get; set; }
		[ForeignKey("TestSectionId")]
		public TestSection TestSection { get; set; }

        public List<StudentAnswer> StudentAnswers { get; set; }
        public List<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        public List<MatchingQuestionSide> MatchingQuestionSides { get; set; }
        public List<MatchingAnswerSide>   MatchingAnswerSides   { get; set; }
        public List<FillInTheBlankQuestion>   FillInTheBlankQuestions   { get; set; }


        // Holds the student answer for populating take test
        [NotMapped]
        public bool? studentTrueFalseAnswer { get; set; }

        [NotMapped]
        public List<StudentMultipleChoiceAnswer> studentMultipleChoiceAnswers { get; set; }

        [NotMapped]
        public string studentEssayAnswer { get; set; }

        [NotMapped]
        public List<StudentMatchingAnswer> studentMatchingAnswers { get; set; }

        // Returns a Json object to be used in responses to Ajax calls for Multiple Choice Questions
        public JObject GetJsonMultipleChoice()
        {
            // Create the response Object
            dynamic question = new JObject();
            question.pointValue = PointValue;
            question.content = Content;
            question.questionId = QuestionId;
            question.sectionId = TestSectionId;

            JArray MCOptions = new JArray();
            foreach (var option in MultipleChoiceAnswers)
            {
                dynamic MCOption = new JObject();
                MCOption.multipleChoiceAnswerId = option.MultipleChoiceAnswerId;
                MCOption.isCorrect = option.IsCorrect;
                MCOption.optionContent = option.MultipleChoiceAnswerOption;

                MCOptions.Add(MCOption);
            }
            question.multipleChoiceAnswers = MCOptions;

            return question;
        }

        // Returns a Json object to be used in responses to Ajax calls for Multiple Choice Questions
        public JObject GetJsonMatching()
        {
            // Create the response Object
            dynamic question = new JObject();
            question.pointValue = PointValue;
            question.content = Content;
            question.questionId = QuestionId;
            question.sectionId = TestSectionId;

            // format the mathching groups
            JArray matchingGroups = new JArray();
            foreach (var answer in MatchingAnswerSides)
            {
                string answerContent = answer.MatchingAnswer;

                var MatchToAnswer = MatchingQuestionSides.Where(mqs => mqs.MatchingAnswerSideId == answer.MatchingAnswerSideId).ToList();

                JArray jMatchs = new JArray();

                foreach(var match in MatchToAnswer)
                {
                    dynamic Jmatch = new JObject();
                    Jmatch.match = match.Content;
                    Jmatch.answer = answerContent;
                    jMatchs.Add(Jmatch);
                }

                dynamic matchingGroup = new JObject();
                matchingGroup.matchAnswer = answerContent;
                matchingGroup.matchs = jMatchs;

                matchingGroups.Add(matchingGroup);

            }
            question.matchingGroups = matchingGroups;

            // return as JObject
            return question;
        }
    }
}