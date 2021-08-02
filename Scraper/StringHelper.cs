using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper
{
    public class StringHelper
    {

        public static string SemAcentos2(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            else
            {
                string ca = "áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇªº?ñÑ";
                string sa = "aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC   nN";
                for (int k = 0; k < ca.Length; k++)
                {
                    text = text.Replace(ca[k], sa[k]);
                }
                int idx = text.IndexOf((char)176);
                if (idx >= 0)
                {
                    text = text.Replace((char)176, (char)32);
                }
                int idxp = text.IndexOf((char)21);
                if (idxp >= 0)
                {
                    text = text.Replace((char)21, (char)32);
                }
                return text;
            }
        }

        public static string SemCaracteresEspeciais(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            else
            {
                string ca = ",.;/°!@#$%¨&*()_+=-§/*-++.<>:?`´{[}]º^~/\\|'\"";
                for (int k = 0; k < ca.Length; k++)
                {
                    text = text.Replace(ca[k], ' ');
                }
                int idx = text.IndexOf((char)176);
                if (idx >= 0)
                {
                    text = text.Replace((char)176, (char)32);
                }
                int idxp = text.IndexOf((char)21);
                if (idxp >= 0)
                {
                    text = text.Replace((char)21, (char)32);
                }
                return text;
            }
        }
    }
}
