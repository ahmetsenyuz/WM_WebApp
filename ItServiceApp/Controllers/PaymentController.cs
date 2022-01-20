using ItServiceApp.Extensions;
using ItServiceApp.Models.Payment;
using ItServiceApp.Services;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Controllers
{
    [Authorize]
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
            //var result = _paymentService.CheckInstallments("4603450000000000", 1000);
            //var binNumbers = new string[]
            //{
            //    "5890040000000016","5400360000000003","4130111111111118","4111111111111129"
            //};
            //foreach (var bin in binNumbers)
            //{
            //    var result1 = _paymentService.CheckInstallments(bin, 1000);
            //}
            return View();
        }
        [HttpPost]
        public IActionResult CheckInstallment(string binNumber,decimal price)
        {
            var result = _paymentService.CheckInstallments(binNumber, price);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Index(PaymentViewModel model)
        {
            var paymentModel = new PaymentModel()
            {
                Installment = model.Installment,
                Address = new AddressModel(),
                BasketList = new List<BasketModel>(),
                Customer = new CustomerModel(),
                CardModel = model.CardModel,
                Price = 1000,
                UserId = HttpContext.GetUserId(),
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
            };
            var installmentInfo = _paymentService.CheckInstallments(paymentModel.CardModel.CardNumber, paymentModel.Price);
            var installmentNumber = installmentInfo.InstallmentPrices.FirstOrDefault(x => x.InstallmentNumber == model.Installment);

            paymentModel.PaidPrice = decimal.Parse(installmentNumber != null ? installmentNumber.TotalPrice.Replace('.',
                ',') : installmentInfo.InstallmentPrices[0].TotalPrice.Replace('.', ','));

            var result = _paymentService.Pay(paymentModel);
            return View();
        }
    }
}
