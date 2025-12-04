namespace MvAlimentosApp.Models
{
    public class Compra
    {
        public int ?Id { get; set; }
        public string ?Usuario { get; set; } = string.Empty;

        public decimal Total { get; set; }
        public DateTime  Fecha { get; set; }
    }
}
