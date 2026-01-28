using System.Data;
using System.Text;
using Dapper;
using SOC.Models;

namespace SOC.Repository
{

    public class SOC_Repository : ISOC_Repository
    {
        private readonly DapperContext context;
        public SOC_Repository(DapperContext context)
        {
            this.context = context;



        }
        public async Task<tbl_login> login(string username, string password)
        {
            try
            {
                var query = "sp_login";


                var parameters = new DynamicParameters();
                parameters.Add("@Action", "SelectLogin");
                parameters.Add("@username", username);
                parameters.Add("@password", password);



                using (var connection = context.CreateConnection())
                {
                    var result = await connection.QuerySingleOrDefaultAsync<tbl_login>(query, parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


        public async Task<int> InsertDisease(InsertDisease model)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertDisease");
            parameters.Add("@disease", model.disease);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@Result");
            }
        }


        public async Task<int> UpdateDisease(UpdateDisease model)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateDisease");
            parameters.Add("@did", model.did);
            parameters.Add("@disease", model.disease);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@Result");
            }
        }

        public async Task<IEnumerable<UpdateDisease>> GetDiseaseList()
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectDiseaseList");

            using (var connection = context.CreateConnection())
            {
                var result = await connection.QueryAsync<UpdateDisease>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task<int> InsertBodyPart(InsertBodyPart model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertBodyPart");
            parameters.Add("@bptype", model.bptype);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }

        // 🔹 UPDATE
        public async Task<int> UpdateBodyPart(UpdateBodyPart model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateBodyPart");
            parameters.Add("@id", model.bpid);
            parameters.Add("@bptype", model.bptype);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }

        // 🔹 SELECT LIST
        public async Task<BodyPartWithDoctorsResponse> GetBodyPartList()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectBodyPartList");

            using var connection = context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(
                "sp_master",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var bodyParts = (await multi.ReadAsync<BodyPartList>()).ToList();
            var doctors = (await multi.ReadAsync<DoctorListModel>()).ToList();

            return new BodyPartWithDoctorsResponse
            {
                BodyParts = bodyParts,
                Doctors = doctors
            };
        }
        public async Task<int> InsertMapping(InsertMapping model)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertMapping");
            parameters.Add("@bpid", model.bpid);
            parameters.Add("@disease_type", model.disease_type);
            parameters.Add("@diseaseIds", model.diseaseIds);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@Result");
            }
        }

        // 🔹 GET MAPPING LIST
        public async Task<IEnumerable<MappingList>> GetMappingList()
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectMappingList");

