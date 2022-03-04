using AutoMapper;
using ItServiceApp.Core.Payment;
using Iyzipay.Model;

namespace ItServiceApp.Business.MapperProfiles
{
    public class PaymentProfile : Profile 
    {
        public PaymentProfile()
        {
            CreateMap<InstallmentPriceModel, InstallmentPrice>().ReverseMap();
            //CreateMap<InstallmentPrice, InstallmentPriceModel>(); ReverseMap eklemeseydik bu şekil tanımlamamız lazımdı
            CreateMap<InstalmentModel, InstallmentDetail>().ReverseMap();//soldakiler bizimkiler sağdakiler iyzinin
            CreateMap<CardModel, PaymentCard>().ReverseMap();
            CreateMap<BasketModel, BasketItem>().ReverseMap();
            CreateMap<AddressModel, Address>().ReverseMap();
            CreateMap<CustomerModel, Buyer>().ReverseMap();
            CreateMap<PaymentResponseModel, Payment>().ReverseMap();
        }
    }
}
