using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.Data;
using BookStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    /// <summary>
    /// Endpoint use to interact with author in book's store database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _loggerSerivce;
        private readonly IMapper _mapper;
        public AuthorsController(IAuthorRepository authorRepository, ILoggerService loggerService, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _loggerSerivce = loggerService;
            _mapper = mapper;

        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>List of authors.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _loggerSerivce.LogInfor("Attepmted to get all authors.");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _loggerSerivce.LogInfor("Sucessfully get all authors.");
                return Ok(authors);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Get author with thier Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a author based on their Id.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            
            try
            {
                _loggerSerivce.LogInfor($"Attepmted to get author with id:{id}.");
                var author = await _authorRepository.FindById(id);
                if(author == null)
                {
                    _loggerSerivce.LogWarn($"author with id:{id} was not found.");
                    return NotFound();

                }
                var response = _mapper.Map<AuthorDTO>(author);
                _loggerSerivce.LogInfor($"Sucessfully get author with id:{id}.");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
            
        }
        /// <summary>
        /// Create new authors.
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns>new authors.</returns>
        [HttpPost]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create ([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                if(authorDTO == null)
                {
                    _loggerSerivce.LogInfor("Empty request was submitted.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _loggerSerivce.LogInfor("Author's data was not completed.");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(author);
                if (!isSuccess)
                {
                    return InternalError($"Authors failed to created.");

                }
                _loggerSerivce.LogInfor("Author is created!");
                return Created("Create", new { author });

            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Update author based on their id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            {
                _loggerSerivce.LogInfor("Author's update attepmted.");
                if(id < 1|| authorDTO == null || id != authorDTO.Id)
                {
                    _loggerSerivce.LogWarn("update fail because of bad data.");
                    return BadRequest();
                }
                var isExists = await _authorRepository.isExists(id);
                if (!isExists)
                {
                    _loggerSerivce.LogWarn($"Author with id:{id} was not found");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _loggerSerivce.LogWarn($"Author Data was Incomplete");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Update(author);
                if (!isSuccess)
                {
                    InternalError("Update fail.");
                }
                _loggerSerivce.LogWarn($"Author with id: {id} successfully updated");
                return NoContent();

            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Delete Author based on id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggerSerivce.LogInfor($"Attepmted to delete an author with {id}");
                if(id < 1)
                {
                    _loggerSerivce.LogWarn("Author delete failed with bad data.");
                    return BadRequest();
                }
                var isExist = await _authorRepository.isExists(id);
                if (!isExist)
                {
                    _loggerSerivce.LogWarn($"Author with id {id} was not found.");
                    return NotFound();

                }

                var author = await _authorRepository.FindById(id);
                var isSuccess = await _authorRepository.Delete(author);

                if (!isSuccess)
                {
                    return InternalError($"Author delete failed");
                }
                _loggerSerivce.LogWarn($"Author with id {id} is successfully deleted");
                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }




        private ObjectResult InternalError(string message)
        {
            _loggerSerivce.LogError(message);
            return StatusCode(500, "Somethings went wrong, contract admins asap.");
        }
        
    }
}
