using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using System.Net.Http;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(string id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            string definedcurrency = book.Currency;
            var client = new HttpClient();
            Console.WriteLine("Hello");
            if (definedcurrency != "")
            {
                try
                {
                    Console.WriteLine("inside");
                    var reponsestring = "https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/" + definedcurrency + "/sgd.json";

                    var responseresult = client.GetAsync(reponsestring);
                    responseresult.Wait();

                    var result = responseresult.Result;
                    if (result.IsSuccessStatusCode)
                    {
                      
                        var readAsync = result.Content.ReadAsStringAsync();
                        readAsync.Wait();
                        var finalData = readAsync.Result;
                        Currency datajson = Newtonsoft.Json.JsonConvert.DeserializeObject<Currency>(finalData);
                        Console.WriteLine(datajson.date);
                        var sgdconvert = datajson.sgd;
                        Console.WriteLine(book.Amount);
                        Console.WriteLine(sgdconvert);

                        book.Amount *= (decimal)sgdconvert;
                        Console.WriteLine(book.Amount);
                        book.Currency = "";

                    }
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                
                
            }
           
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);

        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
