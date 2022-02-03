using AutoMapper;
using ItServiceApp.Data;
using ItServiceApp.Models.Entities;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Controllers
{
    //asp net identity
    public class HomeController : Controller
    {
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;
        public HomeController(MyContext myContext, IMapper mapper)
        {
            _myContext = myContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = _myContext.SubscriptionTypes.OrderBy(x => x.Price).ToList()
                .Select(x => _mapper.Map<SubscriptionTypeViewModel>(x)).ToList();//önemli burası incele Mapper tolistten sonra yazıldı!!! Aşağıdaki yorum satırıyla aynı işi
            //yapıyor. Bir satırda!!!!!

            //var data = new List<SubscriptionTypeViewModel>();
            //var query = _myContext.SubscriptionTypes.OrderBy(x=> x.Price).ToList();
            //foreach (SubscriptionType subscriptionType in query)
            //{
            //    data.Add(new SubscriptionTypeViewModel()
            //    {
            //        Name = subscriptionType.Name,
            //        Description = subscriptionType.Description,
            //        Id = subscriptionType.Id,
            //        Month = subscriptionType.Month,
            //        Price = subscriptionType.Price
            //    });
            //}

            return View(model);
        }
    }
}
