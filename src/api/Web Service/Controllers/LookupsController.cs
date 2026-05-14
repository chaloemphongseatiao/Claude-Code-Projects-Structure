using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TemplateWebService.Data;
using TemplateWebService.Helpers;
using TemplateWebService.Models.Constants;
using TemplateWebService.Models.Entities;
using TemplateWebService.Models.Requests.Lookups;
using TemplateWebService.Models.Responses.Lookups;
using TemplateWebService.Models.Shared;

namespace TemplateWebService.Controllers
{
    [ApiController]
    [Route("api/lookups")]
    [Produces("application/json")]
    public class LookupsController : ControllerBase
    {
        private readonly LomaLottoContext _context;

        public LookupsController(LomaLottoContext context)
        {
            _context = context;
        }

        #region GET

        [HttpGet("{lookupType}")]
        [ProducesResponseType(typeof(ApiResponse<List<LookupResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<LookupResponse>>>> GetLookup(
            [FromRoute] string lookupType,
            [FromQuery] bool includeInactive = false,
            CancellationToken ct = default)
        {
            try
            {
                var query = BuildLookupQuery(lookupType, includeInactive);

                if (query is null)
                {
                    return NotFound(
                        ApiResponseHelper.Fail<List<LookupResponse>>("เนเธกเนเธเธเธเธฃเธฐเน€เธ เธ— lookup เธ—เธตเนเธฃเธฐเธเธธ"));
                }

                var data = await query.ToListAsync(ct);

                return Ok(
                    ApiResponseHelper.Ok(
                        data,
                        data.Count > 0 ? "เธ”เธถเธเธเนเธญเธกเธนเธฅเธชเธณเน€เธฃเนเธ" : "เนเธกเนเธเธเธเนเธญเธกเธนเธฅ"));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<List<LookupResponse>>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseHelper.ApiError(ex));
            }
        }

        #endregion

        #region CREATE

        [HttpPost("{lookupType}")]
        [ProducesResponseType(typeof(ApiResponse<LookupResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<LookupResponse>>> CreateLookup(
            [FromRoute] string lookupType,
            [FromBody] LookupUpsertRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเนเธญเธกเธนเธฅเนเธกเนเธ–เธนเธเธ•เนเธญเธ"));
                }

                request.Code = (request.Code ?? string.Empty).Trim().ToUpperInvariant();
                request.Name = (request.Name ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธฃเธซเธฑเธช"));
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธเธทเนเธญ"));
                }

                var createdBy = GetCurrentUser();

                var createResult = await CreateLookupInternalAsync(lookupType, request, createdBy, ct);

                if (!createResult.Success)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>(createResult.Message));
                }

                return Ok(
                    ApiResponseHelper.Ok(
                        createResult.Data!,
                        "เธชเธฃเนเธฒเธเธเนเธญเธกเธนเธฅเธชเธณเน€เธฃเนเธ"));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<LookupResponse>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ApiResponseHelper.ApiError(ex));
            }
        }

        #endregion

        #region UPDATE

        [HttpPut("{lookupType}/{code}")]
        [ProducesResponseType(typeof(ApiResponse<LookupResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<LookupResponse>>> UpdateLookup(
            [FromRoute] string lookupType,
            [FromRoute] string code,
            [FromBody] LookupUpsertRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเนเธญเธกเธนเธฅเนเธกเนเธ–เธนเธเธ•เนเธญเธ"));
                }

                code = (code ?? string.Empty).Trim().ToUpperInvariant();
                request.Code = (request.Code ?? string.Empty).Trim().ToUpperInvariant();
                request.Name = (request.Name ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธฃเธซเธฑเธชเธ—เธตเนเธ•เนเธญเธเธเธฒเธฃเนเธเนเนเธ"));
                }

                if (!string.Equals(code, request.Code, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธฃเธซเธฑเธชเนเธ route เนเธฅเธฐ body เธ•เนเธญเธเธ•เธฃเธเธเธฑเธ"));
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเธเธทเนเธญ"));
                }

                var updatedBy = GetCurrentUser();

                var updateResult = await UpdateLookupInternalAsync(lookupType, request, updatedBy, ct);

                if (!updateResult.Success)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<LookupResponse>(updateResult.Message));
                }

                return Ok(
                    ApiResponseHelper.Ok(
                        updateResult.Data!,
                        "เนเธเนเนเธเธเนเธญเธกเธนเธฅเธชเธณเน€เธฃเนเธ"));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<LookupResponse>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ApiResponseHelper.ApiError(ex));
            }
        }

        #endregion

        #region Private - Query Builders

