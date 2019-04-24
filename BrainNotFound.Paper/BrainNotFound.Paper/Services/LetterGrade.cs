namespace BrainNotFound.Paper.Services
{
    public static class LetterGrade
    {
        public static char CalculateLetterGrade(float grade, float totalPoints)
        {
            char letterGrade;
            float value = grade / totalPoints * 100;
            if (value > 90)
            {
                letterGrade = 'A';
            }
            else if (value > 80)
            {
                letterGrade = 'B';
            }
            else if (value > 70)
            {
                letterGrade = 'C';
            }
            else if (value > 60)
            {
                letterGrade = 'D';
            }
            else
            {
                letterGrade = 'F';
            }

            return letterGrade;
        }
    }
}
