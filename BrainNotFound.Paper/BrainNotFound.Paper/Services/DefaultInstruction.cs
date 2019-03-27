using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.Services
{
    public static class DefaultInstruction
    {
        public const string TrueFalse = "Determine if each statement is True or False.";
        public const string MultipleChoice  = "Chose the best choice for each question.";
        public const string Matching        = "Match each concept to it's corrent.";
        public const string FillInTheBlank  = "Fill in the blank with the correct word.";
        public const string Essay = "Answer each essay question.";
    }
}
