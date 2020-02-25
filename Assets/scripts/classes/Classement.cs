using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;
using Newtonsoft.Json;


namespace Assets.scripts.classes
{
    class Classement
    {
        protected int length;
        protected Firebase firebase;
        public List<Score> allScores { get; set; }

        public void fetchHighScores(int length)
        {
            Debug.Log("recuperation du classement par firebase");
            this.length = length;

            firebase = Firebase.CreateNew(Globals.firebaseAddress);

            // On se place au niveau de la collection "highscores"
            Firebase scoresFirebase = firebase.Child("highscores");

            // On ajoute quelques callbacks savoir si tout ce passe bien ou pas.
            scoresFirebase.OnGetSuccess += GetOKHandler;
            scoresFirebase.OnGetFailed += GetFailHandler;

            // On récupère tous les highscores, 
            // On trie par "level" mais on pourrait ne pas le faire étant spécifié dans les règles, 
            //  puis on recupère que les 10 derniers étant la seule manière de faire un tri décroisant.
            scoresFirebase.GetValue(FirebaseParam.Empty.OrderByChild("points").LimitToLast(5));


        }

        void GetOKHandler(Firebase sender, DataSnapshot snapshot)
        {
          
            var scorelist = (Dictionary<string, object>)Json.Deserialize(snapshot.RawJson);
            Debug.Log(snapshot.RawJson);
            allScores = new List<Score>();

            foreach (KeyValuePair<string, object> json in scorelist)
            {
                var score = (Dictionary<string, object>)json.Value;

                // Venant d'un Json, tout est sous forme de string
                
                var points = (long)score["points"];
                string name = score["name"].ToString();

                

                // On rassemble le tout dans un même object
                allScores.Add(
                    new Score()
                    {
                        points = unchecked((int)points),
                        name = name
                    });
            }
            List<Score> highscores = allScores.OrderByDescending(o => o.points).ToList();
            Debug.Log(JsonConvert.SerializeObject(highscores));
            //Création du texte d'affichage des scores
            int i = 1;
            foreach (Score score in highscores.Take(5))
            {
                Globals.ranking += i + " " + score.name + " " + score.points + "\n";
                i++;
            }
            Globals.displayRanking = true;
        }

        void GetFailHandler(Firebase sender, FirebaseError err)
        {
            // S'il y a eut un problème de connexion ou de validation
            Debug.Log("[ERR] Get from key: " + sender.FullKey + ",  " + err.Message
                + " (" + (int)err.Status + ")");
        }

    }

    
}
