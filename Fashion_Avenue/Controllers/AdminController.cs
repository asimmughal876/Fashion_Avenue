using Fashion_Avenue.Data;
using Fashion_Avenue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Colorful;
using System.Security.Claims;
using System.Drawing;

namespace Fashion_Avenue.Controllers
{
    public class AdminController : Controller
    {
        private readonly FashionAvenueContext db;

        public AdminController(FashionAvenueContext db)
        {
            this.db = db;
        }

        public IActionResult IndexAdmin()
        {
            ViewData["User"] = db.UserRecords.Count();
            ViewData["Product"] = db.Products.Count();
            ViewData["Like"] = db.ProductLikes.Count();
            ViewData["Coupon"] = db.Coupons.Count();
            ViewData["UsedCoupon"] = db.UsedCoupons.Count();
            ViewData["Cart"] = db.OrderItems.Count();
            ViewData["Blog"] = db.Blogs.Count();
            ViewData["BlogComment"] = db.BlogComments.Count();
            return View();
        }
 
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public IActionResult About(About about, IFormFile file)
        {
          
                if (file != null && file.Length > 0)
                {
                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileexten = file.ContentType.ToLower();
                    var filestring = fileexten.Substring(6);

                    if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                    {
                        var img = filename + "." + filestring;
                        var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                        using (var stream = new FileStream(imgpath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                    about.AImage = dbpath;
                        TempData["Add_about"] = "About Inserted Successfully";
                        db.Add(about);
                        db.SaveChanges();
                        return RedirectToAction(nameof(Fetch_About));
                    }
                    else
                    {
                        TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                    }
                }
            return View();
        }
        [HttpGet]
        public IActionResult Edit_About(int? id)
        {
            var about = db.Abouts.FirstOrDefault(a=> a.AId == id);
            return View(about);
        }
        [HttpPost]
        public IActionResult Edit_About(About about, IFormFile file, string Pr_img)
        {

            if (file != null && file.Length > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileexten = file.ContentType.ToLower();
                var filestring = fileexten.Substring(6);

                if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                {
                    var img = filename + "." + filestring;
                    var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                    using (var stream = new FileStream(imgpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                    about.AImage = dbpath;

                    db.Update(about);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                    return RedirectToAction(nameof(Edit_About));
                }
            }
            else
            {
                about.AImage = Pr_img;
                db.Update(about);
                db.SaveChanges();
            }
            TempData["U_About"] = "About Update Successfully";

            return RedirectToAction(nameof(Fetch_About));
        }
        [HttpGet]
        public IActionResult Fetch_About()
        {
            try
            {

                var about = db.Abouts.ToList();
                return View(about);

            }
            catch (Exception ex)
            {
                TempData["A_Fetch_Mess"] = ex.Message;
            }
            return View();

        }
        public IActionResult Delete_About(int? id)
        {
            try
            {
                var about = db.Abouts.FirstOrDefault(a => a.AId == id);
                if (about != null)
                {
                    db.Remove(about);
                    db.SaveChanges();
                    TempData["D_about"] = "About Deleted Successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["D_about"] = ex.Message;
            }
            return RedirectToAction(nameof(Fetch_About));
        }

        [HttpGet]
        public IActionResult Fetch_Product(string search)
        {
            try
            {
                List<Product> product;
                if (search != null)
                {
                    product = db.Products.Include(p => p.PCatg).Include(p => p.PColorNavigation).Where(p => p.PName.Contains(search)).ToList();
                }
                else
                {
                    product = db.Products.Include(p => p.PCatg).Include(p => p.PColorNavigation).ToList();
                }
                return View(product);

            }
            catch (Exception ex)
            {
                TempData["P_Fetch Mess"] = ex.Message;
            }
            return View();

        }
        [HttpGet]
        public IActionResult Product()
        {
            ViewBag.ProdCatgId = new SelectList(db.Categories , "CId", "CName");
            ViewBag.ProdColorId = new SelectList(db.ProductColors, "PcId", "PcColor");
            return View();
        }

     
        [HttpPost]
        public IActionResult Product(Product prod, IFormFile file)
        {
            ViewBag.ProdColorId = new SelectList(db.ProductColors, "PcId", "PcColor");
            ViewBag.ProdCatgId = new SelectList(db.Categories, "CId", "CName");
            if (prod.PCatgId == null)
            {
                TempData["Catg_Mess"] = "Please Select Category";
            }

            if (prod.PColor == null)
            {
                TempData["Color_Mess"] = "Please Select Color";
            }
            else
            {
                if (file != null && file.Length > 0)
                {
                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileexten = file.ContentType.ToLower();
                    var filestring = fileexten.Substring(6);

                    if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                    {
                        var img = filename + "." + filestring;
                        var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                        using (var stream = new FileStream(imgpath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                        prod.PImage = dbpath;
                        TempData["Add_Prod"] = "Product Inserted Successfully";
                        db.Add(prod);
                        db.SaveChanges();
                        return RedirectToAction(nameof(Fetch_Product));
                    }
                    else
                    {
                        TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                    }
                }
            }
            
            return View();
        }
        [HttpGet]
        public IActionResult Edit_Product(int? id)
        {
            ViewBag.ProdCatgId = new SelectList(db.Categories, "CId", "CName");
            ViewBag.ProdColorId = new SelectList(db.ProductColors, "PcId", "PcColor");
            var product = db.Products.Include(p => p.PCatg).FirstOrDefault(p=> p.PId == id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit_Product(Product prod, IFormFile file, string Pr_img)
        {
            if (prod.PCatgId == null)
            {
                TempData["Catg_Mess"] = "Please Select Category";
            }
            if (prod.PColor == null)
            {
                TempData["Color_Mess"] = "Please Select Color";
            }
            ViewBag.ProdColorId = new SelectList(db.ProductColors, "PcId", "PcColor");
            ViewBag.ProdCatgId = new SelectList(db.Categories, "CId", "CName");

            if (file != null && file.Length > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileexten = file.ContentType.ToLower();
                var filestring = fileexten.Substring(6);

                if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                {
                    var img = filename + "." + filestring;
                    var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                    using (var stream = new FileStream(imgpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                    prod.PImage = dbpath;

                    db.Update(prod);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                    return RedirectToAction(nameof(Edit_Product));
                }
            }else
            {
                prod.PImage = Pr_img;
                db.Update(prod);
                db.SaveChanges();
            }
            TempData["U_Prod"] = "Product Update Successfully";

            return RedirectToAction(nameof(Fetch_Product));
        }
        public IActionResult Delete_Product(int? id)
        {
            try
            {
                var product = db.Products.FirstOrDefault(p => p.PId == id);
                if(product != null)
                {
                    db.Remove(product);
                    db.SaveChanges();
                    TempData["D_Prod"] = "Product Deleted Successfully";
                }

            }
            catch(Exception ex)
            {
                TempData["D_Prod"] = ex.Message;
            }
            return RedirectToAction(nameof(Fetch_Product));
        }
        [HttpGet]
        public IActionResult Fetch_Blog_Comment(string search)
        {
            try
            {
                List<BlogComment> Blog;
                if (search != null)
                {
                    Blog = db.BlogComments.Include(u => u.Blo).Include(b => b.Blog).Where(b=> b.Blog.BTitle.Contains(search)).ToList();
                }
                else
                {
                    Blog = db.BlogComments.Include(u => u.Blo).Include(b => b.Blog).ToList();
                }
                return View(Blog);

            }
            catch (Exception ex)
            {
                TempData["B_Fetch Mess"] = ex.Message;
            }
            return View();
        }
     
        public IActionResult Delete_Blog_Comment(int? id)
        {
            try
            {
                var Blog = db.BlogComments.FirstOrDefault(b=> b.BlogCId == id);

                db.Remove(Blog);
                TempData["B_delete"] = "Comment Deleted Successfully";
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                TempData["B_delete"] = ex.Message;
            }
            return RedirectToAction(nameof(Fetch_Blog_Comment));
        }
        [HttpGet]
        public IActionResult Blog()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Blog(Blog Blog, IFormFile file)
        {
            
            if (file != null && file.Length > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileexten = file.ContentType.ToLower();
                var filestring = fileexten.Substring(6);

                if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                {
                    var img = filename + "." + filestring;
                    var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                    using (var stream = new FileStream(imgpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                    Blog.BImage = dbpath;
                    TempData["Add_Blog"] = "Blog Inserted Successfully";
                    db.Add(Blog);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Fetch_Blog));
                }
                else
                {
                    TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult Fetch_Blog(string search)
        {
            try
            {
                List<Blog> Blog;
                if (search != null)
                {
                    Blog = db.Blogs.Where(b=> b.BTitle.Contains(search)).ToList();
                }
                else
                {
                    Blog = db.Blogs.ToList();
                }
                return View(Blog);

            }
            catch (Exception ex)
            {
                TempData["B_Fetch Mess"] = ex.Message;
            }
            return View();
        }
        public IActionResult Delete_Blog(int? id)
        {
            try
            {
                var Blog = db.Blogs.FirstOrDefault(p => p.BId == id);
                if (Blog != null)
                {
                    db.Remove(Blog);
                    db.SaveChanges();
                    TempData["D_Blog"] = "Blog Deleted Successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["D_Blog"] = ex.Message;
            }
            return RedirectToAction(nameof(Fetch_Blog));
        }
        [HttpGet]
        public IActionResult Edit_Blog(int? id)
        {
            var Blog = db.Blogs.FirstOrDefault(p => p.BId == id);
            return View(Blog);
        }
        [HttpPost]
        public IActionResult Edit_Blog(Blog Blog, IFormFile file, string Bl_img)
        {
            if (file != null && file.Length > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileexten = file.ContentType.ToLower();
                var filestring = fileexten.Substring(6);

                if (filestring == "jpg" || filestring == "jpeg" || filestring == "png")
                {
                    var img = filename + "." + filestring;
                    var imgpath = Path.Combine("wwwroot/assets/Admin_Panel/img", img);
                    using (var stream = new FileStream(imgpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbpath = Path.Combine("assets/Admin_Panel/img", img);

                    Blog.BImage = dbpath;

                    db.Update(Blog);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Img_Mess"] = "File Format Should Be jpg, jpeg or png";
                    return RedirectToAction(nameof(Edit_Blog));
                }
            }
            else
            {
                Blog.BImage = Bl_img;
                db.Update(Blog);
                db.SaveChanges();
            }
            TempData["U_Blog"] = "Blog Update Successfully";

            return RedirectToAction(nameof(Fetch_Blog));
        }
        [HttpGet]
        public IActionResult Category()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Category(Category catg)
        {
            try
            {
                var category = db.Categories.FirstOrDefault(c => c.CName == catg.CName);
                if (category != null)
                {
                    TempData["cat_Mess"] = "Already have a Category Named: " + catg.CName;
                    return View();
                }
                db.Add(catg);
                db.SaveChanges();
                TempData["Catg_Mess"] = "Insert Succeccfully";
                return RedirectToAction(nameof(Fetch_Category));
            }
            catch (Exception ex)
            {
                TempData["Catg_Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Fetch_Category(string search)
        {
            try
            {
                List<Category> category;
                if (search != null)
                {
                    category = db.Categories.Where(c=>c.CName.Contains(search)).ToList();
                }
                else
                {
                    category = db.Categories.ToList();
                }
                return View(category);

            }
            catch (Exception ex)
            {
                TempData["C_Fetch Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit_Category(int? id)
        {
            try
            {
                var Category = db.Categories.FirstOrDefault(c=> c.CId == id);
                return View(Category);

            }
            catch (Exception ex)
            {
                TempData["Catg_Mess"] = ex.Message;
                return RedirectToAction(nameof(Fetch_Category));
            }
        }

        [HttpPost]
        public IActionResult Edit_Category(Category catg)
        {
            try
            {
                var category = db.Categories.FirstOrDefault(c => c.CName == catg.CName);
                if (category != null)
                {
                    TempData["cat_Mess"] = "Already have a Category Named: " + catg.CName;
                    return View();
                }
                db.Update(catg);
                db.SaveChanges();
                TempData["U_Catg"] = "Category Updated Successfully";

                return RedirectToAction(nameof(Fetch_Category));
            }
            catch (Exception ex)
            {
                TempData["U_Catg"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Color()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Color(ProductColor color)
        {
            try
            {
                if (!IsColorNameValid(color.PcColor))
                {
                    TempData["Color_Mess"] = "Invalid color name. Please enter a valid color.";
                    return View();
                }
                var exist_color = db.ProductColors.FirstOrDefault(c => c.PcColor == color.PcColor);
                if(exist_color != null)
                {
                    TempData["Color_Mess"] = "Already have a Color Named " + color.PcColor;
                    return View();
                }
                db.Add(color);
                db.SaveChanges();
                TempData["Color_Mess"] = "Insert Succeccfully";
                return RedirectToAction(nameof(Fetch_Color));
            }
            catch (Exception ex)
            {
                TempData["Color_Mess"] = ex.Message;
            }
            return View();
        }
        private bool IsColorNameValid(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return false;

            var colorNames = Enum.GetNames(typeof(KnownColor)).Select(c => c.ToLower());
            return colorNames.Contains(colorName.ToLower());
        }
        [HttpGet]
        public IActionResult Fetch_Color()
        {
            try
            {
                var Color = db.ProductColors.ToList();
                return View(Color);

            }
            catch (Exception ex)
            {
                TempData["C_Fetch Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit_Color(int? id)
        {
            try
            {
                var Color = db.ProductColors.FirstOrDefault(c => c.PcId == id);
                return View(Color);

            }
            catch (Exception ex)
            {
                TempData["Catg_Mess"] = ex.Message;
                return RedirectToAction(nameof(Fetch_Color));
            }
        }
        [HttpPost]
        public IActionResult Edit_Color(ProductColor color)
        {
            try
            {
                if (!IsColorNameValid(color.PcColor))
                {
                    TempData["Color_Mess"] = "Invalid color name. Please enter a valid color.";
                    return View();
                }
                var exist_color = db.ProductColors.FirstOrDefault(c => c.PcColor == color.PcColor);
                if (exist_color != null)
                {
                    TempData["Color_Mess"] = "Already have a Color Named: " + color.PcColor;
                    return View();
                }
                db.Update(color);
                db.SaveChanges();
                TempData["U_Catg"] = "Color Updated Successfully";

                return RedirectToAction(nameof(Fetch_Color));
            }
            catch (Exception ex)
            {
                TempData["U_Catg"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Coupon()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Coupon(Coupon coup)
        {
            try
            {
                var exist_coupon = db.Coupons.FirstOrDefault(c => c.CName == coup.CName);
                if (exist_coupon != null)
                {
                    TempData["coupon_Mess"] = "Already have a Coupon Code: " + coup.CName;
                    return View();
                }
                db.Add(coup);
                db.SaveChanges();
                TempData["CP_Mess"] = "Insert Succeccfully";
                return RedirectToAction(nameof(Fetch_Coupon));
            }
            catch (Exception ex)
            {
                TempData["CP_Mess"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Fetch_Coupon()
        {
            try
            {
                var Coupons = db.Coupons.ToList();
                return View(Coupons);

            }
            catch (Exception ex)
            {
                TempData["C_Fetch Mess"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Fetch_Used_Coupon(string search)
        {
            try
            {
                List<UsedCoupon> Coupons;
                if (search != null)
                {
                    Coupons = db.UsedCoupons.Include(c => c.UcCoupon).Include(u => u.UcUser).Where(c=> c.UcCoupon.CName.Contains(search)).ToList();
                }
                else
                {
                    Coupons = db.UsedCoupons.Include(c => c.UcCoupon).Include(u => u.UcUser).ToList();
                }
                return View(Coupons);

            }
            catch (Exception ex)
            {
                TempData["C_Fetch Mess"] = ex.Message;
            }
            return View();
        }


        [HttpGet]
        public IActionResult Fetch_Orders(string search)
        {
            try
            {
                if (search != null)
                {
                    var order = db.OrderItems.Include(o => o.OrderItemsOrder).ThenInclude(u => u.OrderUser).Where(u=> u.OrderItemsOrder.OrderUser.UName.Contains(search)).Where(s => s.OrderItemsOrder.OrderStatus == "Pending").Include(u => u.OrderItemsProd).ToList();
                    return View(order);
                }
                else
                {
                    var order = db.OrderItems.Include(o => o.OrderItemsOrder).ThenInclude(u => u.OrderUser).Where(s => s.OrderItemsOrder.OrderStatus == "Pending").Include(u => u.OrderItemsProd).ToList();
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                TempData["O_Fetch Mess"] = ex.Message;
            }
            return View();
        }

        public IActionResult Update_Order_Status(int? id)
        {
            try
            {
                var data = db.Orders.FirstOrDefault(o => o.OrderId == id);
                if (data != null)
                {
                    data.OrderStatus = "Delivered";
                    db.Update(data);
                    db.SaveChanges();
                    TempData["status"] = "Status has been updated";

                }
                else
                {
                    TempData["status"] = "Order Not Found";
                }

                return RedirectToAction(nameof(Fetch_Orders_Details));

            }
            catch (Exception ex)
            {
                TempData["status"] = ex.Message;
            }
           return RedirectToAction(nameof(Fetch_Orders_Details));
        }

        [HttpGet]
        public IActionResult Fetch_Orders_Details(string search)
        {
            try
            {
                if (search != null)
                {
                    var Coupons = db.OrderItems.Include(o => o.OrderItemsOrder).ThenInclude(u => u.OrderUser).Where(s => s.OrderItemsOrder.OrderStatus == "Delivered" && s.OrderItemsOrder.OrderUser.UName.Contains(search)).Include(u => u.OrderItemsProd).ToList();
                    return View(Coupons);
                }
                else
                {
                    var Coupons = db.OrderItems.Include(o => o.OrderItemsOrder).ThenInclude(u => u.OrderUser).Where(s => s.OrderItemsOrder.OrderStatus == "Delivered").Include(u => u.OrderItemsProd).ToList();
                    return View(Coupons);
                }

            }
            catch (Exception ex)
            {
                TempData["O_Fetch Mess"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Fetch_Users(string search)
        {
            try
            {
                List<UserRecord> user;
                if(search != null)
                {
                    user = db.UserRecords.Where(u=> u.UEmail.Contains(search)).ToList();
                }
                else
                {
                    user = db.UserRecords.ToList();
                }
                return View(user);

            }
            catch (Exception ex)
            {
                TempData["U_Fetch_Mess"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Fetch_Product_Likes()
        {
            try
            {
                var Product = db.ProductLikes.Include(p=> p.PlProd).Include(u => u.PlUserNavigation).ToList();
                return View(Product);

            }
            catch (Exception ex)
            {
                TempData["L_Fetch_Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Total_Product_Likes(string search)
        {
            try
            {
                List<LikeCount> likes;
                if (search != null)
                {
                   likes = db.LikeCounts.Include(p => p.LcProd).Where(p => p.LcProd.PName.Contains(search)).ToList();
                 
                }
                else
                {
                    likes = db.LikeCounts.Include(p => p.LcProd).ToList();
                }
                return View(likes);


            }
            catch (Exception ex)
            {
                TempData["L_Fetch_Mess"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Fetch_Contact(string search)
        {
            try
            {
                List<Contact> contact;
                if(search != null)
                {
                    contact = db.Contacts.Where(c=> c.CName.Contains(search)).ToList();
                }
                else
                {
                    contact = db.Contacts.ToList();

                }
                return View(contact);

            }
            catch (Exception ex)
            {
                TempData["C_Fetch_Mess"] = ex.Message;
            }
            return View();
        }
        public IActionResult Contact_Delete(int? id)
        {
            try
            {
                var contact = db.Contacts.FirstOrDefault(c => c.CId == id);
                if (contact != null)
                {
                    db.Remove(contact);
                    db.SaveChanges();
                    TempData["D_Contact"] = "Message Deleted Successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["D_Contact"] = ex.Message;
            }
            return RedirectToAction(nameof(Fetch_Contact));
        }

    }
}
