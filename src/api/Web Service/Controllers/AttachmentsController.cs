using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using TemplateWebService.Data;
using TemplateWebService.Extensions;
using TemplateWebService.Helpers;
using TemplateWebService.Models.Entities;
using TemplateWebService.Models.Requests.Attachments;
using TemplateWebService.Models.Responses.Attachments;
using TemplateWebService.Models.Settings;
using TemplateWebService.Models.Shared;

namespace TemplateWebService.Controllers
{
    [ApiController]
    [Route("api/attachments")]
    [Produces("application/json")]
    public class AttachmentsController : ControllerBase
    {
        private readonly LomaLottoContext _context;
        private readonly IConfiguration _config;
        private readonly WebServiceSettings _settings;

        public AttachmentsController(
            LomaLottoContext context,
            IConfiguration config,
            IOptions<WebServiceSettings> settings)
        {
            _context = context;
            _config = config;
            _settings = settings.Value;
        }

        #region Endpoints

        [HttpPost]
        [RequestSizeLimit(1024L * 1024L * 300L)] // 300 MB
        public async Task<ActionResult<ApiResponse<AttachmentResponse>>> Upload(
            [FromForm] UploadAttachmentRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponseHelper.Fail<AttachmentResponse>(ModelState.GetErrorMessages()));
            }

            if (request.File is null || request.File.Length <= 0)
            {
                return BadRequest(
                    ApiResponseHelper.Fail<AttachmentResponse>("เนเธกเนเธเธเนเธเธฅเนเธ—เธตเนเธญเธฑเธเนเธซเธฅเธ”"));
            }

