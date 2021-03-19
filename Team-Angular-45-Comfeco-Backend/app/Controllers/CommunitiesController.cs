using AutoMapper;

using BackendComfeco.DTOs.Comunity;
using BackendComfeco.DTOs.Shared;
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
    [Route("api/communities")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = ApplicationConstants.Roles.AdminRoleName)]
    public class CommunitiesController : ExtendedBaseController
    {
        public CommunitiesController(ApplicationDbContext applicationDbContext,
            IMapper mapper)
            : base(applicationDbContext, mapper)
        {

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ComunityDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Comunity, ComunityDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetComunity")]
        [AllowAnonymous]
        public async Task<ActionResult<ComunityDTO>> Get(int id)
        {
            return await Get<Comunity, ComunityDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ComunityCreationDTO comunityCreationDTO)
        {
            return await Post<ComunityCreationDTO, Comunity, ComunityDTO>(comunityCreationDTO, "GetComunity");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ComunityCreationDTO comunityCreationDTO)
        {
            return await Put<ComunityCreationDTO, Comunity>(id, comunityCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Comunity>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id,
            [FromBody]JsonPatchDocument<ComunityCreationDTO> patchDocument)
        {
            return await Patch<Comunity, ComunityCreationDTO>(id,patchDocument);
        }


    }
}
