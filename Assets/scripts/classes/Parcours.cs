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

        protected static Firebase firebase;

        protected List<Question> questions = new List<Question>();
        private static int length;

        public static Score score = new Score();

        public Parcours()
        {
            //On vérifie s'il n'y a pas déjà une instance
            if (!created)
            {
                created = true;
                length = Globals.parcoursLength;
                Debug.Log("Initialisation du parcours");
                firebase = Firebase.CreateNew(Globals.firebaseAddress);
                getQuestions();
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
            Globals.instruction = "Votre score: "+score.points;
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
                Debug.Log("RECUPERATION DU CLASSEMENT");
                Classement classement = new Classement();
                classement.fetchHighScores(5);
                
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        public void SendScore()
        {
            
            //Récupérer le nom, et envoyer le nom et le score par l'api
            Debug.Log(score.points);
            if (Globals.playerName == string.Empty)
                Globals.playerName = "Anonymous";
            Debug.Log(Globals.playerName);
            score.name = Globals.playerName;
            SubmitScore();

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
        }

        public bool IsGameStarted()
        {
            return _isGameStarted;
        }

        public int getScore()
        {
            return score.points;
        }

        public void SetScore()
        {
            if (!parcours[currentQuestion].indiceActivated)
                score.points += Globals.goodAnswerPoints;
            else
                score.points += Globals.indiceAnswerPoints;

            Debug.Log("SCORE:" + score.points);
        }

        public void SetPenalty()
        {
            if (score.points > 0)
                score.points -= Globals.badAnswerPoints;
            if (score.points < 0)
                score.points = 0;
            Debug.Log("SCORE:" + score.points);

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
                Debug.Log("index choisi:" + r);
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

        public void SubmitScore()
        {

            Debug.Log("coucou");
            // Pour convertir un double en forçant le point et non la virgule vous 
            //  pouvez utiliser double.ToString("F1")
            string jsonToSend = string.Format(
                "{{ \"name\": \"{0}\", \"points\": {1} }}",
                score.name, score.points);

            // On se place au niveau de la collection "highscores" qui sera créé 
            //  si elle n'existe pas
            Firebase scoresFirebase = firebase.Child("highscores");

            // On ajoute quelques callbacks savoir si tout ce passe bien ou pas.
            // Il est possible de mettre les callback sur le parent "firebase" mais 
            //  il aurait fallut demander à ce que la collection enfant hérite des 
            //  callbacks via firebase.Child("highscores", true);
            scoresFirebase.OnPushSuccess += PushOKHandler;
            scoresFirebase.OnPushFailed += PushFailHandler;

            // Enfin on push le tout, en spéficiant qu'on passe un json
            // Push(string json, bool isJson, string param = "")
            scoresFirebase.Push(jsonToSend, true);
        }

        void PushOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            // Dans le cas où la requête a fonctionné
            Debug.Log("[OK] Push from key: " + sender.FullKey);
            DisplayRanking();

        }

        void PushFailHandler(Firebase sender, FirebaseError err)
        {
            // S'il y a eut un problème de connexion ou de validation
            Debug.Log("[ERR] Push from key: " + sender.FullKey + ", " + err.Message
                + " (" + (int)err.Status + ")");
        }

    }

    class DownloadedParcours
    {
        public string id { get; set; }
        public Question question { get; set; }

    }

}
