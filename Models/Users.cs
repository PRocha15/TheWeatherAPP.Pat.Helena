namespace TheWeatherAPP.Pat.Helena.Models
{
    public class Users
    {
        public int Id;
        public string firstName;
        public string lastName;
        public char gender;
        public DateTime birthdate;
        public string email;
        public int phoneNumber;


        public int ID { get; set; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public char Gender { get => gender; set => gender = value; }
        public DateTime Birthdate { get => birthdate; set => birthdate = value; }
        public string Email { get => email; set => email = value; }
        public int PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
    }
}
