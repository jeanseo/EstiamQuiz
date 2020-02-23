using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;

namespace Assets.scripts.classes
{
    
    public class Parcours
    {
        public static List<Question> parcours = new List<Question>();
        private static int currentQuestion = 0;

        protected static bool created = false;

        protected static bool _isGameStarted;

        protected static int score;

        protected Firebase firebase;

        protected List<Question> questions = new List<Question>();
        private static int length;

        public Parcours()
        {
            //On vérifie s'il n'y a pas déjà une instance
            if (!created)
            {
                created = true;
                length = Globals.parcoursLength;
                Debug.Log("Initialisation du parcours");
                firebase = Firebase.CreateNew("https://quizestiam.firebaseio.com/");
                getQuestions();
                // A remplacer par l'appel API
                //string path = Application.streamingAssetsPath + "/parcours.json";
                //string JSONString = File.ReadAllText(path);
                //Debug.Log(JSONString);
                //DownloadedParcours _parcours = JsonConvert.DeserializeObject<DownloadedParcours>(JSONString);
                //parcours = _parcours.parcours;
                _isGameStarted = false;
            }
            
        }

        internal void indiceActivated()
        {
            parcours[currentQuestion].indiceActivated = true;
            Debug.Log("un indice a été demandé"+ parcours[currentQuestion].indiceActivated);
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
            Debug.Log("Question "+ currentQuestion +"/"+parcours.Count);
            return (currentQuestion == parcours.Count);
        }

        public void PassToNextQuestion()
        {
            //Ajout du score
            currentQuestion++;
        }

        public void Finish()
        {
            _isGameStarted = false;
            Globals.displayScore = false;
            Globals.instruction = "Votre score: "+score;
            Globals.displayInformation = true;

            //Afficher le score, et la saisie du nom
            Globals.displayForm = true;
            
        }

        public void DisplayRanking()
        {
            //TODO Afficher le classement
            // A remplacer par l'appel API: Récupération des 5 premiers
            try
            {
                string path = Application.streamingAssetsPath + "/HighScore.json";
                string JSONString = File.ReadAllText(path);
                Debug.Log("RECUPERATION DU CLASSEMENT");
                Debug.Log(JSONString);
                List<Score> ranking = JsonConvert.DeserializeObject<List<Score>>(JSONString);

                //Création du texte d'affichage des scores

                Debug.Log(ranking.Count);
                foreach (Score score in ranking)
                {
                    Globals.ranking += score.rank + " " + score.user + " " + score.score + "\n";
                }
                Globals.displayRanking = true;
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        public void SendScore()
        {
            
            //Récupérer le nom, et envoyer le nom et le score par l'api
            Debug.Log(score);
            if (Globals.playerName == string.Empty)
                Globals.playerName = "Anonymous";
            Debug.Log(Globals.playerName);

            DisplayRanking();

        }

        public void displayForm()
        {
            Globals.instruction = "Envoies ton score";
            Globals.displayScore = false;
            Globals.displayInformation = true;
            Globals.displayForm = true;
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

        public int getScore()
        {
            return score;
        }

        public void SetScore()
        {
            if (!parcours[currentQuestion].indiceActivated)
                score += Globals.goodAnswerPoints;
            else
                score += Globals.indiceAnswerPoints;

            Debug.Log("SCORE:" + score);
        }

        public void SetPenalty()
        {
            if (score > 0)
                score -= Globals.badAnswerPoints;
            if (score < 0)
                score = 0;
            Debug.Log("SCORE:" + score);

        }

        public void getQuestions()
        {
            // On se place au niveau de la collection "questions"
            Firebase questionFirebase = firebase.Child("questions");

            // On ajoute quelques callbacks savoir si tout ce passe bien ou pas.
            questionFirebase.OnGetSuccess += GetOKHandler;
            questionFirebase.OnGetFailed += GetFailHandler;

            questionFirebase.GetValue();
        }

        void GetOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            // La requête a fonctionnée et on a les données demandées
            Debug.Log("[OK] Get from key: " + sender.FullKey);
            Debug.Log("[OK] Raw Json: " + snapshot.RawJson);
            var questionList = (Dictionary<string, object>)Json.Deserialize(snapshot.RawJson);
            var getQuestions = new List<Question>();

            foreach (KeyValuePair<string, object> json in questionList)
            {
                var questionItem = (Dictionary<string, object>)json.Value;

                // Venant d'un Json, tout est sous forme de string
                string _name = questionItem["name"].ToString();
                string _question = questionItem["question"].ToString();
                string _indice = questionItem["indice"].ToString();
                string _answer = questionItem["answer"].ToString();

                // On rassemble le tout dans un même object
                // La méthode unchecked() étant utilisé pour ignore les dépassements, 
                //  si un double est trop grand pour le int.
                // voir https://docs.microsoft.com/fr-fr/dotnet/csharp/language-reference/keywords/unchecked
                questions.Add(
                    new Question()
                    {
                        name = _name,
                        question = _question,
                        indice = _indice,
                        answer = _answer
                    });
            }
            pickQuestions();
            Debug.Log("Nombre de questions:" + parcours.Count);
        }

        private void pickQuestions()
        {
            //Si la longueur demandée du questionnaire est supérieure aux nombre de questions disponibles
            if (length > questions.Count)
                length = questions.Count;
            System.Random rnd = new System.Random();

            for (int i = 0; i < length;)
            {
                // Onc choisit un index aléatoirement
                int r = rnd.Next(questions.Count);
                //Si l'item n'est pas déjà présent, on l'ajoute
                if (!parcours.Contains(questions[r]))
                {
                    parcours.Add(questions[r]);
                    i++;
                }
            }
        }

        void GetFailHandler(Firebase sender, FirebaseError err)
        {
            // S'il y a eut un problème de connexion ou de validation
            Debug.Log("[ERR] Get from key: " + sender.FullKey + ",  " + err.Message
                + " (" + (int)err.Status + ")");
        }

    }

    class Score
    {
        public string user { get; set; }
        public int score { get; set; }
        public int rank { get; set; }
    }

    class DownloadedParcours
    {
        public string id { get; set; }
        public Question question { get; set; }

    }

}