        private IQueryable<LookupResponse>? BuildLookupQuery(
            string lookupType,
            bool includeInactive)
        {
            lookupType = NormalizeLookupType(lookupType);

            return lookupType switch
            {
                // For each lookup type, we project the corresponding DbSet to LookupResponse

                _ => null
            };
        }

        #endregion

        #region Private - Create

        private async Task<LookupCommandResult> CreateLookupInternalAsync(
            string lookupType,
            LookupUpsertRequest request,
            string createdBy,
            CancellationToken ct)
        {
            lookupType = NormalizeLookupType(lookupType);

            switch (lookupType)
            {
                // For each lookup type, we call CreateAsync with the corresponding DbSet and a factory function to create the entity

                default:
                    return LookupCommandResult.Fail("เนเธกเนเธเธเธเธฃเธฐเน€เธ เธ— lookup เธ—เธตเนเธฃเธฐเธเธธ");
            }
        }

        #endregion

        #region Private - Update

        private async Task<LookupCommandResult> UpdateLookupInternalAsync(
            string lookupType,
            LookupUpsertRequest request,
            string updatedBy,
            CancellationToken ct)
        {
            lookupType = NormalizeLookupType(lookupType);

            switch (lookupType)
            {
                // For each lookup type, we call UpdateAsync with the corresponding DbSet

                default:
                    return LookupCommandResult.Fail("เนเธกเนเธเธเธเธฃเธฐเน€เธ เธ— lookup เธ—เธตเนเธฃเธฐเธเธธ");
            }
        }

        #endregion

        #region Private - Generic Helpers

        private async Task<LookupCommandResult> CreateAsync<TEntity>(
            DbSet<TEntity> dbSet,
            LookupUpsertRequest request,
            string createdBy,
            CancellationToken ct,
            Func<LookupUpsertRequest, string, TEntity> factory)
            where TEntity : class
        {
            var exists = await dbSet.AnyAsync(
                x => EF.Property<string>(x, "Code") == request.Code,
                ct);

            if (exists)
            {
                return LookupCommandResult.Fail("เธฃเธซเธฑเธชเธเธตเนเธกเธตเธญเธขเธนเนเนเธฅเนเธงเนเธเธฃเธฐเธเธ");
            }

            var entity = factory(request, createdBy);

            dbSet.Add(entity);
            await _context.SaveChangesAsync(ct);

            return LookupCommandResult.Ok(new LookupResponse
            {
                Code = request.Code,
                Name = request.Name,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            });
        }

        private async Task<LookupCommandResult> UpdateAsync<TEntity>(
            DbSet<TEntity> dbSet,
            LookupUpsertRequest request,
            string updatedBy,
            CancellationToken ct)
            where TEntity : class
        {
            var entity = await dbSet.FirstOrDefaultAsync(
                x => EF.Property<string>(x, "Code") == request.Code,
                ct);

            if (entity is null)
            {
                return LookupCommandResult.Fail("เนเธกเนเธเธเธเนเธญเธกเธนเธฅเธ—เธตเนเธ•เนเธญเธเธเธฒเธฃเนเธเนเนเธ");
            }

            typeof(TEntity).GetProperty("Name")?.SetValue(entity, request.Name);
            typeof(TEntity).GetProperty("SortOrder")?.SetValue(entity, request.SortOrder);
            typeof(TEntity).GetProperty("IsActive")?.SetValue(entity, request.IsActive);
            typeof(TEntity).GetProperty("UpdatedBy")?.SetValue(entity, updatedBy);
            typeof(TEntity).GetProperty("UpdatedDate")?.SetValue(entity, DateTime.Now);

            await _context.SaveChangesAsync(ct);

            return LookupCommandResult.Ok(new LookupResponse
            {
                Code = request.Code,
                Name = request.Name,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            });
        }

        private static string NormalizeLookupType(string lookupType)
        {
            return (lookupType ?? string.Empty).Trim().ToLowerInvariant();
        }

        private string GetCurrentUser()
        {
            return User?.Identity?.Name?.Trim() switch
            {
                { Length: > 0 } name => name,
                _ => "system"
            };
        }

        #endregion

        #region Private - Result Class

        private sealed class LookupCommandResult
        {
            public bool Success { get; private set; }

            public string Message { get; private set; } = string.Empty;

            public LookupResponse? Data { get; private set; }

            public static LookupCommandResult Ok(LookupResponse data)
            {
                return new LookupCommandResult
                {
                    Success = true,
                    Data = data,
                    Message = "เธชเธณเน€เธฃเนเธ"
                };
            }

            public static LookupCommandResult Fail(string message)
            {
                return new LookupCommandResult
                {
                    Success = false,
                    Message = message
                };
            }
        }

        #endregion
    }
}