namespace Martiello.Application.Extensions
{
    public static class DocumentExtensions
    {
        public static bool IsValidCpf(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            var invalidCpfs = new string[]
            {
            "00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555",
            "66666666666", "77777777777", "88888888888", "99999999999"
            };
            if (invalidCpfs.Contains(cpf))
            {
                return false;
            }

            var digits = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            var sum1 = 0;
            for (int i = 0; i < 9; i++)
            {
                sum1 += digits[i] * (10 - i);
            }

            var remainder1 = sum1 % 11;
            var checkDigit1 = (remainder1 < 2) ? 0 : 11 - remainder1;

            if (checkDigit1 != digits[9])
            {
                return false;
            }

            var sum2 = 0;
            for (int i = 0; i < 10; i++)
            {
                sum2 += digits[i] * (11 - i);
            }

            var remainder2 = sum2 % 11;
            var checkDigit2 = (remainder2 < 2) ? 0 : 11 - remainder2;

            return checkDigit2 == digits[10];
        }

        public static bool IsValidCpf(this long cpfLong)
        {
            var cpfString = cpfLong.ToString().PadLeft(11, '0'); 
            return cpfString.IsValidCpf();
        }
    }
}
