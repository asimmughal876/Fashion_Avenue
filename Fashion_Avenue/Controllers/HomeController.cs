using Fashion_Avenue.Data;
using Fashion_Avenue.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fashion_Avenue.Controllers
{
    public class HomeController : Controller
    {
        private readonly FashionAvenueContext db;

        public HomeController(FashionAvenueContext db)
        {
            this.db = db;
        }

        [HttpGet]
       
        public IActionResult Index()
        {
            try
            {
                var model = new IndexModel
                {
                    Blog = db.Blogs.ToList(),
                    Product = db.Products.ToList(),
                    ProductLike = db.ProductLikes.Include(u=> u.PlUserNavigation).ToList(),
                };
                return View(model);

            }
            catch (Exception ex)
            {
                TempData["p_mess"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Product(string sorting, int id,int catgid,string search)
        {
            try
            {
               
                var productsQuery = db.Products.AsQueryable();

                if (sorting == "low")
                {
                    productsQuery = productsQuery.OrderBy(p => p.PPrice);
                }
                else if (sorting == "high")
                {
                    productsQuery = productsQuery.OrderByDescending(p => p.PPrice);
                }
                else if (sorting == "popular")
                {
                    productsQuery = productsQuery.OrderByDescending(p => p.LikeCounts.Sum(lc => lc.LcCount));
                }
                else if (sorting == "default")
                {
                    productsQuery = productsQuery.OrderBy(p=> p.PId);
                }
                if (id > 0)
                {
                    productsQuery = productsQuery.Where(p => p.PColor == id);
                }
                if(catgid >0)
                {
                    productsQuery = productsQuery.Where(p => p.PCatgId == catgid);
                }
                if (search != null)
                {
                    productsQuery = productsQuery.Where(p => p.PName.Contains(search));
                }
                var model = new IndexModel
                {
                    Category = db.Categories.ToList(),
                    ProductColor = db.ProductColors.ToList(),
                    Product = productsQuery.ToList(),
                    Product_Count = productsQuery.Count(),
                    ProductLike = db.ProductLikes.Include(u => u.PlUserNavigation).ToList()
                };
              
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["p_mess"] = ex.Message;
            }
            return View();
        }

        public IActionResult ProductLike(ProductLike pl,int id, LikeCount Count)
        {
            try
            {
                var userid = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userid != null)
                {
                    var nameIdentifierClaim = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    var data = db.ProductLikes.FirstOrDefault(x => x.PlUser == nameIdentifierClaim && x.PlProdId == id);
                    var Like = db.LikeCounts.FirstOrDefault(x => x.LcProdId == id);
                    if (data != null)
                    {
                        if (Like != null)
                        {
                            if (Like.LcCount > 0)
                            {
                                Like.LcCount--;
                                db.Update(Like);
                                db.SaveChanges();
                            }
                        }
                        db.Remove(data);
                        db.SaveChanges();
                        TempData["Prod_Like"] = "Product Like Removed";

                    }
                    else
                    {

                        if (Like != null)
                        {
                            Like.LcCount++;
                            db.Update(Like);
                            db.SaveChanges();
                        }
                        else
                        {
                            Count.LcCount = 1;
                            Count.LcProdId = id;
                            db.Add(Count);
                            db.SaveChanges();
                        }
                        pl.PlProdId = id;
                        pl.PlUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        pl.PlCount = 1;
                        db.Add(pl);
                        db.SaveChanges();
                        TempData["Prod_Like"] = "Product has Been Like";
                    }
                   
                }
                else
                {
                    TempData["Prod_Like"] = "Login Is Required For Likes";
                }

            }
            catch(Exception ex)
            {
                TempData["Prod_Like"] = ex.Message;
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Cart()
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim != null)
            {
                ViewBag.mycart = HttpContext.Session.GetObject<List<Cart>>("Sess_Name");

                if (TempData.ContainsKey("Total"))
                {
                    TempData.Keep("Total");
                }
                return View();
            }
            else {
                TempData["Reuire_Login"] = "Login Is Required For Cart Page";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult AddtoCart(int? id)
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim != null)
            {
                List<Cart> cartItems = HttpContext.Session.GetObject<List<Cart>>("Sess_Name") ?? new List<Cart>();
                Cart exisitng_item = cartItems.Find(item => item.ProdId == id);
                if (exisitng_item != null)
                {
                    exisitng_item.Quantity += 1;
                }
                else
                {
                    var mydata = db.Products.Find(id);
                    cartItems.Add(
                    new Cart
                    {
                        ProdId = mydata.PId,
                        ProdName = mydata.PName,
                        ProdImage = mydata.PImage,
                        Quantity = 1,
                        Price = (int)mydata.PPrice
                    }
                    );
                }
                HttpContext.Session.SetObject<List<Cart>>("Sess_Name", cartItems);
                ViewBag.mycart = HttpContext.Session.GetObject<List<Cart>>("Sess_Name");
                return RedirectToAction(nameof(Cart));
            }
            else
            {
                TempData["Cart"] = "Login Is Require For Add To Cart";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult minus(int Id)
        {
            List<Cart> cartItems = HttpContext.Session.GetObject<List<Cart>>("Sess_Name") ?? new List<Cart>();
            Cart exisitng_item = cartItems.Find(item => item.ProdId == Id);
            if (exisitng_item != null)
            {
                if (exisitng_item.Quantity > 1)
                {
                    exisitng_item.Quantity -= 1;
                }
                else if (exisitng_item.Quantity == 1)
                {
                    cartItems.Remove(exisitng_item);

                }
            }
            HttpContext.Session.SetObject<List<Cart>>("Sess_Name", cartItems);
            ViewBag.mycart = HttpContext.Session.GetObject<List<Cart>>("Sess_Name");
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult checkout(IFormCollection f,decimal total)
        {
            List<Cart> cartItems = HttpContext.Session.GetObject<List<Cart>>("Sess_Name") ?? new List<Cart>();

            Order o = new Order();
            o.TotalPrice = total;
            o.OrderStatus = "Pending";
            o.OrderUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            db.Add(o);
            db.SaveChanges();

            foreach (Cart itms in cartItems)
            {
                OrderItem itemsTable = new OrderItem();
                itemsTable.OrderItemsProdId = itms.ProdId;
                itemsTable.OrderItemsQuantity = itms.Quantity;
                itemsTable.OrderItemsOrderId = o.OrderId;
                db.Add(itemsTable);
                db.SaveChanges();
            }
            TempData.Remove("Total");
            HttpContext.Session.SetObject("Sess_Name", "");
            TempData["Cart"] = "Transaction has been Complete";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Coupon(string coupon_code,UsedCoupon Uc)
        {
            var coupon = db.Coupons.FirstOrDefault(c=> c.CName == coupon_code);
            if (coupon != null)
            {
                var user_Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
                var u_coupon = db.UsedCoupons.FirstOrDefault(u => u.UcUserId == user_Id);
                if (u_coupon == null)
                {
                    Uc.UcCouponId = coupon.CId;
                    Uc.UcUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    TempData["Total"] = coupon.CDiscount.ToString();
                    db.Add(Uc);
                    db.SaveChanges();
                    TempData["Coupon"] = "Coupon Code has been Used";
                }
                else
                {
                    TempData["Coupon"] = "Coupon Code Already Used";
                }
                
            }
            else
            {
                TempData["Coupon"] = "Coupon Code not Found";
            }

            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserRecord User)
        {
            TempData.Remove("Message");
            var data = db.UserRecords.FirstOrDefault(x => x.UEmail == User.UEmail && x.UPass == User.UPass);
            ClaimsIdentity identity = null;
            bool isAuthenticate = false;
            if (data != null)
            {

                if (data.URoleId == 101)
                {
                    identity = new ClaimsIdentity(new[]
                    {
               new Claim(ClaimTypes.Name, data.UName),
               new Claim(ClaimTypes.Email, data.UEmail),
                new Claim(ClaimTypes.NameIdentifier, data.UId.ToString()),
               new Claim(ClaimTypes.Role,"Admin")
           }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }
                else
                {
                    identity = new ClaimsIdentity(new[]
                 {
              new Claim(ClaimTypes.Name, data.UName),
               new Claim(ClaimTypes.Email, data.UEmail),
                new Claim(ClaimTypes.NameIdentifier, data.UId.ToString()),
               new Claim(ClaimTypes.Role,"User")

           }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }
                if (isAuthenticate)
                {
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (data.URoleId == 101)
                    {
                        return RedirectToAction("IndexAdmin", "Admin");
                    }
                    else
                    {
                        TempData["Login"] = "Login Successfully";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            TempData.Remove("Total");
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.SetObject("Sess_Name", "");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UserRecord user)
        {
            try {
                var data = db.UserRecords.FirstOrDefault(x => x.UEmail == user.UEmail);
                if (data == null)
                {
                    user.URoleId = 102;
                    db.Add(user);
                    db.SaveChanges();
                    TempData["Message"] = "User Registered Successfully..";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["Message"] = "This Email Address Already has a User";
                }
       
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View();
        }
        public IActionResult About()
        {
            try
            {
                var about = db.Abouts.ToList();
                return View(about);
            }
            catch(Exception ex) {
                TempData["about_error"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            db.Add(contact);
            db.SaveChanges();
            TempData["cont_mess"] = "Your Message has been Recieved";
            return View();
        }

        public IActionResult Blog()
        {
            try
            {
                var model = new IndexModel
                {
                    Blog = db.Blogs.ToList(),
                    BlogComment = db.BlogComments.Include(b=> b.Blog).ToList(),
                };
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["Blog_Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Blog_Detail(int id)
        {
            try
            {
                var model = new IndexModel
                {
                    BlogDetail = db.Blogs.FirstOrDefault(b => b.BId == id),
                    BlogComment = db.BlogComments.Include(b=> b.Blo).Where(b=> b.BlogId == id).ToList(),
                };
                
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Blog_error"] = ex.Message;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Blog_Detail(int id,BlogComment b_comm,string comment)
        {
            try
            {
                var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (nameIdentifierClaim != null)
                {
                    b_comm.BlogId = id;
                    b_comm.BlogUId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    b_comm.BlogCName = comment;
                    db.Add(b_comm);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Blog_C_error"] = "Login is Required for Comments";
                }
            }
            catch (Exception ex)
            {
                TempData["Blog_C_error"] = ex.Message;
            }
            return RedirectToAction(nameof(Blog_Detail));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}