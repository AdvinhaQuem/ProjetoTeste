using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoTeste.Classes;
using ProjetoTeste.Models;
using RestSharp;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace ProjetoTeste.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult InfoPessoa(int idPessoa = 0)
        {
            People pessoa = new People();

            #region RECUPERAR INFORMAÇÕES DE PESSOA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/people/" + idPessoa + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    pessoa = JsonConvert.DeserializeObject<People>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de uma pessoa
                if (pessoa != null)
                {
                    #region Homeworld
                    if (!string.IsNullOrWhiteSpace(pessoa.homeworld))
                    {
                        var planetaSplit = pessoa.homeworld.Split('/');
                        string idHomeworld = planetaSplit[planetaSplit.Length - 2];
                        request = WebRequest.Create("https://swapi.dev/api/planets/" + idHomeworld + "/");
                        request.Method = "GET";
                        request.ContentType = "application/json; charset=utf-8";
                        request.Timeout = 600000;

                        ws = request.GetResponse();

                        retorno = string.Empty;

                        using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                        {
                            retorno = sReader.ReadToEnd();
                        }

                        if (!string.IsNullOrWhiteSpace(retorno))
                        {
                            Planets planet = JsonConvert.DeserializeObject<Planets>(retorno);
                            if (planet != null)
                                pessoa.mundoNatal = idHomeworld + ": " + planet.name;
                            else
                                pessoa.mundoNatal = "Não informado";
                        }

                        ws.Close();
                        ws.Dispose();
                    }
                    #endregion

                    #region Filmes
                    if (pessoa.films != null && pessoa.films.Count > 0)
                    {
                        pessoa.filmes = "";
                        foreach (var film in pessoa.films)
                        {
                            if (!string.IsNullOrWhiteSpace(film))
                            {
                                var filmSplit = film.Split('/');
                                string idFilm = filmSplit[filmSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/films/" + idFilm + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Films filme = JsonConvert.DeserializeObject<Films>(retorno);
                                    if (filme != null)
                                        pessoa.filmes += "<br>" + idFilm + ": " + filme.title + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        pessoa.filmes = "Não informado";
                    }
                    #endregion

                    #region Espécie
                    if (pessoa.species != null && pessoa.species.Count > 0)
                    {
                        foreach (var spec in pessoa.species)
                        {
                            if (!string.IsNullOrWhiteSpace(spec))
                            {
                                var specSplit = spec.Split('/');
                                string idSpec = specSplit[specSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/species/" + idSpec + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Species specie = JsonConvert.DeserializeObject<Species>(retorno);
                                    if (specie != null)
                                        pessoa.especies += "<br>" + idSpec + ": " + specie.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }

                        }
                    }
                    else
                        pessoa.especies = "Não informado";
                    #endregion

                    #region Veículos
                    if (pessoa.vehicles != null && pessoa.vehicles.Count > 0)
                    {
                        foreach (var vehicle in pessoa.vehicles)
                        {
                            if (!string.IsNullOrWhiteSpace(vehicle))
                            {
                                var vehicleSplit = vehicle.Split('/');
                                string idVehicle = vehicleSplit[vehicleSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/vehicles/" + idVehicle + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Vehicles vehicles = JsonConvert.DeserializeObject<Vehicles>(retorno);
                                    if (vehicles != null)
                                        pessoa.veiculos += "<br>" + idVehicle + ": " + vehicles.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }
                        }

                        if (!string.IsNullOrWhiteSpace(pessoa.veiculos))
                        {
                            pessoa.veiculos = pessoa.veiculos.TrimEnd();
                            pessoa.veiculos = pessoa.veiculos.Remove(pessoa.veiculos.Length - 1);
                        }
                        else
                        {
                            pessoa.veiculos = "Não informado";
                        }
                    }
                    else
                        pessoa.veiculos = "Não informado";
                    #endregion

                    #region Aeronaves
                    if (pessoa.starships != null && pessoa.starships.Count > 0)
                    {
                        foreach (var starship in pessoa.starships)
                        {
                            if (!string.IsNullOrWhiteSpace(starship))
                            {
                                var starshipSplit = starship.Split('/');
                                string idStarship = starshipSplit[starshipSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/starships/" + idStarship + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Starships starships = JsonConvert.DeserializeObject<Starships>(retorno);
                                    if (starships != null)
                                        pessoa.aeronaves += "<br>" + idStarship + ": " + starships.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(pessoa.aeronaves))
                        {
                            pessoa.aeronaves = pessoa.aeronaves.TrimEnd();
                            pessoa.aeronaves = pessoa.aeronaves.Remove(pessoa.aeronaves.Length - 1);
                        }
                        else
                        {
                            pessoa.aeronaves = "Não informado";
                        }
                    }
                    else
                        pessoa.aeronaves = "Não informado";
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { pessoa = "null" });
            }

            return Json(new { pessoa = pessoa});
        }

        public JsonResult InfoPlaneta(int idPlaneta = 0)
        {
            Planets planeta = new Planets();

            #region RECUPERAR INFORMAÇÕES DE PLANETA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/planets/" + idPlaneta + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    planeta = JsonConvert.DeserializeObject<Planets>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de uma planeta
                if (planeta != null)
                {
                    #region Filmes
                    if (planeta.films != null && planeta.films.Count > 0)
                    {
                        planeta.filmes = "";
                        foreach (var film in planeta.films)
                        {
                            if (!string.IsNullOrWhiteSpace(film))
                            {
                                var filmSplit = film.Split('/');
                                string idFilm = filmSplit[filmSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/films/" + idFilm + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Films filme = JsonConvert.DeserializeObject<Films>(retorno);
                                    if (filme != null)
                                        planeta.filmes += "<br>" + idFilm + ": " + filme.title + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        planeta.filmes = "Não informado";
                    }
                    #endregion

                    #region Residentes
                    if (planeta.residents != null && planeta.residents.Count > 0)
                    {
                        foreach (var res in planeta.residents)
                        {
                            if (!string.IsNullOrWhiteSpace(res))
                            {
                                var resSplit = res.Split('/');
                                string idRes = resSplit[resSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/people/" + idRes + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    People people = JsonConvert.DeserializeObject<People>(retorno);
                                    if (people != null)
                                        planeta.residentes += "<br>" + idRes + ": " + people.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }

                        }
                    }
                    else
                        planeta.residentes = "Não informado";
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { planeta = "null" });
            }

            return Json(new { planeta = planeta });
        }

        public JsonResult InfoFilme(int idFilme = 0)
        {
            Films filme = new Films();

            #region RECUPERAR INFORMAÇÕES DE PESSOA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/films/" + idFilme + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    filme = JsonConvert.DeserializeObject<Films>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de um filme
                if (filme != null)
                {
                    #region Personagens
                    if (filme.characters != null && filme.characters.Count > 0)
                    {
                        filme.personagens = "";
                        foreach (var character in filme.characters)
                        {
                            if (!string.IsNullOrWhiteSpace(character))
                            {
                                var characterSplit = character.Split('/');
                                string idCharacter = characterSplit[characterSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/people/" + idCharacter + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    People pessoa = JsonConvert.DeserializeObject<People>(retorno);
                                    if (filme != null)
                                        filme.personagens += "<br>" + idCharacter + ": " + pessoa.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        filme.personagens = "Não informado";
                    }
                    #endregion

                    #region Homeworld
                    if (filme.planets != null && filme.planets.Count > 0)
                    {
                        foreach (var planet in filme.planets)
                        {
                            if (!string.IsNullOrWhiteSpace(planet))
                            {
                                var planetSplit = planet.Split('/');
                                string idPlanet = planetSplit[planetSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/planets/" + idPlanet + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Planets planeta = JsonConvert.DeserializeObject<Planets>(retorno);
                                    if (planeta != null)
                                        filme.planetas += "<br>" + idPlanet + ": " + planeta.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }

                        }
                    }
                    else
                        filme.especies = "Não informado";
                    #endregion

                    #region Espécie
                    if (filme.species != null && filme.species.Count > 0)
                    {
                        foreach (var spec in filme.species)
                        {
                            if (!string.IsNullOrWhiteSpace(spec))
                            {
                                var specSplit = spec.Split('/');
                                string idSpec = specSplit[specSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/species/" + idSpec + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Species specie = JsonConvert.DeserializeObject<Species>(retorno);
                                    if (specie != null)
                                        filme.especies += "<br>" + idSpec + ": " + specie.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }

                        }
                    }
                    else
                        filme.especies = "Não informado";
                    #endregion

                    #region Veículos
                    if (filme.vehicles != null && filme.vehicles.Count > 0)
                    {
                        foreach (var vehicle in filme.vehicles)
                        {
                            if (!string.IsNullOrWhiteSpace(vehicle))
                            {
                                var vehicleSplit = vehicle.Split('/');
                                string idVehicle = vehicleSplit[vehicleSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/vehicles/" + idVehicle + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Vehicles vehicles = JsonConvert.DeserializeObject<Vehicles>(retorno);
                                    if (vehicles != null)
                                        filme.veiculos += "<br>" + idVehicle + ": " + vehicles.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();

                            }
                        }

                        if (!string.IsNullOrWhiteSpace(filme.veiculos))
                        {
                            filme.veiculos = filme.veiculos.TrimEnd();
                            filme.veiculos = filme.veiculos.Remove(filme.veiculos.Length - 1);
                        }
                        else
                        {
                            filme.veiculos = "Não informado";
                        }
                    }
                    else
                        filme.veiculos = "Não informado";
                    #endregion

                    #region Aeronaves
                    if (filme.starships != null && filme.starships.Count > 0)
                    {
                        foreach (var starship in filme.starships)
                        {
                            if (!string.IsNullOrWhiteSpace(starship))
                            {
                                var starshipSplit = starship.Split('/');
                                string idStarship = starshipSplit[starshipSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/starships/" + idStarship + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Starships starships = JsonConvert.DeserializeObject<Starships>(retorno);
                                    if (starships != null)
                                        filme.aeronaves += "<br>" + idStarship + ": " + starships.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(filme.aeronaves))
                        {
                            filme.aeronaves = filme.aeronaves.TrimEnd();
                            filme.aeronaves = filme.aeronaves.Remove(filme.aeronaves.Length - 1);
                        }
                        else
                        {
                            filme.aeronaves = "Não informado";
                        }
                    }
                    else
                        filme.aeronaves = "Não informado";
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { filme = "null" });
            }

            return Json(new { filme = filme });
        }

        public JsonResult InfoEspecie(int idEspecie = 0)
        {
            Species especie = new Species();

            #region RECUPERAR INFORMAÇÕES DE PESSOA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/species/" + idEspecie + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    especie = JsonConvert.DeserializeObject<Species>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de um especie
                if (especie != null)
                {
                    #region Homeworld
                    if (!string.IsNullOrWhiteSpace(especie.homeworld))
                    {
                        var planetaSplit = especie.homeworld.Split('/');
                        string idHomeworld = planetaSplit[planetaSplit.Length - 2];
                        request = WebRequest.Create("https://swapi.dev/api/planets/" + idHomeworld + "/");
                        request.Method = "GET";
                        request.ContentType = "application/json; charset=utf-8";
                        request.Timeout = 600000;

                        ws = request.GetResponse();

                        retorno = string.Empty;

                        using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                        {
                            retorno = sReader.ReadToEnd();
                        }

                        if (!string.IsNullOrWhiteSpace(retorno))
                        {
                            Planets planet = JsonConvert.DeserializeObject<Planets>(retorno);
                            if (planet != null)
                                especie.mundoNatal = idHomeworld + ": " + planet.name;
                            else
                                especie.mundoNatal = "Não informado";
                        }

                        ws.Close();
                        ws.Dispose();
                    }
                    #endregion

                    #region Personagens
                    if (especie.people != null && especie.people.Count > 0)
                    {
                        especie.pessoas = "";
                        foreach (var character in especie.people)
                        {
                            if (!string.IsNullOrWhiteSpace(character))
                            {
                                var characterSplit = character.Split('/');
                                string idCharacter = characterSplit[characterSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/people/" + idCharacter + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    People pessoa = JsonConvert.DeserializeObject<People>(retorno);
                                    if (especie != null)
                                        especie.pessoas += "<br>" + idCharacter + ": " + pessoa.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        especie.pessoas = "Não informado";
                    }
                    #endregion

                    #region Filmes
                    if (especie.films != null && especie.films.Count > 0)
                    {
                        especie.filmes = "";
                        foreach (var film in especie.films)
                        {
                            if (!string.IsNullOrWhiteSpace(film))
                            {
                                var filmSplit = film.Split('/');
                                string idFilm = filmSplit[filmSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/films/" + idFilm + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Films filme = JsonConvert.DeserializeObject<Films>(retorno);
                                    if (filme != null)
                                        especie.filmes += "<br>" + idFilm + ": " + filme.title + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        especie.filmes = "Não informado";
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { especie = "null" });
            }

            return Json(new { especie = especie });
        }

        public JsonResult InfoVehicle(int idVehicle = 0)
        {
            Vehicles vehicle = new Vehicles();

            #region RECUPERAR INFORMAÇÕES DE PESSOA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/vehicles/" + idVehicle + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    vehicle = JsonConvert.DeserializeObject<Vehicles>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de um vehicle
                if (vehicle != null)
                {
                    #region Pilotos
                    if (vehicle.pilots != null && vehicle.pilots.Count > 0)
                    {
                        vehicle.pilotos = "";
                        foreach (var character in vehicle.pilots)
                        {
                            if (!string.IsNullOrWhiteSpace(character))
                            {
                                var characterSplit = character.Split('/');
                                string idCharacter = characterSplit[characterSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/people/" + idCharacter + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    People pessoa = JsonConvert.DeserializeObject<People>(retorno);
                                    if (vehicle != null)
                                        vehicle.pilotos += "<br>" + idCharacter + ": " + pessoa.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        vehicle.pilotos = "Não informado";
                    }
                    #endregion

                    #region Filmes
                    if (vehicle.films != null && vehicle.films.Count > 0)
                    {
                        vehicle.filmes = "";
                        foreach (var film in vehicle.films)
                        {
                            if (!string.IsNullOrWhiteSpace(film))
                            {
                                var filmSplit = film.Split('/');
                                string idFilm = filmSplit[filmSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/films/" + idFilm + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Films filme = JsonConvert.DeserializeObject<Films>(retorno);
                                    if (filme != null)
                                        vehicle.filmes += "<br>" + idFilm + ": " + filme.title + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        vehicle.filmes = "Não informado";
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { vehicle = "null" });
            }

            return Json(new { vehicle = vehicle });
        }

        public JsonResult InfoAeronave(int idAeronave = 0)
        {
            Starships starship = new Starships();

            #region RECUPERAR INFORMAÇÕES DE PESSOA
            WebRequest request = WebRequest.Create("https://swapi.dev/api/starships/" + idAeronave + "/");
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 600000;
            try
            {
                WebResponse ws = request.GetResponse();
                string retorno = string.Empty;

                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                {
                    retorno = sReader.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(retorno))
                {
                    starship = JsonConvert.DeserializeObject<Starships>(retorno);
                }

                ws.Close();
                ws.Dispose();
                //client.Dispose();
                #endregion

                #region Recuperar as informações adicionais de um starship
                if (starship != null)
                {
                    #region Pilotos
                    if (starship.pilots != null && starship.pilots.Count > 0)
                    {
                        starship.pilotos = "";
                        foreach (var character in starship.pilots)
                        {
                            if (!string.IsNullOrWhiteSpace(character))
                            {
                                var characterSplit = character.Split('/');
                                string idCharacter = characterSplit[characterSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/people/" + idCharacter + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    People pessoa = JsonConvert.DeserializeObject<People>(retorno);
                                    if (starship != null)
                                        starship.pilotos += "<br>" + idCharacter + ": " + pessoa.name + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        starship.pilotos = "Não informado";
                    }
                    #endregion

                    #region Filmes
                    if (starship.films != null && starship.films.Count > 0)
                    {
                        starship.filmes = "";
                        foreach (var film in starship.films)
                        {
                            if (!string.IsNullOrWhiteSpace(film))
                            {
                                var filmSplit = film.Split('/');
                                string idFilm = filmSplit[filmSplit.Length - 2];
                                request = WebRequest.Create("https://swapi.dev/api/films/" + idFilm + "/");
                                request.Method = "GET";
                                request.ContentType = "application/json; charset=utf-8";
                                request.Timeout = 600000;

                                ws = request.GetResponse();

                                retorno = string.Empty;

                                using (StreamReader sReader = new StreamReader(ws.GetResponseStream()))
                                {
                                    retorno = sReader.ReadToEnd();
                                }

                                if (!string.IsNullOrWhiteSpace(retorno))
                                {
                                    Films filme = JsonConvert.DeserializeObject<Films>(retorno);
                                    if (filme != null)
                                        starship.filmes += "<br>" + idFilm + ": " + filme.title + "</br>";
                                }

                                ws.Close();
                                ws.Dispose();
                            }
                        }
                    }
                    else
                    {
                        starship.filmes = "Não informado";
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { aeronave = "null" });
            }

            return Json(new { aeronave = starship });
        }
    }
}