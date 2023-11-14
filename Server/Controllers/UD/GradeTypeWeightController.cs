using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Shared;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using AutoMapper;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]

    public class GradeTypeWeightController : BaseController, GenericRestController<GradeTypeWeightDTO>
    {
        public GradeTypeWeightController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }


        [HttpGet]
        [Route("Get/{SchoolId}/{SectionId}")]
        //  Route for this is....  <URL>/api/GradeTypeWeight/Get/10
        public async Task<IActionResult> Get(int SchoolId, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeTypeWeightDTO? result = await _context
                    .GradeTypeWeights
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolId)
                     .Select(sp => new GradeTypeWeightDTO
                     {
                         SchoolId = sp.SchoolId,
                         SectionId = sp.SectionId,
                         GradeTypeCode = sp.GradeTypeCode,
                         NumberPerSection = sp.NumberPerSection,
                         PercentOfFinalGrade = sp.PercentOfFinalGrade,
                         DropLowest = sp.DropLowest,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate

                     })
                .SingleOrDefaultAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeTypeWeights.Select(sp => new GradeTypeWeightDTO
                {
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    NumberPerSection = sp.NumberPerSection,
                    PercentOfFinalGrade = sp.PercentOfFinalGrade,
                    DropLowest = sp.DropLowest,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody]
                                                GradeTypeWeightDTO _GradeTypeWeightDTO)
        {
            Debugger.Launch();
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypeWeights.Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    GradeTypeWeight c = new GradeTypeWeight
                    {
                        SchoolId = _GradeTypeWeightDTO.SchoolId,
                        SectionId = _GradeTypeWeightDTO.SectionId,
                        GradeTypeCode = _GradeTypeWeightDTO.GradeTypeCode,
                        NumberPerSection = _GradeTypeWeightDTO.NumberPerSection,
                        PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade,
                        DropLowest = _GradeTypeWeightDTO.DropLowest,
                        
                    };
                    _context.GradeTypeWeights.Add(c);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody]
                                                GradeTypeWeightDTO _GradeTypeWeightDTO)
        {

            Debugger.Launch();

            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypeWeights.Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId).FirstOrDefaultAsync();

                itm.NumberPerSection = _GradeTypeWeightDTO.NumberPerSection;
                itm.PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade;
                itm.DropLowest = _GradeTypeWeightDTO.DropLowest;

                _context.GradeTypeWeights.Update(itm);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        [HttpDelete]
        [Route("Delete/{SchoolId}/{SectionId}")]
        public async Task<IActionResult> Delete(int SchoolId, int SectionId)
        {

            Debugger.Launch();

            try
            {
                await _context.Database.BeginTransactionAsync();


                var itm = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.SectionId == SectionId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeTypeWeights.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}")]
        public async Task<IActionResult> Delete(int SchoolId)
        {
            Debugger.Launch();

            try
            {
                await _context.Database.BeginTransactionAsync();


                var itm = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeTypeWeights.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        [HttpGet]
        [Route("Get/{SchoolId}")]
        public async Task<IActionResult> Get(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeTypeWeightDTO? result = await _context
                    .GradeTypeWeights
                    .Where(x => x.SchoolId == SchoolId)
                     .Select(sp => new GradeTypeWeightDTO
                     {
                         SchoolId = sp.SchoolId,
                         SectionId = sp.SectionId,
                         GradeTypeCode = sp.GradeTypeCode,
                         NumberPerSection = sp.NumberPerSection,
                         PercentOfFinalGrade = sp.PercentOfFinalGrade,
                         DropLowest = sp.DropLowest,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate
                     })
                .SingleOrDefaultAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }
}
