namespace WebApi.Schemas;

public class UserDetails
{
    public int id { get; set; }
    public string name { get; set; } = default!;
    public string codigo_cliente { get; set; } = default!;
    public Account? accounts { get; set; }
}
