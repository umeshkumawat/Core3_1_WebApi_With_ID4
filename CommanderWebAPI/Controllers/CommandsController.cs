using AutoMapper;
using CommanderWebAPI.Data;
using CommanderWebAPI.Dtos;
using CommanderWebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var allCommands = _repo.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(allCommands));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var result = _repo.GetCommandById(id);
                
            if (result != null)
                return Ok(_mapper.Map<CommandReadDto>(result));
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand([FromBody]CommandCreateDto cmd)
        {
            var commandModel = _mapper.Map<Command>(cmd);
            _repo.CreateCommand(commandModel);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = Request.Scheme;
            uriBuilder.Host = Request.Host.Host;
            uriBuilder.Path = Request.Path;

            return CreatedAtRoute(nameof(GetCommandById), new { id = commandModel.Id }, commandReadDto);
            //return Created(uriBuilder.Uri + $"/{commandModel.Id}", commandReadDto); // Ok(commandModel);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, [FromBody] CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repo.GetCommandById(id);

            if (commandModelFromRepo == null)
                return NotFound();

            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            _repo.UpdateCommand(commandModelFromRepo);

            _repo.SaveChanges();

            return NoContent();
        }


        /*
         patch request body contents

        [
            {
                "op": "replace",
                "path": "/howto",
                "value": "some new value"
            }
        ]
         */

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repo.GetCommandById(id);

            if (commandModelFromRepo == null)
                return NotFound();


            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repo.UpdateCommand(commandModelFromRepo);

            _repo.SaveChanges();

            return NoContent();
        }
    }
}
