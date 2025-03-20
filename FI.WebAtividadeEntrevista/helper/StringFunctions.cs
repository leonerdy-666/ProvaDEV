using System.Text.RegularExpressions;
using System;
using System.Reflection;

namespace WebAtividadeEntrevista.Helper
{
    public class StringFunctions
    {
        public static string SoNumeroString(string Cpf)
        {
            return String.Join("", Regex.Split(Cpf, @"[^\d]"));
        }

    }
}