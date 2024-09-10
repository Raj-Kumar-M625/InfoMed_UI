using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        public PaymentService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<PaymentDetailsDto> AddPaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddPaymentDetails;
                var jsonString = JsonConvert.SerializeObject(paymentDetailsDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<PaymentDetailsDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddPaymentDetails() in PaymentService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<PaymentDetailsDto> GetPaymentDetails(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetPaymentDetails + $"?id={id}&idVersion={idVersion}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<PaymentDetailsDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetPaymentDetails() in PaymentService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }

        public async Task<PaymentDetailsDto> UpdatePaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdatePaymentDetails;
                var jsonString = JsonConvert.SerializeObject(paymentDetailsDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<PaymentDetailsDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdatePaymentDetails() in PaymentService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}
