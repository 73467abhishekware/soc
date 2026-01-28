using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using SOC.Models;
using SOC.Repository;

namespace SOC.Controllers
{
    [ApiController]
    [Route("api/")]
    [Table("Table")]
    public class SOC_Controller : ControllerBase
    {
        private readonly ISOC_Repository soc_repository;

        public SOC_Controller(ISOC_Repository socRepository)
        {
            this.soc_repository = socRepository;
        }

        [HttpGet]
        [Route("Login/{username}/{password}")]
        public async Task<ActionResult> login(string username, string password)
        {
            try
            {
                //ExportQRs objectExport = new ExportQRs();
                //objectExport.generatePdf();


                var result = await soc_repository.login(username, password);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                    //return NotFound();
                }
                return Ok(result);
                //return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        //[HttpPost]
        //[Route("InsertOpd")]
        //public async Task<IActionResult> InsertOpd([FromBody] OpdModel model)
        //{
        //    try
        //    {
        //        var result = await soc_repository.InsertOpd(model);

        //        if (result == 1)
        //            return Ok(new { message = "OPD Inserted Successfully" });

        //        return BadRequest(new { message = "Insert Failed" });
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}


        [HttpPost]
        [Route("InsertDisease")]
        public async Task<IActionResult> InsertDisease([FromBody] InsertDisease model)
        {
            try
            {
                var result = await soc_repository.InsertDisease(model);

                if (result == 1)
                    return Ok(new { message = "Inserted Successfully" });

                return BadRequest(new { message = "Insert Failed" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        [Route("UpdateDisease")]
        public async Task<IActionResult> UpdateDisease([FromBody] UpdateDisease model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.disease))
                    return BadRequest(new { message = "Invalid input" });

                var result = await soc_repository.UpdateDisease(model);

                if (result == 1)
                    return Ok(new { message = "Updated Successfully" });

                return NotFound(new { message = "Disease ID not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetDiseaseList")]
        public async Task<IActionResult> GetDiseaseList()
        {
            try
            {
                var result = await soc_repository.GetDiseaseList();

                if (result == null || !result.Any())
                    return NotFound(new { message = "No diseases found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("InsertBodyPart")]
        public async Task<IActionResult> InsertBodyPart([FromBody] InsertBodyPart model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.bptype))
                    return BadRequest(new { message = "Invalid body part" });

                var result = await soc_repository.InsertBodyPart(model);

                if (result == 1)
                    return Ok(new { message = "Inserted Successfully" });

                return BadRequest(new { message = "Insert Failed" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // 🔹 UPDATE BODY PART
        [HttpPost("UpdateBodyPart")]
        public async Task<IActionResult> UpdateBodyPart([FromBody] UpdateBodyPart model)
        {
            try
            {
                var result = await soc_repository.UpdateBodyPart(model);

                if (result == 1)
                    return Ok(new { message = "Updated Successfully" });

                return BadRequest(new { message = "Update Failed" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetBodyPartList")]
        public async Task<IActionResult> GetBodyPartList()
        {
            try
            {
                var result = await soc_repository.GetBodyPartList();
                return Ok(new
                {
                    success = true,
                    bodyParts = result.BodyParts,
                    doctors = result.Doctors
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        // 🔹 INSERT MAPPING
        [HttpPost("InsertMapping")]
        public async Task<IActionResult> InsertMapping([FromBody] InsertMapping model)
        {
            try
            {
                // 🔹 UPDATED VALIDATION: Check diseaseIds instead of did
                if (model == null ||
                    model.bpid == 0 ||
                    string.IsNullOrWhiteSpace(model.diseaseIds) ||
                    string.IsNullOrWhiteSpace(model.disease_type))
                {
                    return BadRequest(new { message = "Invalid input data. Please provide bpid, disease_type, and diseaseIds." });
                }

                var result = await soc_repository.InsertMapping(model);

                if (result == 1)
                    return Ok(new { result = 1, message = "Mapping inserted successfully" });

                return BadRequest(new { result = 0, message = "Insert failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }
        // 🔹 GET MAPPING LIST
        [HttpGet("GetMappingList")]
        public async Task<IActionResult> GetMappingList()
        {
            try
            {
                var result = await soc_repository.GetMappingList();

                if (result == null || !result.Any())
                    return NotFound(new { message = "No mappings found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        // 🔹 DELETE MAPPING
        [HttpPost("DeleteMapping")]
        public async Task<IActionResult> DeleteMapping([FromBody] DeleteMapping model)
        {
            try
            {
                if (model == null || model.id == 0)
                    return BadRequest(new { message = "Invalid mapping ID" });

                var result = await soc_repository.DeleteMapping(model.id);

                if (result == 1)
                    return Ok(new { result = 1, message = "Mapping deleted successfully" });

                return NotFound(new { result = 0, message = "Mapping not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        // 🔹 GET MAPPINGS BY BODY PART ID
        [HttpGet("GetMappingsByBodyPart/{bpid}")]
        public async Task<IActionResult> GetMappingsByBodyPart(int bpid)
        {
            try
            {
                if (bpid <= 0)
                    return BadRequest(new { message = "Invalid body part ID" });

                var result = await soc_repository.GetMappingsByBodyPart(bpid);

                if (result == null || !result.Any())
                    return NotFound(new { message = "No mappings found for this body part" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        [HttpPost("InsertOpd")]
        public async Task<IActionResult> InsertOpd([FromBody] OpdModel model)
        {
            try
            {
                // Enforce single body part selection
                int selectedBpid = 0;
                if (model.bpids != null && model.bpids.Any())
                {
                    if (model.bpids.Count() > 1)
                        return BadRequest(new { message = "Please select only one body part" });
                    selectedBpid = model.bpids.First();
                }
                else if (model.bpid > 0)
                {
                    selectedBpid = model.bpid;
                }

                if (selectedBpid == 0)
                    return BadRequest(new { message = "Body part must be selected" });

                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Invalid data" });

                // Set the single bpid for processing
                model.bpid = selectedBpid;

                var result = await soc_repository.InsertOpd(model);

                if (result > 0)
                    return Ok(new { message = "Patient registered successfully", pid = result });
                if (result == -1)
                    return Conflict(new { message = "Name already exists" });

                return BadRequest(new { message = "Failed to register patient" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }
        // 🔹 UPDATE OPD PATIENT
        [HttpPost("UpdateOpd")]
        public async Task<IActionResult> UpdateOpd([FromBody] OpdModel model)
        {
            try
            {
                if (!ModelState.IsValid || model.pid == null || model.pid <= 0)
                    return BadRequest(new { message = "Valid PID is required for update" });

                var result = await soc_repository.UpdateOpd(model);

                if (result == 1)
                    return Ok(new { message = "Patient updated successfully" });

                return NotFound(new { message = "Patient not found or update failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        [HttpGet("GetPatientList/{date}")]
        public async Task<IActionResult> GetPatientList(string date)
        {
            try
            {
                DateTime? filterDate = null;

                // date validate & parse
                if (!string.IsNullOrWhiteSpace(date))
                {
                    if (DateTime.TryParse(date, out DateTime parsedDate))
                    {
                        filterDate = parsedDate.Date;
                    }
                    else
                    {
                        return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD" });
                    }
                }

                var result = await soc_repository.GetPatientList(filterDate);

                if (result == null || !result.Any())
                    return NotFound(new { message = "No patients found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }


        // 🔹 GET PATIENT BY ID (FOR EDIT)
        [HttpGet("GetPatientById/{pid}")]
        public async Task<IActionResult> GetPatientById(int pid)
        {
            try
            {
                if (pid <= 0)
                    return BadRequest(new { message = "Invalid Patient ID" });

                var patient = await soc_repository.GetPatientById(pid);

                if (patient == null)
                    return NotFound(new { message = "Patient not found" });

                return Ok(new
                {
                    status = "Success",
                    data = patient
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Server error", error = ex.Message });
            }
        }

        // 🔹 GET PATIENT BY OPD NO
        [HttpGet("GetPatientByOpdNo/{opd_no}")]
        public async Task<IActionResult> GetPatientByOpdNo(int opd_no)
        {
            try
            {
                if (opd_no <= 0)
                    return BadRequest(new { message = "Invalid OPD Number" });

                var patient = await soc_repository.GetPatientByOpdNo(opd_no);

                if (patient == null)
                    return NotFound(new { message = "Patient not found", status = "Patient not found" });

                return Ok(new
                {
                    message = "Patient found successfully",
                    status = "Success",
                    data = patient
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Server error", error = ex.Message });
            }
        }


        [HttpPost("SaveFees")]
        public async Task<IActionResult> SaveFees([FromBody] FeesModel model)
        {
            if (model == null || model.pid <= 0 || model.fees <= 0)
                return BadRequest(new { message = "Invalid fees data" });

            var result = await soc_repository.SaveFees(model);

            if (result > 0)
                return Ok(new { message = "Fees saved successfully", fees_id = result });
            if (result == -1)
                return NotFound(new { message = "Patient not found" });

            return BadRequest(new { message = "Failed to save fees" });
        }

        // Add this method to your SOC_Controller class

        // 🔹 UPDATE FEES
        [HttpPost("UpdateFees")]
        public async Task<IActionResult> UpdateFees([FromBody] UpdateFeesModel model)
        {
            try
            {
                if (model == null || model.id <= 0 || model.fees <= 0)
                    return BadRequest(new { message = "Invalid fees data. ID and fees amount are required." });

                var result = await soc_repository.UpdateFees(model);

                if (result == 1)
                    return Ok(new { message = "Fees updated successfully" });

                return NotFound(new { message = "Fees record not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        [HttpGet("GetFeesList")]
        public async Task<IActionResult> GetFeesList([FromQuery] DateTime? date = null)
        {
            var result = await soc_repository.GetFeesList(date);
            return Ok(result);
        }

        [HttpPost("InsertItem")]
        public async Task<IActionResult> InsertItem([FromBody] InsertItem model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.item_name) || model.rate <= 0)
                    return BadRequest(new { message = "Invalid item data. Item name and rate are required." });

                var result = await soc_repository.InsertItem(model);

                if (result > 0)
                    return Ok(new { message = "Item inserted successfully", item_id = result });

                return BadRequest(new { message = "Failed to insert item" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        // 🔹 UPDATE ITEM
        [HttpPost("UpdateItem/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItem model)
        {
            model.item_id = id; // ✅ route → model

            if (model == null || model.item_id <= 0 ||
                string.IsNullOrWhiteSpace(model.item_name) || model.rate <= 0)
                return BadRequest(new { message = "Invalid item data" });

            var result = await soc_repository.UpdateItem(model);

            if (result == 1)
                return Ok(new { message = "Item updated successfully" });

            return NotFound(new { message = "Item not found" });
        }


        // 🔹 GET ITEMS LIST
        [HttpGet("GetItemsList")]
        public async Task<IActionResult> GetItemsList()
        {
            try
            {
                var result = await soc_repository.GetItemsList();

                if (result == null || !result.Any())
                    return NotFound(new { message = "No items found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }


        // 🔹 DELETE PATIENT BODY PART
        [HttpPost("DeletePatientBodyPart/{pid}/{bpid}")]
        public async Task<IActionResult> DeletePatientBodyPart(int pid, int bpid)
        {
            try
            {
                if (pid <= 0 || bpid <= 0)
                    return BadRequest(new { message = "Valid PID and BPID are required" });

                var result = await soc_repository.DeletePatientBodyPart(pid, bpid);

                if (result == 1)
                    return Ok(new { result = 1, message = "Patient body part mapping deleted successfully" });

                return BadRequest(new { result = 0, message = "Delete failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }


        [HttpGet("GetMappingsByBodyPartAndType/{bpid}/{disease_type}")]
        public async Task<IActionResult> GetMappingsByBodyPartAndType(int bpid, string disease_type)
        {
            try
            {
                if (bpid <= 0)
                    return BadRequest(new { message = "Invalid body part ID" });

                if (string.IsNullOrWhiteSpace(disease_type))
                    return BadRequest(new { message = "Disease type is required" });

                var result = await soc_repository.GetMappingsByBodyPartAndType(bpid, disease_type);

                if (result == null || !result.Any())
                    return NotFound(new { message = "No mappings found for this body part and disease type" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }

        // ✅ CORRECTED: InsertBillItems Controller
        [HttpPost("InsertBill")]
        public async Task<IActionResult> InsertBill([FromBody] InsertBillModel model)
        {
            try
            {
                if (model == null || model.pid <= 0)
                    return BadRequest(new { message = "Invalid bill data" });

                if (model.items == null || !model.items.Any())
                    return BadRequest(new { message = "At least one bill item required" });

                foreach (var item in model.items)
                {
                    if (item.quantity <= 0 || item.rate <= 0)
                        return BadRequest(new { message = $"Invalid item quantity or rate for {item.item_name}" });

                    // Decimal tolerance
                    if (Math.Abs(item.amount - (item.quantity * item.rate)) > 0.01M)
                        return BadRequest(new { message = $"Amount mismatch for {item.item_name}" });
                }

                var result = await soc_repository.InsertBill(model);

                if (result == 1)
                    return Ok(new { result = 1, message = "Bill inserted successfully" });

                if (result == -1)
                    return Conflict(new { result = -1, message = "Patient already exists for this date" });

                return BadRequest(new { result = 0, message = "Insert failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("UpdateBill")]
        public async Task<IActionResult> UpdateBill([FromBody] UpdateBillModel model)
        {
            try
            {
                if (model == null || model.bill_no <= 0 || model.pid <= 0)
                    return BadRequest(new { message = "Invalid bill data" });

                if (model.items == null || !model.items.Any())
                    return BadRequest(new { message = "At least one bill item required" });

                foreach (var item in model.items)
                {
                    if (string.IsNullOrWhiteSpace(item.item_name))
                        return BadRequest(new { message = "Item name is required" });

                    if (item.quantity <= 0 || item.rate <= 0)
                        return BadRequest(new { message = $"Invalid quantity or rate for {item.item_name}" });

                    if (item.amount != item.quantity * item.rate)
                        return BadRequest(new { message = $"Amount mismatch for {item.item_name}" });
                }

                var result = await soc_repository.UpdateBill(model);

                if (result == 1)
                    return Ok(new { result = 1, message = "Bill updated successfully" });

                if (result == -1)
                    return NotFound(new { result = -1, message = "Patient not found" });

                if (result == -2)
                    return NotFound(new { result = -2, message = "Bill not found" });

                return BadRequest(new { result = 0, message = "Update failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetBillItemsByDateFlag/{date}/{flag}")]
        public async Task<IActionResult> GetBillItemsByDateFlag(string date, string flag)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                    return BadRequest(new { message = "Invalid date format (YYYY-MM-DD)" });

                if (string.IsNullOrWhiteSpace(flag))
                    return BadRequest(new { message = "Flag is required" });

                var result = await soc_repository.GetBillItemsByDateFlag(parsedDate.Date, flag);

                if (result == null || !result.Any())
                    return NotFound(new { message = "No bills found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetBillItemsDetailsByBillNo/{bill_no}")]
        public async Task<IActionResult> GetBillItemsDetailsByBillNo(
    int bill_no,
    [FromQuery] string flag)
        {
            try
            {
                if (bill_no <= 0)
                    return BadRequest(new { message = "Invalid bill number" });

                if (string.IsNullOrWhiteSpace(flag))
                    return BadRequest(new { message = "Flag is required" });

                var result = await soc_repository.GetBillItemsDetailsByBillNo(bill_no, flag);

                if (result == null || !result.Any())
                    return NotFound(new { message = "No bill items found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetDoctorList")]
        public async Task<IActionResult> GetDoctorList()
        {
            try
            {
                var result = await soc_repository.GetDoctorList();

                if (result == null || !result.Any())
                    return NotFound(new { message = "No doctors found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error", error = ex.Message });
            }
        }

        [HttpGet("GetItemRateList")]
        public async Task<IActionResult> GetItemRateList()
        {
            var data = await soc_repository.GetItemRateList();
            return Ok(data);
        }


        [HttpGet("SelectAllPatientList")]
        public async Task<IActionResult> SelectAllPatientList()
        {
            try
            {

              
                var result = await soc_repository.SelectAllPatientList();

                if (result == null || !result.Any())
                    return NotFound(new { message = "No patients found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error: " + ex.Message });
            }
        }




    }
}
