using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BanSachWeb.ViewModels;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
namespace BanSachWeb.Controllers
{
    public class SachController : Controller
    {
        // GET: Sach
        QuanLyBanSachModel db=new QuanLyBanSachModel();
        
        public ActionResult Index(int? page, decimal? minPrice, decimal? maxPrice)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var books = db.Saches.AsQueryable();

            if (minPrice.HasValue)
            {
                books = books.Where(b => b.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                books = books.Where(b => b.GiaBan <= maxPrice.Value);
            }

            var model = books.OrderBy(s => s.MaSach).Select(b => new BookViewModel
            {
                MaSach = b.MaSach,
                TenSach = b.TenSach,
                AnhSach = b.AnhSach,
                GiaBan = b.GiaBan,
                GiaGoc = b.GiaGoc,
                SoLuongDaBan = b.SoLuongDaBan,
                
            }).ToPagedList(pageNumber, pageSize);

            ViewBag.nameAction = "Index"; // Set the action name for pagination
            return View(model);
        }
        // GET: Book/GetCategories
        public ActionResult GetCategories()
        {
            var categories = db.DanhMucChinhs
                       .Include("DanhMucPhus")
                       .Where(c => c.DanhMucPhus.Any(s => s.Visible == true))
                       .ToList();

            // Alternatively, you might want to load only visible subcategories
            foreach (var category in categories)
            {
                category.DanhMucPhus = category.DanhMucPhus.Where(s => s.Visible == true).ToList();
            }

            
            return PartialView("GetCategories", categories);
        }
        public ActionResult ViewCategory(int id, int? page, decimal? minPrice, decimal? maxPrice)
        {
            var subCategory = db.DanhMucPhus.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Sách theo danh mục - " + subCategory.TenDanhMuc;
            ViewBag.nameAction = "ViewCategory";
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;

            var products = db.Saches.Where(s => s.MaDanhMuc == id);

            if (minPrice.HasValue)
            {
                products = products.Where(s => s.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(s => s.GiaBan <= maxPrice.Value);
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var productViewModels = products.OrderBy(s => s.MaSach).Where(s => s.Visible == true).Select(s => new BookViewModel
            {
                MaSach = s.MaSach,
                TenSach = s.TenSach,
                SoLuongDaBan = s.SoLuongDaBan,
                AnhSach = s.AnhSach,
                GiaBan = s.GiaBan,
                GiaGoc = s.GiaGoc
            }).ToPagedList(pageNumber, pageSize);

            return View("Index", productViewModels);
        }



        public IQueryable<BookViewModel> GetBookQuery()
        {
            return db.Saches.AsNoTracking()
                .Select(s => new BookViewModel
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    SoLuongDaBan = s.SoLuongDaBan,
                    AnhSach = s.AnhSach,
                    GiaBan = s.GiaBan,
                    GiaGoc = s.GiaGoc
                });
        }
        public IQueryable<BookViewModel> GetTopBookQuery(int amount = 24)
        {

            return db.Saches.Where(s => s.Visible == true)
                .Select(s => new BookViewModel
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    SoLuongDaBan = s.SoLuongDaBan,
                    AnhSach = s.AnhSach,
                    GiaBan = s.GiaBan,
                    GiaGoc = s.GiaGoc
                }).OrderByDescending(s => s.SoLuongDaBan).Take(amount);
        }
        


        public ActionResult SachDetail(int id)
        {
            var book = db.Saches.FirstOrDefault(s => s.MaSach == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            var relevantBooks = db.Saches
                .Where(s => s.MaDanhMuc == book.MaDanhMuc && s.MaSach != id&&s.Visible==true)
                .Take(4) // limit to 5 relevant books
                .ToList();

            var model = new BookDetailsViewModel
            {
                Book = book,
                RelevantBooks = relevantBooks
            };

            return View(model);
        }
        public ActionResult TopSale(int? page, decimal? minPrice, decimal? maxPrice)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            var topBooks = GetTopBookQuery().AsQueryable();
            if (minPrice.HasValue)
            {
                topBooks = topBooks.Where(s => s.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                topBooks = topBooks.Where(s => s.GiaBan <= maxPrice.Value);
            }
            ViewBag.Title = "Sách bán chạy";
            ViewBag.nameAction = "TopSale";
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
           var topBookspagelist=topBooks.ToPagedList(pageNumber, pageSize);
            return View("Index", topBookspagelist);
        }
        public PartialViewResult TopSalePartial()
        {
            var topSaleBooks = GetTopBookQuery(4);
            return PartialView("_TopSale", topSaleBooks);
        }

        public IQueryable<BookViewModel> GetSearchedBookQuery(List<Sach> sachs)
        {
            return sachs.Select(s => new BookViewModel
            {
                MaSach = s.MaSach,
                TenSach = s.TenSach,
                SoLuongDaBan = s.SoLuongDaBan,
                AnhSach = s.AnhSach,
                GiaBan = s.GiaBan,
                GiaGoc = s.GiaGoc
            }).AsQueryable();
        }
        //Xử lý phần tìm kiếm
        // API to get search suggestions
        public JsonResult GetSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<string>(), JsonRequestBehavior.AllowGet);
            }

