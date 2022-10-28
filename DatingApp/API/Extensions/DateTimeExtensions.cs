namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            // xsi este año ahún no tiene su cumpleaños
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
