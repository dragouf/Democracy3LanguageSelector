using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Democracy3LanguageSelector
{
    public class Project
    {
        public Project()
        {
            this.Teams = new List<string>();
            this.Resources = new List<Resource>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string Tags { get; set; }
        public List<string> Teams { get; set; }
        public List<Resource> Resources { get; set; }

        public List<Language> Languages
        {
            get
            {
                var languages = new List<Language>();

                if(Teams != null)
                    Teams.ForEach(t => languages.Add(new Language(t)));

                return languages;
            }
        }
    }
}
