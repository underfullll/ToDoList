namespace ToDoListLibrary
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Image {  get; set; }

        public User()
        {
            // Пустой конструктор для SQLite
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Username: {Username}, Password: {Password}";
        }
    }
}