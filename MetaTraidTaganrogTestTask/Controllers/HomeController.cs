using MetaTraidTaganrogTestTask.ApplicationContext;
using MetaTraidTaganrogTestTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MetaTraidTaganrogTestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ClientRepository _clientRepository;
        private BookRepository _bookRepository;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _clientRepository = new ClientRepository();
            _bookRepository = new BookRepository();
         }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetUsers()
        { 
            var clients = _clientRepository.GetAllClients().ToList();
            var result = JsonConvert.SerializeObject(new {data = clients });
            return result;
        }
        [HttpGet]
        public string GetBooks()
        {
            var books = _bookRepository.GetAllBooks().ToList();
            var result = JsonConvert.SerializeObject(new { data = books });
            return result;
        }

        public IActionResult EditBookView(int bookId)
        {
            ViewBag.BookId = bookId;
            return View();
        }
        [HttpPost]
        public IActionResult EditBook(int bookId, string newDescription)
        {
            _bookRepository.EditBookDescription(bookId, newDescription);
            return View("Index");
        }
        public void CreateBook(string name, string description)
        {
            if (name != null && description != null)
            {
                Book book = new Book() { Name = name, Description = description };
                _bookRepository.AddBook(book);
            }
        }
        public void CreateClient(string name, string surname)
        {
            if (name != null && surname != null)
            {
                Client client = new Client() { Name = name, Surname = surname };
                _clientRepository.AddClient(client);
            }
        }
        
        public IActionResult IssueBook(int bookId, string bookName)
        {
            ViewBag.BookId = bookId;
            ViewBag.BookName = bookName;
            return View();
        }
        public IActionResult IssueBookToUser(int bookId, int userId)
        {
            _bookRepository.TakeBook(bookId, userId);
            return View("Index");
        }

        [HttpPost]
        public void ReturnBook(int bookId)
        {
            _bookRepository.ReturnBook(bookId);
        }

        [HttpGet]
        public IActionResult Edit(int Id, int bookId)
        {
            Client client = new Client();
            if (client == null)
            {
                return NotFound(); // Обработка случая, если клиент не найден
            }

            return View(client);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}