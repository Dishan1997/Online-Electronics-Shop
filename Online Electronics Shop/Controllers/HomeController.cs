using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Online_Electronics_Shop.Repository;
using Online_Electronics_Shop.Models;
using Online_Electronics_Shop.DB;
using Online_Electronics_Shop.Models.Home;

namespace Online_Electronics_Shop.Controllers
{
    public class HomeController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbOnlineElectronicsShopEntities ctx = new dbOnlineElectronicsShopEntities();
        public ActionResult Index(string search, int? page)
        {
            Session["ID"] = Session["UserID"];


            HomeIndexViewModel model = new HomeIndexViewModel();
            if (TempData["cart"] != null)
            {
                float x = 0;
                float y = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    x += item.bill;
                    y += item.qty;

                }

                TempData["total"] = x;
                TempData["total_item"] = y;
            }
            TempData.Keep();

            return View(model.CreateModel(search, 4, page));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(Tbl_Contact tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Contact>().Add(tbl);
            TempData["msg1"] = "Thanks For Contacting With Us....";
            TempData.Keep();

            return RedirectToAction("Contact");
        }


        public ActionResult Details(int productId)
        {
            // ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productId));
        }

        [HttpPost]
        public ActionResult Details(Tbl_Product tbl, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                //file is uploaded
                file.SaveAs(path);



            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Update(tbl);
            return RedirectToAction("Product");
        }




        public ActionResult AddCart(int? Id)

        {
            Tbl_Product p = ctx.Tbl_Product.Where(x => x.ProductId == Id).SingleOrDefault();
            return View(p);



        }

        List<cart> li = new List<cart>();
        [HttpPost]
        public ActionResult AddCart(Tbl_Product pi, string qty, int Id)
        {
            Tbl_Product p = ctx.Tbl_Product.Where(x => x.ProductId == Id).SingleOrDefault();

            cart c = new cart();
            c.productid = p.ProductId;
            c.price = (float)p.Price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.productname = p.ProductName;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;

            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productid == c.productid)
                    {

                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;


                    }


                }

                if (flag == 0)
                {
                    li2.Add(c);
                }


                TempData["cart"] = li2;
            }

            TempData.Keep();




            return RedirectToAction("Index");
        }

        public ActionResult remove(int? id)
        {
            List<cart> li2 = TempData["cart"] as List<cart>;
            cart c = li2.Where(x => x.productid == id).SingleOrDefault();
            li2.Remove(c);
            float h = 0;
            foreach (var item in li2)
            {


                h += item.bill;

            }
            TempData["total"] = h;
            //TempData["cart"] = li2;
            /*if (h == 0)
            {
                TempData["cart"] = null;

            }*/
            if (h == 0)
            {
                TempData["total"] = null;

            }
            TempData.Keep();

           
            
            return RedirectToAction("checkout");





        }

        public ActionResult checkout()
        {
            TempData.Keep();


            return View();
        }

        public ActionResult AddShippingDetails()
        {

            TempData.Keep();

            return View();

        }

        [HttpPost]

        //public ActionResult AddShippingDetails(tbl_order O)
        public ActionResult AddShippingDetails(Tbl_ShippingDetails O)
        {
            List<cart> li = TempData["cart"] as List<cart>;
            tbl_invoice iv = new tbl_invoice();
            iv.in_fk_user = Convert.ToInt32(Session["ID"].ToString());
            iv.in_date = System.DateTime.Now;
            iv.in_totalbill = (float)TempData["total"];
            ctx.tbl_invoice.Add(iv);
            ctx.SaveChanges();


            foreach (var item in li)
            {
                tbl_order od = new tbl_order();
                od.o_fk_pro = item.productid;
                od.o_fk_invoice = iv.in_id;
                od.o_date = System.DateTime.Now;
                od.o_qty = item.qty;
                od.o_unitprice = (float)item.price;
                od.o_bill = item.bill;
                ctx.tbl_order.Add(od);
                ctx.SaveChanges();


            }

            foreach (var itm in li)
            {
                //Tbl_Product pd = new Tbl_Product();

                Tbl_Product pd = ctx.Tbl_Product.FirstOrDefault(x => x.ProductId == itm.productid);

                pd.Quantity = pd.Quantity - itm.qty;

                ctx.SaveChanges();

            }

            Tbl_ShippingDetails sh = new Tbl_ShippingDetails();
            sh.FirstName = O.FirstName;
            sh.LastName = O.LastName;
            sh.State = O.State;
            sh.City = O.City;
            sh.Country = O.Country;
            sh.ZipCode = O.ZipCode;
            sh.Address = O.Address;
            sh.ContactNo = O.ContactNo;
            //sh.OrderId = iv.in_id;
            sh.InvoiceId = iv.in_id;
            ctx.Tbl_ShippingDetails.Add(sh);
            ctx.SaveChanges();





            TempData.Remove("total");
            TempData.Remove("total_item");
            TempData.Remove("cart");

            TempData["msg"] = "Transaction Completed....";
            TempData.Keep();



            return RedirectToAction("Index");
        }



        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
            // return View("~/Views/Home/Index");
        }

    }
}