            using (var connection = context.CreateConnection())
            {
                var result = await connection.QueryAsync<MappingList>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        // 🔹 DELETE MAPPING
        public async Task<int> DeleteMapping(int id)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "DeleteMapping");
            parameters.Add("@id", id);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@Result");
            }
        }

        // 🔹 GET MAPPINGS BY BODY PART
        public async Task<IEnumerable<MappingsByBodyPart>> GetMappingsByBodyPart(int bpid)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetMappingsByBodyPart");
            parameters.Add("@bpid", bpid);

            using (var connection = context.CreateConnection())
            {
                var result = await connection.QueryAsync<MappingsByBodyPart>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task<int> InsertOpd(OpdModel model)
        {
            var query = "sp_master";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertOpd");
            parameters.Add("@pid", model.pid ?? 0);
            parameters.Add("@name", model.name);
            parameters.Add("@referral_doctor", model.referral_doctor);
            parameters.Add("@age", model.age);
            parameters.Add("@weight", model.weight);
            parameters.Add("@address", model.address);
            parameters.Add("@date", model.date);
            parameters.Add("@sex", model.sex);
            parameters.Add("@time", model.time);
            parameters.Add("@profession", model.profession);
            parameters.Add("@mobile", model.mobile);
            parameters.Add("@mlc_no", model.mlc_no);

            // Use single bpid instead of array (to match your SP)
            int bpidValue = 0;
            if (model.bpids != null && model.bpids.Any())
            {
                bpidValue = model.bpids.First(); // Take first bpid from array
            }
            else if (model.bpid > 0)
            {
                bpidValue = model.bpid;
            }

            parameters.Add("@bpid", bpidValue);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        public async Task<int> UpdateOpd(OpdModel model)
        {
            if (model.pid == null || model.pid <= 0)
                throw new ArgumentException("PID is required for update");

            var query = "sp_master";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateOpd");
            parameters.Add("@pid", model.pid);
            parameters.Add("@bpid", model.bpid);
            parameters.Add("@name", model.name);
            parameters.Add("@referral_doctor", model.referral_doctor);
            parameters.Add("@age", model.age);
            parameters.Add("@weight", model.weight);
            parameters.Add("@address", model.address);
            parameters.Add("@sex", model.sex);
            parameters.Add("@time", model.time);
            parameters.Add("@profession", model.profession);
            parameters.Add("@mobile", model.mobile);
            parameters.Add("@mlc_no", model.mlc_no);

            // Handle bpids array - Convert to DataTable for Table-Valued Parameter
            var bpidsTable = new DataTable();
            bpidsTable.Columns.Add("bpid", typeof(int));

            if (model.bpids != null && model.bpids.Any())
            {
                foreach (var bpid in model.bpids)
                {
                    bpidsTable.Rows.Add(bpid);
                }
            }
            else if (model.bpid > 0)
            {
                // Fallback: If bpids array nahi ahe tar single bpid use kar
                bpidsTable.Rows.Add(model.bpid);
            }

            parameters.Add("@bpids", bpidsTable.AsTableValuedParameter("dbo.BpidListType"));
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        public async Task<IEnumerable<PatientList>> GetPatientList(DateTime? filterDate = null)
        {
            var query = "sp_master";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectPatientList");
            parameters.Add("@date", filterDate);  // Changed from @FilterDate to @date

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<PatientList>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<OpdModel?> GetPatientById(int pid)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetPatientById");
            parameters.Add("@pid", pid);

            using var connection = context.CreateConnection();

            var result = await connection.QueryAsync<OpdModel>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var patient = result.FirstOrDefault();

            // Fetch associated body parts
            if (patient != null)
            {
                var bpidsQuery = "SELECT bpid FROM m_patient_bodypart WHERE pid = @pid";
                var bpidsList = await connection.QueryAsync<int>(bpidsQuery, new { pid });
                patient.bpids = bpidsList.ToList();
            }

            return patient;
        }

        // 🔹 GET PATIENT BY OPD NO
        public async Task<OpdModel> GetPatientByOpdNo(int opd_no)
        {
            var query = "sp_master";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetPatientByOpdNo");
            parameters.Add("@opd_no", opd_no);

            using var connection = context.CreateConnection();

            var result = await connection.QueryAsync<OpdModel>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault(); // जर patient मिळाला तर OpdModel return, नाही तर null
        }

        // Save Fees
        public async Task<int> SaveFees(FeesModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SaveFees");
            parameters.Add("@pid", model.pid);
            parameters.Add("@opd_no", model.opd_no);
            parameters.Add("@fees", model.fees);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
            // >0 = new fees id, -1 = patient not found, 0 = error
        }

        // Add this method to your SOC_Repository class
        public async Task<int> UpdateFees(UpdateFeesModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateFees");
            parameters.Add("@id", model.id);
            parameters.Add("@fees", model.fees);
            parameters.Add("@date", model.fee_date);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
            // Returns: 1 = Success, 0 = Failed/Not Found
        }

        // Get Fees List (optional date parameter)
        public async Task<IEnumerable<FeesList>> GetFeesList(DateTime? date = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetFeesList");
            parameters.Add("@date", date);

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<FeesList>("sp_master", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertItem(InsertItem model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertItem");
            parameters.Add("@item_name", model.item_name);
            parameters.Add("@rate", model.rate);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
            // Returns: item_id on success, 0 on failure
        }

        public async Task<int> UpdateItem(UpdateItem model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateItem");
            parameters.Add("@item_id", model.item_id);
            parameters.Add("@item_name", model.item_name);
            parameters.Add("@rate", model.rate);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync("sp_master", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
            // Returns: 1 on success, 0 on failure
        }

        public async Task<IEnumerable<ItemsList>> GetItemsList()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectItemsList");

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<ItemsList>(
                "sp_master",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeletePatientBodyPart(int pid, int bpid)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "DeletePatientBodyPart");
            parameters.Add("@pid", pid);
            parameters.Add("@bpid", bpid);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int?>("@Result") ?? 0;
            }
        }


      

        public async Task<IEnumerable<MappingsByBodyPartAndType>> GetMappingsByBodyPartAndType(int bpid, string disease_type)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetMappingsByBodyPartAndType");
            parameters.Add("@bpid", bpid);
            parameters.Add("@disease_type", disease_type);

            using (var connection = context.CreateConnection())
            {
                var result = await connection.QueryAsync<MappingsByBodyPartAndType>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        // ✅ CORRECTED: InsertBillItems Method
        public async Task<int> InsertBill(InsertBillModel model)
        {
            var query = "sp_master";

            // TVP for bill items
            var billTable = new DataTable();
            billTable.Columns.Add("item_name", typeof(string));
            billTable.Columns.Add("quantity", typeof(int));
            billTable.Columns.Add("rate", typeof(decimal));
            billTable.Columns.Add("amount", typeof(decimal));

            foreach (var item in model.items)
            {
                billTable.Rows.Add(
                    item.item_name,
                    item.quantity,
                    item.rate,
                    item.amount
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InsertBill");
            parameters.Add("@pid", model.pid);
            parameters.Add("@opd_no", model.opd_no);
            parameters.Add("@date", model.date);
            parameters.Add("@from_date", model.from_date);
            parameters.Add("@to_date", model.to_date);
            parameters.Add("@amount", model.amount);
            parameters.Add("@discount", model.discount);
            parameters.Add("@roomno", model.roomno);
            parameters.Add("@flag", model.flag);
            parameters.Add("@TempBill", billTable.AsTableValuedParameter("dbo.TempBill"));
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }

        public async Task<int> UpdateBill(UpdateBillModel model)
        {
            var query = "sp_master";

            // 🔹 Create TVP
            var billItemsTable = new DataTable();
            billItemsTable.Columns.Add("item_name", typeof(string));
            billItemsTable.Columns.Add("quantity", typeof(int));
            billItemsTable.Columns.Add("rate", typeof(decimal));
            billItemsTable.Columns.Add("amount", typeof(decimal));

            foreach (var item in model.items)
            {
                billItemsTable.Rows.Add(
                    item.item_name,
                    item.quantity,
                    item.rate,
                    item.amount
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateBillI");
            parameters.Add("@bill_no", model.bill_no);
            parameters.Add("@pid", model.pid);
            parameters.Add("@opd_no", model.opd_no);
            parameters.Add("@date", model.date);
            parameters.Add("@from_date", model.from_date);
            parameters.Add("@to_date", model.to_date);
            parameters.Add("@amount", model.amount);
            parameters.Add("@discount", model.discount);
            parameters.Add("@roomno", model.roomno);
            parameters.Add("@flag", model.flag);
            parameters.Add("@TempBill", billItemsTable.AsTableValuedParameter("dbo.TempBill"));
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        // 🔹 GET BILL ITEMS BY DATE
        public async Task<IEnumerable<BillListByDateDto>> GetBillItemsByDateFlag(DateTime date, string flag)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetBillItemsByDateFlag");
            parameters.Add("@date", date);
            parameters.Add("@flag", flag);

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<BillListByDateDto>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<BillItemDetailDto>> GetBillItemsDetailsByBillNo(int bill_no, string flag)
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetBillItemsDetailsByBillNo");
            parameters.Add("@bill_no", bill_no);
            parameters.Add("@flag", flag);

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<BillItemDetailDto>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<IEnumerable<DoctorListModel>> GetDoctorList()
        {
            var query = "sp_master";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectDoctorList");

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<DoctorListModel>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ItemRateDto>> GetItemRateList()
        {
            using var connection = context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectRate");

            var result = await connection.QueryAsync<ItemRateDto>(
                "sp_master",   // ✅ SAME SP NAME
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<IEnumerable<PatientList>> SelectAllPatientList()
        {
            var query = "sp_master";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "SelectAllPatientList");

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<PatientList>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
