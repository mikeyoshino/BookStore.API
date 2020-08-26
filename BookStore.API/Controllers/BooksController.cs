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
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _loggerSerivce;
        private readonly IMapper _mapper;
        public BooksController(IBookRepository bookRepository, ILoggerService loggerService, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _loggerSerivce = loggerService;
            _mapper = mapper;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _loggerSerivce.LogInfor("Attepmted to find all books.");
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                _loggerSerivce.LogInfor("Sucessfully get all books.");

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }


        }

        /// <summary>
        /// Get book based on ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _loggerSerivce.LogInfor($"{location} Attepmted to get book with id:{id}.");
                var book = await _bookRepository.FindById(id);
                if (book == null)
                {
                    _loggerSerivce.LogWarn($"{location} book with id:{id} was not found.");
                    return NotFound();

                }
                var response = _mapper.Map<BookDTO>(book);
                _loggerSerivce.LogInfor($"{location} Sucessfully get author with id:{id}.");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Create a new book.
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _loggerSerivce.LogInfor($"{location} Create Attempted.");
                if (bookDTO == null)
                {
                    _loggerSerivce.LogInfor($"{location} Empty request was submitted.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _loggerSerivce.LogInfor($"{location} Book's data was not completed.");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location} failed to created.");

                }
                _loggerSerivce.LogInfor($"{location} Book is created!");
                _loggerSerivce.LogInfor($"{location} {book}");
                return Created("Create", new { book });

            }
            catch (Exception e)
            {
                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Update book based on Book Id
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administration")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] BookUpdateDTO bookDTO, int id)
        {
            var location = GetControllerActionNames();
            try
            {
                if(id < 1 || bookDTO == null || bookDTO.Id != id)
                {
                    _loggerSerivce.LogWarn($"{location} Fail Update Bad Data request.");
                    return BadRequest();
                }
                var isExists = await _bookRepository.isExists(id);
                if (!isExists)
                {
                    _loggerSerivce.LogWarn($"{location} Book with id:{id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _loggerSerivce.LogWarn($"{location} Book Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Update(book);

                if (!isSuccess)
                {
                    InternalError($"{location} Update fail.");
                }
                _loggerSerivce.LogWarn($"{location} Book with id: {id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Delete book based on book id.
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
            var location = GetControllerActionNames();
            try
            {
                _loggerSerivce.LogInfor($"{location} Attepmted to delete an book id with {id}");
                if (id < 1)
                {
                    _loggerSerivce.LogWarn($"{location} delete failed with bad data.");
                    return BadRequest();
                }
                var isExist = await _bookRepository.isExists(id);
                if (!isExist)
                {
                    _loggerSerivce.LogWarn($"{location} book with id {id} was not found.");
                    return NotFound();

                }

                var book = await _bookRepository.FindById(id);
                var isSuccess = await _bookRepository.Delete(book);

                if (!isSuccess)
                {
                    return InternalError($"{location} Book delete failed");
                }
                _loggerSerivce.LogWarn($"{location} Book with id {id} is successfully deleted");
                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{location}{e.Message} - {e.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;

            return $"{controllerName} - {actionName}";
        }

        private ObjectResult InternalError(string message)
        {
            _loggerSerivce.LogError(message);
            return StatusCode(500, "Somethings went wrong, contract admins asap.");
        }
    }
}
