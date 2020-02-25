using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.classes
{
    public static class Globals
    {
        public static int currentLevel = 0;
        public static string correctAnswer = "Entrée";
        public static string lastDoor = "Entrée";
        public static string instruction = "Trouve le panneau Estiam pour commencer";
        public const string firstDoorID = "Entrée";
        public static bool displayInformation = true;
        public static int goodAnswerPoints = 10;
        public static int indiceAnswerPoints = 6;
        public static int badAnswerPoints = 1;
        public static string ranking = string.Empty;
        public static bool displayScore = true;
        public static bool displayRanking = false;
        public static bool displayForm = false;
        public static string playerName = string.Empty;
        public static int parcoursLength = 5;
        public static string firebaseAddress = "https://quizestiam.firebaseio.com/";

    }
}
