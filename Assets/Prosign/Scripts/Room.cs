namespace EliCDavis.Prosign
{
    public class Room
    {
        private string id;

        private string name;

        public Room(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format("ID({0}) Name({1})", id, name);
        }

        public string GetID()
        {
            return id;
        }


        public string GetName()
        {
            return name;
        }

    }

}