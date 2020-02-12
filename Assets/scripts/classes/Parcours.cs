using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.scripts.classes
{
    
    public class Parcours
    {
        public static List<Question> parcours = new List<Question>();
        private static int currentQuestion = 0;

        public static bool _isGameStarted { get; internal set; }

        public Parcours()
        {
            if (parcours.Count == 0)
            {
                Debug.Log("Initialisation du parcours");
                // A remplacer par l'appel API
                string path = Application.streamingAssetsPath + "/parcours.json";
                string JSONString = File.ReadAllText(path);
                Debug.Log(JSONString);
                parcours = JsonConvert.DeserializeObject<List<Question>>(JSONString);
                Debug.Log(parcours.Count);
                _isGameStarted = false;
            }
            
        }

        public Question getCurrentQuestion()
        {
            return parcours[currentQuestion];
        }

        public bool IsCorrectAnswer(string doorId)
        {
            return (doorId == getCurrentQuestion().answer);
        }

        public bool IsLastAnswer()
        {
            return (currentQuestion+1 == parcours.Count);
        }

        public void PassToNextQuestion()
        {
            //Ajout du score
            currentQuestion++;
        }

        public void Finish()
        {
            //TODO Calculer le score
            //TODO Saisir le nom
            //TODO Afficher le classement
            Globals.instruction = "GAME OVER!!";
        }

        public void StartGame()
        {
            _isGameStarted = true;
            //Démarrer chrono
            //Afficher chrono
            //Afficher score
        }

        public bool IsGameStarted()
        {
            return _isGameStarted;
        }
    }
    
}
