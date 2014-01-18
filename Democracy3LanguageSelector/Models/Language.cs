using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Democracy3LanguageSelector
{
    public class Language
    {
        public Language(string code)
        {
            this.Code = code;
        }

        public string Code { get; set; }
        public string Name
        {
            get
            {
                string name = this.Code;

                if (this.Code.Length == 2)
                    name = new CultureInfo(this.Code).DisplayName;
                else
                {
                    var code = this.Code.Split('_').First();
                    name = new CultureInfo(code).DisplayName;
                }

                return name;
            }
        }
    }
}
