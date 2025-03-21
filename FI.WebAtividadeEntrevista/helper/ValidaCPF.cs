using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

public class ValidaCPF : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        string cpf = value.ToString().Replace(".", "").Replace("-", "").Trim();

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Verifica se todos os dígitos são iguais
        if (new string(cpf[0], 11) == cpf)
            return false;

        // Cálculo de validação do CPF
        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = (soma % 11);
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;

        tempCpf += primeiroDigito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = (soma % 11);
        int segundoDigito = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(primeiroDigito.ToString() + segundoDigito.ToString());
    }
}
