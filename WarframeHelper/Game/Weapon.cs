namespace WarframeHelper.Game
{
    public class Weapon
    {
        public static Weapon[] Parse(string Path)
        {
            string Json = System.IO.File.ReadAllText(Path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Weapon[]>(Json);
        }

        public string name { get; set; }
        public string url { get; set; }
        public int? mr { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string impact { get; set; }
        public string puncture { get; set; }
        public string slash { get; set; }
        public string crit_chance { get; set; }
        public string crit_mult { get; set; }
        public string status_chance { get; set; }
        public string polarities { get; set; }
        public string thumbnail { get; set; }
        public string location { get; set; }

        public double speed { get; set; }
        public string damage { get; set; }
        public string slide { get; set; }
        public string jump { get; set; }
        public string wall { get; set; }
        public string channeling { get; set; }

        public string flight { get; set; }
        public string trigger { get; set; }
        public string projectile { get; set; }
        public string rate { get; set; }
        public string noise { get; set; }
        public string accuracy { get; set; }
        public string magazine { get; set; }
        public string ammo { get; set; }
        public string reload { get; set; }
        public string riven_disposition { get; set; }
    }
}