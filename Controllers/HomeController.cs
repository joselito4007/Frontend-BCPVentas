using Frontend_BCPVentas.Authorization;
using Frontend_BCPVentas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Frontend_BCPVentas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public List<VentasInformation> ListaVentas = new List<VentasInformation>();
        public List<Asesor> Asesor = new List<Asesor>();
        public List<Client> Clientes = new List<Client>();
        public List<Producto> Producto = new List<Producto>();
        public Login login = new Login();

        public IActionResult Index()
        {
            string Respuesta;

            string UrlApi = "https://localhost:7094/api/";

            var options = new RestClientOptions()
            {
                Authenticator = new ApiAuthorization(UrlApi, new Login()
                {
                    Email = "carlitos.4007@gmail.com",
                    Pwd = "123456"

                })
            };

            var Client = new RestClient(options);

            var request = new RestRequest()
            {
                Resource = UrlApi + "Venta",
                Method = Method.Get
            };

            //RestRequest request = new RestRequest("Venta");
            var response = Client.Execute(request);

            Respuesta = response.Content;

            var oVentasInformation = JsonConvert.DeserializeObject<List<VentasInformation>>(Respuesta);

            ListaVentas = oVentasInformation;

            return View(ListaVentas);
        }

        public IActionResult Nuevo()
        {
            //Combos

            string Respuesta;
            string Respuesta2;
            string Respuesta3;

            string UrlApi = "https://localhost:7094/api/";

            var options = new RestClientOptions()
            {
                Authenticator = new ApiAuthorization(UrlApi, new Login()
                {
                    Email = "carlitos.4007@gmail.com",
                    Pwd = "123456"

                })
            };

            var Client = new RestClient(options);

            var request = new RestRequest()
            {
                Resource = UrlApi + "Asesor",
                Method = Method.Get
            };

            var response = Client.Execute(request);

            Respuesta = response.Content;

            var oAsesor = JsonConvert.DeserializeObject<List<Asesor>>(Respuesta);

            Asesor = oAsesor;

            List<SelectListItem> items = Asesor.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdAsesor.ToString(),
                    Selected = false
                };
            });

            var Client2 = new RestClient(options);

            var request2 = new RestRequest()
            {
                Resource = UrlApi + "Clients",
                Method = Method.Get
            };

            var response2 = Client2.Execute(request2);

            Respuesta2 = response2.Content;

            var oClient = JsonConvert.DeserializeObject<List<Client>>(Respuesta2);

            Clientes = oClient;

            List<SelectListItem> items2 = Clientes.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.name.ToString()+" "+d.lastname,
                    Value = d.IdClient.ToString(),
                    Selected = false
                };
            });

            var Client3 = new RestClient(options);

            var request3 = new RestRequest()
            {
                Resource = UrlApi + "Producto",
                Method = Method.Get
            };

            var response3 = Client3.Execute(request3);

            Respuesta3 = response3.Content;

            var oProducto = JsonConvert.DeserializeObject<List<Producto>>(Respuesta3);

            Producto = oProducto;

            List<SelectListItem> items3 = Producto.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdProducto.ToString(),
                    Selected = false
                };
            });

            ViewBag.items = items;
            ViewBag.items2 = items2;
            ViewBag.items3 = items3;

            return View();
        }

        [HttpPost]
        public IActionResult Nuevo(string IdAsesor,string IdCliente, string IdProducto, string periodo, string monto)
        {
            dynamic respuesta;
            string Respuesta;
            string Respuesta2;
            string Respuesta3;

            Ventas ventas = new Ventas();

            string UrlApi = "https://localhost:7094/api/";

            var options = new RestClientOptions()
            {
                Authenticator = new ApiAuthorization(UrlApi, new Login()
                {
                    Email = "carlitos.4007@gmail.com",
                    Pwd = "123456"

                })
            };

            ventas.Periodo = Convert.ToDateTime(periodo);
            ventas.IdAsesor = Convert.ToInt32(IdAsesor);
            ventas.IdClient = Convert.ToInt32(IdCliente);
            ventas.IdProduct = Convert.ToInt32(IdProducto);
            ventas.Fecha = DateTime.Now;
            ventas.Monto = Convert.ToDouble(monto);

            var Ventas = new RestClient(options);

            string json = JsonConvert.SerializeObject(ventas);
            respuesta = Ventas.PostJson(UrlApi + "Venta", json);

            //Combos

            var Client = new RestClient(options);

            var request = new RestRequest()
            {
                Resource = UrlApi + "Asesor",
                Method = Method.Get
            };

            var response = Client.Execute(request);

            Respuesta = response.Content;

            var oAsesor = JsonConvert.DeserializeObject<List<Asesor>>(Respuesta);

            Asesor = oAsesor;

            List<SelectListItem> items = Asesor.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdAsesor.ToString(),
                    Selected = false
                };
            });

            var Client2 = new RestClient(options);

            var request2 = new RestRequest()
            {
                Resource = UrlApi + "Producto",
                Method = Method.Get
            };

            var response2 = Client2.Execute(request2);

            Respuesta2 = response2.Content;

            var oClient = JsonConvert.DeserializeObject<List<Client>>(Respuesta2);

            Clientes = oClient;

            List<SelectListItem> items2 = Clientes.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.name.ToString() + " " + d.lastname,
                    Value = d.IdClient.ToString(),
                    Selected = false
                };
            });

            var Client3 = new RestClient(options);

            var request3 = new RestRequest()
            {
                Resource = UrlApi + "Clients",
                Method = Method.Get
            };

            var response3 = Client3.Execute(request3);

            Respuesta3 = response3.Content;

            var oProducto = JsonConvert.DeserializeObject<List<Producto>>(Respuesta3);

            Producto = oProducto;

            List<SelectListItem> items3 = Producto.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdProducto.ToString(),
                    Selected = false
                };
            });

            ViewBag.items = items;
            ViewBag.items2 = items2;
            ViewBag.items3 = items3;

            return View();
        }

            public IActionResult BuscarVentas()
        {

            string UrlApi = "https://localhost:7094/api/";
            string Respuesta;

            var options = new RestClientOptions()
            {
                Authenticator = new ApiAuthorization(UrlApi, new Login()
                {
                    Email = "carlitos.4007@gmail.com",
                    Pwd = "123456"

                })
            };

            var Client = new RestClient(options);

            var request = new RestRequest()
            {
                Resource = UrlApi + "Asesor",
                Method = Method.Get
            };

            var response = Client.Execute(request);

            Respuesta = response.Content;

            var oAsesor = JsonConvert.DeserializeObject<List<Asesor>>(Respuesta);

            Asesor = oAsesor;

            List<SelectListItem> items = Asesor.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdAsesor.ToString(),
                    Selected = false
                };
            });

            ViewBag.items = items;

            return View();
        }

        [HttpPost]
        public IActionResult BuscarVentas(string IdAsesor)
        {
            int sumaPuntos;
            int Id_Asesor = Int32.Parse(IdAsesor);

            string UrlApi = "https://localhost:7094/api/";
            string Respuesta;
            string Respuesta2;

            var options = new RestClientOptions()
            {
                Authenticator = new ApiAuthorization(UrlApi, new Login()
                {
                    Email = "carlitos.4007@gmail.com",
                    Pwd = "123456"

                })
            };

            var Client = new RestClient(options);

            var request = new RestRequest()
            {
                Resource = UrlApi + "Venta/" + Id_Asesor,
                Method = Method.Get
            };

            //RestRequest request = new RestRequest("Venta");
            var response = Client.Execute(request);

            Respuesta = response.Content;

            var oVentasInformation = JsonConvert.DeserializeObject<List<VentasInformation>>(Respuesta);

            ListaVentas = oVentasInformation;

            //Combo
            var Client2 = new RestClient(options);

            var request2 = new RestRequest()
            {
                Resource = UrlApi + "Asesor",
                Method = Method.Get
            };

            var response2 = Client2.Execute(request2);

            Respuesta2 = response2.Content;

            var oAsesor = JsonConvert.DeserializeObject<List<Asesor>>(Respuesta2);

            Asesor = oAsesor;

            List<SelectListItem> items = Asesor.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name.ToString(),
                    Value = d.IdAsesor.ToString(),
                    Selected = false
                };
            });

            ViewBag.items = items;

            return View(ListaVentas);
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
    }
}