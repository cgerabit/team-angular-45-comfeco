using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/socialnetworks")]
    public class SocialNetworksController : ExtendedBaseController
    {
        public SocialNetworksController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<SocialNetworkDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<SocialNetwork, SocialNetworkDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetSocialNetwork")]
        public async Task<ActionResult<SocialNetworkDTO>> Get(int id)
        {

            return await Get<SocialNetwork, SocialNetworkDTO>(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post(SocialNetworkCreationDTO socialNetworkCreationDTO)
        {

            return await Post<SocialNetworkCreationDTO, SocialNetwork, SocialNetworkDTO>(socialNetworkCreationDTO, "GetSocialNetwork");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, SocialNetworkCreationDTO socialNetworkCreationDTO)
        {
            return await Put<SocialNetworkCreationDTO, SocialNetwork>(id, socialNetworkCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<SocialNetwork>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<SocialNetworkCreationDTO> jsonPatchDocument)
        {
            return await Patch<SocialNetwork, SocialNetworkCreationDTO>(id, jsonPatchDocument);
        }

    }
}
