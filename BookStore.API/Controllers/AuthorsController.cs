using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.DTOs;
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
                return internalError($"{e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Get author with thier Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a author based on their Id.</returns>
        [HttpGet("{id}")]
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
                return internalError($"{e.Message} - {e.InnerException}");
            }
            
        }
        private ObjectResult internalError(string message)
        {
            _loggerSerivce.LogError(message);
            return StatusCode(500, "Somethings went wrong, contract admins asap.");
        }
        
    }
}
