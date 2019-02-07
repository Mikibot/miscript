using Miki.Localization;
using Miki.Localization.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiScript.Parser.Exceptions
{
    public class ParseException : LocalizedException
    {
        public override IResource LocaleResource => new LanguageResource("error_miscript_parse", Message);

        public ParseException(string message)
            
        {

        }
    }
}
