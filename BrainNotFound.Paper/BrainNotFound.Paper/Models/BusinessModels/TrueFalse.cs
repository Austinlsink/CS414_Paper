using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class TrueFalse : Question
      {
        //ask about this for required attributes
	    public bool TrueFalseAnswer { get; set; }

        // Returns a Json object to be used in responses to Ajax calls
        public JObject ToJObject()
        {
            // Create the response Object
            dynamic question = new JObject();
            question.pointValue = PointValue;
            question.content = Content;
            question.questionId = QuestionId;
            question.sectionId = TestSectionId;
            question.answer = TrueFalseAnswer;

            return question;
        }
    }
}