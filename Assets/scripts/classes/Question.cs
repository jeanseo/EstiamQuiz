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
        public string name { get; set; }
        public string question { get; set; }
        public string indice { get; set; }
        public string answer { get; set; }
        public bool indiceActivated { get; set; }

        public Question()
        {
            indiceActivated = false;
        }
    }

    

}
