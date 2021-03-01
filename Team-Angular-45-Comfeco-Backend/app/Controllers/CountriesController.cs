using AutoMapper;

using BackendComfeco.DTOs.Country;
using BackendComfeco.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ExtendedBaseController
    {
        public CountriesController(IMapper mapper,
            ApplicationDbContext applicationDbContext)
            : base(applicationDbContext, mapper)
        {

        }


        [HttpGet]
        public async Task<ActionResult<List<CountryDTO>>> Get()
        {
            return await Get<Country, CountryDTO>();
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
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
