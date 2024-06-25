namespace ToDoListLibrary
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

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
            return $"UserId: {UserId}, Username: {Username}, Password: {Password}";
        }
    }
}