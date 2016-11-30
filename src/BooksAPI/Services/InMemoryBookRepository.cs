using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksAPI.Models;

namespace BooksAPI.Services
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly BooksAPIContext _context;

        public InMemoryBookRepository(BooksAPIContext context)
        {
            _context = context;
        }

        public Book Add(Book book)
        {
            var addedBook = _context.Add(book);
            _context.SaveChanges();
            book.Id = addedBook.Entity.Id;

            return book;
        }

        public void Delete(Book book)
        {
            _context.Remove(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public Book GetById(int id)
        {
            return _context.Books.SingleOrDefault(x => x.Id == id);
        }

        public void Update(Book book)
        {
            var bookToUpdate = GetById(book.Id);
            bookToUpdate.Author = book.Author;
            bookToUpdate.Title = book.Title;
            bookToUpdate.PublishedDate = book.PublishedDate;
            _context.Update(bookToUpdate);
            _context.SaveChanges();
        }
    }
}
