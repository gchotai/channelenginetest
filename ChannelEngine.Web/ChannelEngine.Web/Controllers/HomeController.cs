using ChannelEngine.Core.Services.Orders;
using ChannelEngine.Core.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace ChannelEngine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public HomeController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Action to get top 5 order by the total quantity sold in descending order
                var topProducts = await _orderService.GetTop5ProductsAsync();

                // Action to update the stock of a product
                if (topProducts.Count > 0)
                {
                    int newStock = 25;
                    bool isSuccess = await _productService.UpdateStockForProductAsync(topProducts[0].MerchantProductNo, newStock);
                    if (!isSuccess)
                    {
                        TempData["StockUpdateMessage"] = "Stock update failed.";
                        return RedirectToAction("Index");
                    }
                    else
                        TempData["StockUpdateMessage"] = $"Stock has been updated for '{topProducts[0].Name}' to {newStock}";
                }
                return View(topProducts);
            }
            catch (Exception ex)
            {
                TempData["StockUpdateMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Index");
            }
        }

    }
}
