namespace FIAPCloudGames.Users.Api.Features.Users.Commands;

internal static class DocumentValidator
{
    public static bool IsValidCpf(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        int[] numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        for (int j = 9; j < 11; j++)
        {
            int sum = 0;

            for (int i = 0; i < j; i++)
                sum += numbers[i] * (j + 1 - i);

            int remainder = sum % 11;

            int checkDigit = remainder < 2 ? 0 : 11 - remainder;

            if (numbers[j] != checkDigit)
                return false;
        }

        return true;
    }

    public static bool IsValidCnpj(string cnpj)
    {
        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0]))
            return false;

        int[] multiplicator1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        
        int[] multiplicator2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        int[] digits = cnpj.Select(c => int.Parse(c.ToString())).ToArray();

        int sum = 0;
        
        for (int i = 0; i < 12; i++)
            sum += digits[i] * multiplicator1[i];

        int remainder = sum % 11;
        
        int digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (digits[12] != digit1)
            return false;

        sum = 0;
        
        for (int i = 0; i < 13; i++)
            sum += digits[i] * multiplicator2[i];

        remainder = sum % 11;
        
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        return digits[13] == digit2;
    }
}
