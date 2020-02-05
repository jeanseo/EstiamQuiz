using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.classes
{
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public string indice { get; set; }
        public string answer { get; set; }

        public async Task GetQuestion(string doorId)
        {
            // Appel API pour récupérer la question de la porte
            switch (doorId)
            {
                case "Entrée":
                    question = "Question de la porte d'entrée";
                    indice = "Indice de la porte d'entrée";
                    answer = "A";
                    break;
                case "A":
                    question = "Question de la porte A";
                    indice = "Indice de la porte A";
                    answer = "B";
                    break;
            }
        }

    }

}
