namespace ProviderAPI.Model
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public User(int id, string name, string type, string version)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.version = version;
        }
    }
}
