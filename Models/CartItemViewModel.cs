using DotK_TechShop.Models.Db;

namespace DotK_TechShop.Models;

public class CartItemViewModel
{
    private int _quantity;
    private double _total;
    public readonly Product Product;
    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            _total = value * Product.Price;
        }
    }
    public double Total
    {
        get => _total;
    }

    public CartItemViewModel(Product product)
    {
        Product = product;
        Quantity = 1;
    }
}
