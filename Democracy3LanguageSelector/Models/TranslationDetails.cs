using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Democracy3LanguageSelector
{
    public class TranslationDetails
    {
        public List<string> Coordinators { get; set; }
        public List<string> Translators { get; set; }
        public List<string> Reviewers { get; set; }
        public int Translated_segments { get; set; }
        public int Untranslated_segments { get; set; }
        public int Reviewed_segments { get; set; }
        public int Total_segments { get; set; }
        public int Translated_words { get; set; }

        public double PercentProgression
        {
            get
            {
                if (this.Total_segments != 0)
                    return this.Translated_segments * 100 / this.Total_segments;
                else
                    return 0;
            }
        }
    }
}
