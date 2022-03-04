using ItServiceApp.Core.Payment;

namespace ItServiceApp.Business.Services.Payment
{
    public interface IPaymentService
    {
        public InstalmentModel CheckInstallments(string binNumber, decimal price);
        public PaymentResponseModel Pay(PaymentModel model);
    }
}
