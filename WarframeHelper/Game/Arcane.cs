namespace WarframeHelper.Game
{
    public class Arcane
    {
        public static Arcane[] Parse(string Path)
        {
            string Json = System.IO.File.ReadAllText(Path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Arcane[]>(Json);
        }

        public string name { get; set; }
        public string effect { get; set; }
        public string rarity { get; set; }
        public string location { get; set; }
        public string thumbnail { get; set; }
        public string info { get; set; }
    }
}