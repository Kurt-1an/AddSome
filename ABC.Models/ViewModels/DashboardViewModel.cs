using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }

        public int TotalUnprocessedOrders { get; set; }
        public int OrdersOutForDelivery { get; set; }
        public int CanceledOrders { get; set; }

        public int TotalProducts { get; set; }

        public int LowStockProductCount { get; set; }

        public int OutofStockProductCount { get; set; }

        public int TotalCategories { get; set; }
    }

}
