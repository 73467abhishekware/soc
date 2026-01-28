using Microsoft.AspNetCore.Mvc;
using SOC.Models;

namespace SOC.Repository
{
    public interface ISOC_Repository
    {
        Task<tbl_login> login(string username, string password);
        //Task<int> InsertOpd(OpdModel model);
        Task<int> InsertDisease(InsertDisease model);
        Task<int> UpdateDisease(UpdateDisease model);
        Task<IEnumerable<UpdateDisease>> GetDiseaseList();
        Task<int> InsertBodyPart(InsertBodyPart model);
        Task<int> UpdateBodyPart(UpdateBodyPart model);
        Task<BodyPartWithDoctorsResponse> GetBodyPartList();
        Task<int> InsertMapping(InsertMapping model);
        Task<IEnumerable<MappingList>> GetMappingList();
        Task<int> DeleteMapping(int id);
        Task<IEnumerable<MappingsByBodyPart>> GetMappingsByBodyPart(int bpid);
        Task<int> InsertOpd(OpdModel model);
        Task<int> UpdateOpd(OpdModel model);
        Task<IEnumerable<PatientList>> GetPatientList(DateTime? filterDate = null);
        Task<OpdModel?> GetPatientById(int pid);

        Task<OpdModel> GetPatientByOpdNo(int opd_no);
        Task<int> SaveFees(FeesModel model);
        Task<int> UpdateFees(UpdateFeesModel model);

        Task<IEnumerable<FeesList>> GetFeesList(DateTime? date = null);
        Task<int> InsertItem(InsertItem model);
        Task<int> UpdateItem(UpdateItem model);
        Task<IEnumerable<ItemsList>> GetItemsList();
        Task<int> DeletePatientBodyPart(int pid, int bpid);
        Task<IEnumerable<MappingsByBodyPartAndType>> GetMappingsByBodyPartAndType(int bpid, string disease_type);

        Task<int> InsertBill(InsertBillModel model);
        Task<int> UpdateBill(UpdateBillModel model);
        Task<IEnumerable<BillListByDateDto>> GetBillItemsByDateFlag(DateTime date, string flag);
        Task<IEnumerable<BillItemDetailDto>> GetBillItemsDetailsByBillNo(int bill_no, string flag);
        Task<IEnumerable<DoctorListModel>> GetDoctorList();
        Task<IEnumerable<ItemRateDto>> GetItemRateList();

        Task<IEnumerable<PatientList>> SelectAllPatientList();
    }
}
