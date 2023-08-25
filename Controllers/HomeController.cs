using Frontend_BCPVentas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

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

        public IActionResult Index()
        {
            RestClient Client = new RestClient("https://localhost:7094/api/Venta");
            string Respuesta;

            RestRequest request = new RestRequest();
            var response = Client.Get(request);

            Respuesta = response.Content;

            var oVentasInformation = JsonConvert.DeserializeObject<List<VentasInformation>>(Respuesta);

            ListaVentas = oVentasInformation;

            return View(ListaVentas);
        }

        public IActionResult Nuevo()
        {
            RestClient Client = new RestClient("https://localhost:7094/api/Asesor");
            string Respuesta;

            RestRequest request = new RestRequest();
            var response = Client.Get(request);

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

            RestClient Client2 = new RestClient("https://localhost:7094/api/Clients");
            string Respuesta2;

            RestRequest request2 = new RestRequest();
            var response2 = Client2.Get(request2);

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

            RestClient Client3 = new RestClient("https://localhost:7094/api/Producto");
            string Respuesta3;

            RestRequest request3 = new RestRequest();
            var response3 = Client3.Get(request3);

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
            Ventas ventas = new Ventas();

            ventas.Periodo = Convert.ToDateTime(periodo);
            ventas.IdAsesor = Convert.ToInt32(IdAsesor);
            ventas.IdClient = Convert.ToInt32(IdCliente);
            ventas.IdProduct = Convert.ToInt32(IdProducto);
            ventas.Fecha = DateTime.Now;
            ventas.Monto = Convert.ToDouble(monto);

            RestClient Ventas = new RestClient();
                string json = JsonConvert.SerializeObject(ventas);
                respuesta = Ventas.PostJson("https://localhost:7094/api/Venta/", json);

            //Combos

            RestClient Client = new RestClient("https://localhost:7094/api/Asesor");
            string Respuesta;

            RestRequest request = new RestRequest();
            var response = Client.Get(request);

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

            RestClient Client2 = new RestClient("https://localhost:7094/api/Clients");
            string Respuesta2;

            RestRequest request2 = new RestRequest();
            var response2 = Client2.Get(request2);

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

            RestClient Client3 = new RestClient("https://localhost:7094/api/Producto");
            string Respuesta3;

            RestRequest request3 = new RestRequest();
            var response3 = Client3.Get(request3);

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
            RestClient Client = new RestClient("https://localhost:7094/api/Asesor");
            string Respuesta;

            RestRequest request = new RestRequest();
            var response = Client.Get(request);

            Respuesta = response.Content;

            var oAsesor = JsonConvert.DeserializeObject<List<Asesor>>(Respuesta);

            Asesor = oAsesor;

            List<SelectListItem> items = Asesor.ConvertAll(d=>{
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

            RestClient Client = new RestClient("https://localhost:7094/api/Venta/" + Id_Asesor);
            string Respuesta;

            RestRequest request = new RestRequest();
            var response = Client.Get(request);

            Respuesta = response.Content;

            var oVentasInformation = JsonConvert.DeserializeObject<List<VentasInformation>>(Respuesta);

            ListaVentas = oVentasInformation;

            RestClient Client2 = new RestClient("https://localhost:7094/api/Asesor");
            string Respuesta2;

            RestRequest request2 = new RestRequest();
            var response2 = Client2.Get(request);

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