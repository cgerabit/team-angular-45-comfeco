using AutoMapper;

using BackendComfeco.DTOs.Comunity;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/communities")]
    public class CommunitiesController : ExtendedBaseController
    {
        public CommunitiesController(ApplicationDbContext applicationDbContext,
            IMapper mapper)
            : base(applicationDbContext, mapper)
        {

        }

        [HttpGet]
        public async Task<ActionResult<List<ComunityDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Comunity, ComunityDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetComunity")]
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
