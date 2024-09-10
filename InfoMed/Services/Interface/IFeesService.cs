using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IFeesService
    {
        public Task<List<ConferenceFeeDto>> GetConferenceFeesList  (int id, int idVersion);
        public Task<ConferenceFeeDto> AddFees(ConferenceFeeDto feesMasterDto);
        public Task<ConferenceFeeDto> GetFeesById(int idFeesMaster);
        public Task<ConferenceFeeDto> UpdateFees(ConferenceFeeDto feesMasterDto);
    }
}
