using System;

namespace Util
{
    public static class UDB
    {
        public static string MontarUpdateCampos(string campos)
        {
            string sOrigem = campos;
            string sDestino = "";
            string s = "";
            while (sOrigem.IndexOf(",") > 0)
            {
                s = sOrigem.Substring(0, sOrigem.IndexOf(","));

                if (!String.IsNullOrWhiteSpace(sDestino))
                    sDestino += ", ";

                sDestino += s + "=@" + s;

                sOrigem = sOrigem.Substring(s.Length + 1, sOrigem.Length - s.Length - 1).Trim();
            }
            s = sOrigem;
            if (!String.IsNullOrWhiteSpace(sDestino))
                sDestino += ", ";
            sDestino += s + "=@" + s;

            return sDestino;
        }
    }
}