            term = RemoveVietnameseDiacritics(term.ToLower());
            var saches = db.Saches.ToList();
            var suggestions = saches
                .Where(s => RemoveVietnameseDiacritics(s.TenSach.ToLower()).Contains(term))
                .Select(s => s.TenSach)
                .Distinct()
                .Take(10)
                .ToList();

            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }
        private string RemoveVietnameseDiacritics(string text)
        {
            string[] VietnameseChars = new string[]
            {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietnameseChars.Length; i++)
            {
                for (int j = 0; j < VietnameseChars[i].Length; j++)
                    text = text.Replace(VietnameseChars[i][j], VietnameseChars[0][i - 1]);
            }
            return text;
        }

        public ActionResult Search(SearchViewModel model, int? page, decimal? minPrice, decimal? maxPrice)
        {
            
            if (model == null)
            {
                System.Diagnostics.Debug.WriteLine("Model is null.");
                return RedirectToAction("Index");

            }
            var searchKey = RemoveVietnameseDiacritics(model.SearchKey?.ToLower() ?? "");
            if (searchKey.IsNullOrWhiteSpace())
            {
                System.Diagnostics.Debug.WriteLine("searchkey is empty.");
            }// Null conditional operator and null coalescing operator are used here
            var saches = db.Saches.ToList();
            var tacGias = db.TacGias.ToList();

            var sachViewModelList = (from s in saches
                                     join t in tacGias on s.MaTacGia equals t.MaTacGia into authorGroup
                                     from t in authorGroup.DefaultIfEmpty()
                                     let tenTacGia = t != null ? t.TenTacGia : ""
                                     let tenSach = s.TenSach
                                     where RemoveVietnameseDiacritics((tenTacGia ?? "").ToLower()).Contains(searchKey) ||
                                           RemoveVietnameseDiacritics((tenSach ?? "").ToLower()).Contains(searchKey)
                                     select new BookViewModel
                                     {
                                         MaSach = s.MaSach,
                                         TenSach = s.TenSach,
                                         SoLuongDaBan = s.SoLuongDaBan,
                                         AnhSach = s.AnhSach,
                                         GiaBan = s.GiaBan,
                                         GiaGoc = s.GiaGoc,
                                         TenTacGia = t != null ? t.TenTacGia : "Unknown"
                                     }).AsQueryable();

            if (minPrice.HasValue)
            {
                sachViewModelList = sachViewModelList.Where(s => s.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                sachViewModelList = sachViewModelList.Where(s => s.GiaBan <= maxPrice.Value);
            }

            ViewBag.Title = "Kết quả tìm kiếm";
            ViewBag.nameAction = "Search";
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.SearchKey = model.SearchKey; // Pass the SearchKey to the view

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var bookPagedList = sachViewModelList.ToPagedList(pageNumber, pageSize);

            return View("Index", bookPagedList);
        }

        //hiển thị top sách mới
        public ActionResult TopNewBooksPartial()
        {
            var topNewBooks = GetTopNewBooks(4); // Adjust the number as needed
            return PartialView("_TopNewBooks", topNewBooks);
        }

        public IQueryable<BookViewModel> GetTopNewBooks(int amount = 4)
        {
            return db.Saches.AsNoTracking()
                .OrderByDescending(s => s.MaSach) // Assuming SachID is incremented for newer books
                .Select(s => new BookViewModel
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    SoLuongDaBan = s.SoLuongDaBan,
                    AnhSach = s.AnhSach,
                    GiaBan = s.GiaBan,
                    GiaGoc = s.GiaGoc
                   
                })
                .Take(amount);
        }

        //đánh giá sản phẩm
        [HttpPost]
        public ActionResult RateBook(int bookId, int rating, string comment)
        {
            try
            {
                var maTK = Session["MaTaiKhoan"];
                // Check if the user is authenticated
                if (maTK==null)
                {
                    Session["ReturnUrl"] = Request.UrlReferrer?.ToString(); // Store the return URL
                    // Return a JSON response indicating that the user is not logged in
                    return Json(new { success = false, notLoggedIn = true });
                }

                // Find the book by its ID
                var book = db.Saches.Find(bookId);

                // Check if the book exists
                if (book == null)
                {
                    // Return a JSON response indicating that the book does not exist
                    return Json(new { success = false, message = "Book not found." });
                }

                // Save the rating and comment to the database
                var review = new PhanHoi
                {
                    MaSach = bookId,
                    DiemDanhGia = rating,
                    NoiDung = comment,
                    MaTaiKhoan = (int)Session["MaTaiKhoan"] // Assuming you store the user's ID in the session
                };

                db.PhanHois.Add(review);
                db.SaveChanges();

                // Return a JSON response indicating success
                return Json(new { success = true, message = "Rating and comment saved successfully." });
            }
            catch (Exception ex)
            {
                // Return a JSON response indicating that an error occurred
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        [HttpGet]
        public ActionResult GetBookReviews(int bookId)
        {
            var book = db.Saches.Find(bookId);
            if (book == null)
            {
                return HttpNotFound();
            }

            var reviews = db.PhanHois
                .Where(r => r.MaSach == bookId)
                .Select(r => new ReviewViewModel
                {
                    Username = r.TaiKhoan.TenDangNhap, // Assuming you have a User property in PhanHoi
                    Rating = r.DiemDanhGia?? 0,
                    Comment = r.NoiDung
                })
                .ToList();

            var viewModel = new BookReviewDetailsViewModel
            {
                Book = book,
                Reviews = reviews,
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0
            };

            return View("GetBookReviews", viewModel); // Make sure the view name is correct
        }
        // GET: Sach/GetAverageRating
        public JsonResult GetAverageRating(int bookId)
        {
            // Retrieve all reviews for the given book
            var bookReviews = db.PhanHois.Where(r => r.MaSach == bookId).ToList();

            if (bookReviews.Any())
            {
                // Calculate the average rating
                var averageRating = bookReviews.Average(r => r.DiemDanhGia);
                return Json(new { averageRating, reviewCount = bookReviews.Count }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { averageRating = 0, reviewCount = 0 }, JsonRequestBehavior.AllowGet);
        }

    }
}