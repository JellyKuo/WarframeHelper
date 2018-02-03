namespace WarframeHelper.Game
{
     public class Warframe
    {
        public static Warframe[] Parse(string Path)
        {
            string Json = System.IO.File.ReadAllText(Path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Warframe[]>(Json);
        }

        public string name { get; set; }
        public string url { get; set; }
        public string mr { get; set; }
        public string health { get; set; }
        public string shield { get; set; }
        public string armor { get; set; }
        public string power { get; set; }
        public string speed { get; set; }
        public string conclave { get; set; }
        public string[] polarities { get; set; }
        public string aura { get; set; }
        public string description { get; set; }
        public string info { get; set; }
        public string thumbnail { get; set; }
        public string location { get; set; }
        public int color { get; set; }
        public string prime_mr { get; set; }
        public string prime_health { get; set; }
        public string prime_shield { get; set; }
        public string prime_armor { get; set; }
        public string prime_speed { get; set; }
        public string prime_power { get; set; }
        public string prime_conclave { get; set; }
        public string[] prime_polarities { get; set; }
        public string prime_aura { get; set; }
    }
}