            try
            {
                if (_settings.FileServer.FileSource.Count == 0)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<AttachmentResponse>("เธขเธฑเธเนเธกเนเนเธ”เนเธ•เธฑเนเธเธเนเธฒเนเธซเธฅเนเธเธ—เธตเนเธญเธขเธนเนเธเธญเธเนเธเธฅเน"));
                }

                var sourceName = string.IsNullOrWhiteSpace(request.SourceName)
                    ? "Default"
                    : request.SourceName!.Trim();

                if (!_settings.FileServer.FileSource.TryGetValue(sourceName, out var src))
                {
                    src = _settings.FileServer.FileSource.Values.First();
                }

                var root = Path.GetFullPath(src.RemotePath);

                if (!Path.IsPathRooted(root))
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<AttachmentResponse>("RemotePath เนเธกเนเนเธเน absolute path"));
                }

                var ext = (Path.GetExtension(request.File.FileName) ?? ".bin")
                    .Trim('.')
                    .ToLowerInvariant();

                var subFolder = !string.IsNullOrWhiteSpace(request.Folder)
                    ? SanitizeFolder(request.Folder!)
                    : GuessSubFolder(ext);

                var newName = $"{Guid.NewGuid():N}.{(string.IsNullOrWhiteSpace(ext) ? "bin" : ext)}";

                var relative = string.IsNullOrWhiteSpace(subFolder)
                    ? newName
                    : Path.Combine(subFolder, newName);

                var fullPath = Path.Combine(root, relative);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                await using (var fs = System.IO.File.Create(fullPath))
                {
                    await request.File.CopyToAsync(fs, ct);
                }

                var attachment = new Attachment
                {
                    OriginalName = request.File.FileName,
                    FileName = newName,
                    FilePath = relative.Replace("\\", "/"),
                    FileExtension = ext,
                    ContentType = string.IsNullOrWhiteSpace(request.File.ContentType)
                        ? "application/octet-stream"
                        : request.File.ContentType,
                    FileSize = request.File.Length,
                    IsActive = false,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                _context.Attachments.Add(attachment);
                await _context.SaveChangesAsync(ct);

                var attachmentDto = AttachmentMapToResponse(attachment);

                return Ok(
                    ApiResponseHelper.Ok(
                        attachmentDto,
                        "เธญเธฑเธเนเธซเธฅเธ”เธชเธณเน€เธฃเนเธ"));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<AttachmentResponse>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ApiResponseHelper.ApiError(ex));
            }
        }

        [HttpGet("{fileId:long}")]
        public async Task<ActionResult<ApiResponse<AttachmentResponse>>> GetById(
            [FromRoute] long fileId,
            CancellationToken ct)
        {
            if (fileId <= 0)
            {
                return BadRequest(
                    ApiResponseHelper.Fail<AttachmentResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเนเธเธฅเน"));
            }

            try
            {
                var attachment = await _context.Attachments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.FileId == fileId &&
                            x.IsActive,
                        ct);

                if (attachment is null)
                {
                    return NotFound(
                        ApiResponseHelper.Fail<AttachmentResponse>("เนเธกเนเธเธเธเนเธญเธกเธนเธฅ"));
                }

                var attachmentDto = AttachmentMapToResponse(attachment);

                return Ok(
                    ApiResponseHelper.Ok(
                        attachmentDto,
                        "เธ”เธถเธเธเนเธญเธกเธนเธฅเธชเธณเน€เธฃเนเธ"));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<AttachmentResponse>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ApiResponseHelper.ApiError(ex));
            }
        }

        [HttpGet("{fileId:long}/download")]
        public async Task<IActionResult> DownloadById(
            [FromRoute] long fileId,
            CancellationToken ct)
        {
            if (fileId <= 0)
            {
                return BadRequest(
                    ApiResponseHelper.Fail<AttachmentResponse>("เธเธฃเธธเธ“เธฒเธฃเธฐเธเธธเนเธเธฅเน"));
            }

            try
            {
                var attachment = await _context.Attachments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.FileId == fileId &&
                            x.IsActive,
                        ct);

                if (attachment is null)
                {
                    return NotFound(
                        ApiResponseHelper.Fail<AttachmentResponse>("เนเธกเนเธเธเธเนเธญเธกเธนเธฅ"));
                }

                if (_settings.FileServer.FileSource.Count == 0)
                {
                    return BadRequest(
                        ApiResponseHelper.Fail<AttachmentResponse>("เธขเธฑเธเนเธกเนเนเธ”เนเธ•เธฑเนเธเธเนเธฒเนเธซเธฅเนเธเธ—เธตเนเธญเธขเธนเนเธเธญเธเนเธเธฅเน"));
                }

                var root = Path.GetFullPath(_settings.FileServer.FileSource.Values.First().RemotePath);

                var fullPath = Path.Combine(
                    root,
                    attachment.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound(
                        ApiResponseHelper.Fail<AttachmentResponse>("เนเธกเนเธเธเนเธเธฅเนเธเธเน€เธเธดเธฃเนเธเน€เธงเธญเธฃเน"));
                }

                var contentType = string.IsNullOrWhiteSpace(attachment.ContentType)
                    ? "application/octet-stream"
                    : attachment.ContentType;

                var downloadName = string.IsNullOrWhiteSpace(attachment.OriginalName)
                    ? attachment.FileName
                    : attachment.OriginalName;

                return PhysicalFile(
                    fullPath,
                    contentType,
                    downloadName);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    499,
                    ApiResponseHelper.Fail<AttachmentResponse>("เธเธณเธเธญเธ–เธนเธเธขเธเน€เธฅเธดเธ"));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ApiResponseHelper.ApiError(ex));
            }
        }

        #endregion

        #region Private - Mapping

        private AttachmentResponse AttachmentMapToResponse(Attachment f)
        {
            return new AttachmentResponse
            {
                FileId = f.FileId,
                OriginalName = f.OriginalName,
                FileName = f.FileName,
                FilePath = f.FilePath,
                FileExtension = f.FileExtension,
                ContentType = f.ContentType,
                FileSize = f.FileSize,
                IsActive = f.IsActive,
                PublicUrl = Request.ToPublicUrl(f.FilePath, _config)
            };
        }

        #endregion

        #region Private - Generic Helpers

        private static string GuessSubFolder(string ext) =>
            ext switch
            {
                "jpg" or "jpeg" or "png" or "gif" or "webp" => "images",
                "mp4" or "mov" or "mkv" => "videos",
                "mp3" or "m4a" or "wav" => "audio",
                "pdf" or "doc" or "docx" or "xls" or "xlsx" or "ppt" or "pptx" => "docs",
                _ => "files"
            };

        private static string SanitizeFolder(string folder)
        {
            var invalid = Path.GetInvalidFileNameChars().ToHashSet();

            var segments = folder
                .Replace("\\", "/")
                .Trim()
                .Trim('/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Where(seg => seg != "." && seg != "..")
                .Select(seg => new string(seg.Where(ch => !invalid.Contains(ch)).ToArray()))
                .Where(seg => !string.IsNullOrWhiteSpace(seg));

            return string.Join('/', segments);
        }

        #endregion
    }
}