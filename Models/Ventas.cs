namespace Frontend_BCPVentas.Models
{
    public class Ventas
    {
        public int IdVenta { get; set; }
        public DateTime Periodo { get; set; }
        public int IdAsesor { get; set; }
        public int IdClient { get; set; }
        public int IdProduct { get; set; }
        public int Puntos { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
    }
}
