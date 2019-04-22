using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class StudentEssayAnswer : StudentAnswer
	{
		public string EssayAnswerGiven {get; set;}
        public string Comments { get; set; }
        public int PointsEarned { get; set; }
	}
}