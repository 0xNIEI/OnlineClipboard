using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OnlineClipboard.Data;
using OnlineClipboard.Models;

namespace OnlineClipboard.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EntriesController(ApplicationDbContext context, IConfiguration configuration, ILogger<EntriesController> logger)
        {
            _context = context;
            _random = new Random();
            _configuration = configuration;
            _logger = logger;
        }

        // GET: Entries
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Create));
        }

        // GET: Entries/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            var entryDbModel = new Entry{};
            var realInt = 0;

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ContainsLetterAndNotJustNumbers(id))
            {
                entryDbModel = await _context.Entry.FirstOrDefaultAsync(x => x.custom_id == id.ToLower());
            }

            else if (int.TryParse(id, out realInt))
            {
                entryDbModel = await _context.Entry.FirstOrDefaultAsync(x => x.id == realInt);
            }

            if (entryDbModel?.id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (entryDbModel.expires_at < DateTime.UtcNow)
            {
                _context.Remove(entryDbModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (entryDbModel.onlyViewableOnce)
            {
                _context.Remove(entryDbModel);
                await _context.SaveChangesAsync();
            }

            var ipAddr = GetRequestIp(Request);

            _logger.LogInformation($"{ipAddr} viewed {entryDbModel.id}");

            return View(new EntryViewModel(entryDbModel));
        }

        // GET: Entries/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntryViewModel entryViewModel)
        {
            if (string.IsNullOrWhiteSpace(entryViewModel.content) || entryViewModel.content.Length >= 100000)
            {
                entryViewModel.validationError = "Content is empty or too large";
                return View(entryViewModel);
            }

            var enteredPasswordHash = SHA512(entryViewModel.broToken ?? string.Empty);
            var realPasswordHash = _configuration.GetValue<string>("Hashbrown");

            if (string.IsNullOrWhiteSpace(entryViewModel.broToken) || enteredPasswordHash != realPasswordHash)
            {
                entryViewModel.broToken = string.Empty;
                entryViewModel.validationError = "BroToken™ invalid";
                return View(entryViewModel);
            }

            var entryDbModel = new Entry { };
            var id = 0;
            var idIsDuplicate = true;
            do
            {
                id = _random.Next(0, 9999);
                if (!await _context.Entry.AnyAsync(x => x.id == id))
                {
                    idIsDuplicate = false;
                }

            } while (idIsDuplicate);

            entryDbModel.id = id;
            entryDbModel.content = Base64Encode(entryViewModel.content);
            entryDbModel.created_at = DateTime.UtcNow;
            entryDbModel.onlyViewableOnce = entryViewModel.onlyViewableOnce;
            if (entryDbModel.onlyViewableOnce)
            {
                entryDbModel.expires_at = DateTime.MaxValue;
            }
            else
            {
                entryDbModel.expires_at = DateTime.UtcNow.AddHours(entryViewModel.expiresIn >= 720.00 ? 720.00 : entryViewModel.expiresIn);
            }

            if (entryViewModel.custom_id != null)
            {
                entryViewModel.custom_id = entryViewModel.custom_id.ToLower();

                var pattern = @"^[a-z0-9]{1,16}$";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                var customIdIsValid = ContainsLetterAndNotJustNumbers(entryViewModel.custom_id) && regex.IsMatch(entryViewModel.custom_id);
                var customIdAlreadyExists = await _context.Entry.AnyAsync(x => x.custom_id == entryViewModel.custom_id);
                var customIdLengthIsValid = entryViewModel.custom_id.Length >= 1 && entryViewModel.custom_id.Length <= 16;
                var customIdContainsBadStuff = entryViewModel.custom_id.Contains('/') || entryViewModel.custom_id.Contains("home") || entryViewModel.custom_id.Contains("entries") || entryViewModel.custom_id.Contains("changetheme");

                if (!customIdIsValid || customIdAlreadyExists || !customIdLengthIsValid || customIdContainsBadStuff)
                {
                    entryViewModel.validationError = "Custom ID invalid or already in use";
                    return View(entryViewModel);
                }
                entryDbModel.custom_id = entryViewModel.custom_id;
            }

            var ipAddr = GetRequestIp(Request);

            _logger.LogInformation($"{ipAddr} created {entryDbModel.id}");

            _context.Add(entryDbModel);
            await _context.SaveChangesAsync();

            if (entryViewModel.onlyViewableOnce)
            {
                entryViewModel.id = entryDbModel.id;
                return View("Details", entryViewModel);
            }

            return RedirectToAction(nameof(Details), new { id = id.ToString() });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _context.Entry.Where(x => x.id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            var ipAddr = GetRequestIp(Request);

            _logger.LogInformation($"{ipAddr} deleted {id}");

            return RedirectToAction(nameof(Index));
        }

        //private bool EntryExists(int id)
        //{
        //  return (_context.Entry?.Any(e => e.id == id)).GetValueOrDefault();
        //}

        private static bool ContainsLetterAndNotJustNumbers(string input)
        {
            // Use a regular expression to check if the input contains at least one letter
            // and does not consist only of numbers.
            Regex regex = new Regex(@"[a-zA-Z]");
            return regex.IsMatch(input) && !Regex.IsMatch(input, @"^\d+$");
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private string GetRequestIp(HttpRequest request)
        {
            //Derived from https://developers.cloudflare.com/support/troubleshooting/restoring-visitor-ips/restoring-original-visitor-ips/
            const string HeaderKeyName = "CF-Connecting-IP";
            request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            return headerValue.ToString() == "" ? "Development" : headerValue.ToString();
        }

        private string GetRequestCountry(HttpRequest request)
        {
            const string HeaderKeyName = "CF-IPCountry";
            request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            return headerValue.ToString() == "" ? "Development" : headerValue.ToString();
        }
    }
}
