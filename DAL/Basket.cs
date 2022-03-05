namespace test_api;

public class Basket
{
    public long Id { get; set; }

    public ICollection<Bullion> Bullions { get; set; }

}
