using ItServiceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Controllers
{
    public class PaymentController : Controller
    {
        //private readonly IyzicoPaymentService __paymentService; loose coupling yapmazsak bu şekil tanmlardık (dependency inection ) 
        //public PaymentController(IyzicoPaymentService paymentService)
        //{
        //    __paymentService = paymentService;
        //}
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
