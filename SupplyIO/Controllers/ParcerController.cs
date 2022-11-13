using Microsoft.AspNetCore.Mvc;
using SupplyIO.SupplyIO.Services;
using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.ViewModel;
using System.Net;
using System.Text;

namespace SupplyIO.Controllers
{
    [Route("api/parcer")]
    [ApiController]
    public class ParcerController : Controller
    {
        private readonly IMetalService _metalService;
        private readonly ITokenService _tokenService;
        private readonly string _headerName;

        public ParcerController(IMetalService metalService, ITokenService tokenService, IConfiguration configuration)
            => (_metalService, _tokenService, _headerName) = (metalService, tokenService, configuration.GetSection("HeaderName").Value);

        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateFromLinkAsync([FromBody] CertificateLink certificateLink)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificateLink is null)
                    return BadRequest();

                var certificate = await _metalService.CreateFromLinkAsync(certificateLink);

                return certificate;
            }

            return Unauthorized();
        }

        [HttpPost("certificate/check")]
        public async Task<ActionResult<Certificate>> CheckCertificateAsync([FromBody] CertificateLink certificateLink)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificateLink is null)
                    return BadRequest();

                var certificate = await _metalService.CheckSertificateAsync(certificateLink);

                if (certificate is null)
                    return NotFound();

                return certificate;
            }

            return Unauthorized();
        }

        [HttpPost("certificate")]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] Certificate certificate)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificate is null)
                    return NoContent();

                var id = await _metalService.CreateCertificateAsync(certificate);

                return CreatedAtAction(nameof(CreateCertificateAsync), id);
            }
            else
                return Unauthorized();
        }

        [HttpPost("package/{id}")]
        public async Task<ActionResult<int>> AddPackage(int id, [FromBody] Package package)
        {
            package.CertificateId = id;
            var result = await _metalService.AddPackage(package);

            if (result == -1)
                return NoContent();

            return result;
        }

        [HttpGet("certificate")]
        public async Task<ActionResult<List<Certificate>>> GetAllSertificatesAsync()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _metalService.GetAllCertificatesAsync();
            else
                return Unauthorized();
        }

        [HttpGet("certificate/{id}")]
        public async Task<ActionResult<Certificate>> GetSertificateAsync(int id)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var certificate = await _metalService.GetCertificateAsync(id);

                return certificate is null ? NoContent() : certificate;
            }
            else
                return Unauthorized();
        }



        [HttpGet("package/{id}")]
        public async Task<ActionResult<ExtendedPackageViewModel>> GetPackage(int id)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _metalService.GetPackage(id);

            return Unauthorized();
        }

        [HttpPut("package/update")]
        public async Task<ActionResult<int>> UpdatePackageAsync([FromBody] ExtendedPackageViewModel package)
        {
            return await _metalService.UpdatePackageAsync(package);
        }

        [HttpPost("package/add")]
        public async Task<ActionResult<int>> AddPackageAsync([FromBody] ExtendedPackageViewModel package)
        {
            return await _metalService.AddPackageAsync(package);
        }

        [HttpGet("package/supplier")]
        public async Task<ActionResult<string>> GetSupplierByNumberOfCertificate(string number)
        {
            return await _metalService.GetSupplierByNumberOfCertificate(number);
        }

        [HttpGet("package")]
        public async Task<ActionResult<List<PackageViewModel>>> GetAllPackagesAsync(string? status = null)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                if (status is null)
                {
                    return await _metalService.GetAllPackagesAsync();
                }
                else
                    return await _metalService.GetPackagesByStatus(status);

            else
                return Unauthorized();
        }

        [HttpPut("package")]
        public async Task<IActionResult> UpdateStatusPackageAsync(StatusChange statusChange)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                await _metalService.UpdateStatusPackageAsync(statusChange.PackageId, statusChange.Status);

                return Ok();
            }

            return Unauthorized();
        }

        [HttpPut("package/part")]
        public async Task<IActionResult> SetPackageInProcessingForPartAsync(PartOfPackage partOfPackage)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var result = await _metalService.SetPackageInProcessingForPartAsync(partOfPackage.PackageId, partOfPackage.Net, partOfPackage.Width);

                if (result != -1)
                    return Ok();
                else
                    return NoContent();
            }

            return Unauthorized();
        }

        [HttpPut("package/defect")]
        public async Task<ActionResult<int>> AddDefectToPackageAsync(Defect defect)
        {
            return await _metalService.AddDeffectToPackage(defect);
        }

        [HttpGet("certificate/numbers")]
        public async Task<ActionResult<List<string>>> GetNumbersOfCertificates()
        {
            return await _metalService.GetNumbersOfCertificates();
        }

        [HttpGet("package/search")]
        public async Task<ActionResult<List<PackageViewModel>>> SearchAsync(string str, string status)
        {
            return await _metalService.SearchAsync(str, status);
        }
    }
}