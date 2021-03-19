using AutoMapper;

using BackendComfeco.DTOs.Gender;
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
    [Route("api/genders")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme,
        Policy = ApplicationConstants.Roles.AdminRoleName)]
    public class GendersController : ExtendedBaseController
    {

        public GendersController(IMapper mapper,
            ApplicationDbContext applicationDbContext)
            : base(applicationDbContext, mapper)
        {

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            return await Get<Gender, GenderDTO>();
        }

        [HttpGet("{id:int}", Name = "GetGender")]
        [AllowAnonymous]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {

            return await Get<Gender, GenderDTO>(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post(GenderCreationDTO genderCreationDTO)
        {

            return await Post<GenderCreationDTO, Gender, GenderDTO>(genderCreationDTO, "GetGender");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, GenderCreationDTO genderCreationDTO)
        {
            return await Put<GenderCreationDTO, Gender>(id, genderCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Country>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<GenderCreationDTO> jsonPatchDocument)
        {
            return await Patch<Gender, GenderCreationDTO>(id, jsonPatchDocument);
        }
    }
}
