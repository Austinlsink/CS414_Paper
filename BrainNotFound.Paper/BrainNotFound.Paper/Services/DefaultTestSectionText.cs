using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.Services
{
    public static class DefaultTestSectionText
    {
        public static class Instruction
        {
            public const string TrueFalse = "Determine if each statement is True or False.";
            public const string MultipleChoice = "Choose the best option(s) for each question.";
            public const string Matching = "Match each concept to it's correct answer. Answers may be used once or more than once.";
            public const string FillInTheBlank = "Fill in the blank(s) with the correct word(s).";
            public const string Essay = "Answer each essay question to the best of your ability. Partial credit will be given.";
        }

        public static class Header
        {
            public const string TrueFalse = "True / False";
            public const string MultipleChoice = "Multiple Choice";
            public const string Matching = "Matching";
            public const string FillInTheBlank = "Fill in the Blank";
            public const string Essay = "Essay";

            public static string Get(string questionType)
            {
                switch (questionType)
                {
                    case QuestionType.TrueFalse:
                        return TrueFalse;
                    case QuestionType.Matching:
                        return Matching;
                    case QuestionType.FillInTheBlank:
                        return FillInTheBlank;
                    case QuestionType.MultipleChoice:
                        return MultipleChoice;
                    case QuestionType.Essay:
                        return Essay;
                }

                return null;
            }
        }



    }
}
