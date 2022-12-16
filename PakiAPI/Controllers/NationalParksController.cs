using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakiAPI.Models;
using PakiAPI.Models.Dtos;
using PakiAPI.Repository.IRepository;
using System.Collections.Generic;

namespace PakiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }


            return Ok(objList);
        }

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
         {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
             if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Already Exist!");
                return StatusCode(404, ModelState);
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId!=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record{nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId)) 
            {
                return NotFound();
            }

            var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record{nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
