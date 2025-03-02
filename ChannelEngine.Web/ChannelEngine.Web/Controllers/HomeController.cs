using ChannelEngine.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChannelEngine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChannelEngineService _channelEngineService;

        public HomeController(ChannelEngineService channelEngineService)
        {
            _channelEngineService = channelEngineService;
        }

        public async Task<IActionResult> Index()
        {
            var topProducts = await _channelEngineService.GetTop5ProductsAsync();

            // Action to update the stock of a product
            if (topProducts.Count > 0)
            {
                int newStock = 25;
                bool isSuccess = await _channelEngineService.UpdateStockForProductAsync(topProducts[0].MerchantProductNo, newStock);
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

    }
}
