using AutoMapper;

using BackendComfeco.DTOs.Country;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/countries")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = ApplicationConstants.Roles.AdminRoleName)]
    public class CountriesController : ExtendedBaseController
    {
        public CountriesController(IMapper mapper,
            ApplicationDbContext applicationDbContext)
            : base(applicationDbContext, mapper)
        {

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<CountryDTO>>> Get()
        {
            return await Get<Country, CountryDTO>();
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [AllowAnonymous]
        public async Task<ActionResult<CountryDTO>> Get(int id)
        {

            return await Get<Country, CountryDTO>(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post(CountryCreationDTO countryCreationDTO)
        {

            return await Post<CountryCreationDTO, Country, CountryDTO>(countryCreationDTO, "GetCountry");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, CountryCreationDTO countryCreationDTO)
        {
            return await Put<CountryCreationDTO, Country>(id, countryCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Country>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CountryCreationDTO> jsonPatchDocument)
        {
            return await Patch<Country, CountryCreationDTO>(id, jsonPatchDocument);
        }

    }
}
