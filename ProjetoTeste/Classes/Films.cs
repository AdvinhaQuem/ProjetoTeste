namespace ProjetoTeste.Classes
{
    public class Films
    {
        public string title { get; set; }
        public int episode_id { get; set; }
        public string opening_crawl { get; set; }
        public string director { get; set; }
        public string producer { get; set; }
        public string release_date { get; set; }
        public List<string> characters { get; set; }
        public List<string> planets { get; set; }
        public List<string> starships { get; set; }
        public List<string> vehicles { get; set; }
        public List<string> species { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
        public string url { get; set; }

        //
        public string personagens { get; set; }
        public string planetas { get; set; }
        public string aeronaves { get; set; }
        public string veiculos { get; set; }
        public string especies { get; set; }
    }
}
