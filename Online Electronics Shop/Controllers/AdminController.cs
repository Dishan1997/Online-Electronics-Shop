using Newtonsoft.Json;
using Online_Electronics_Shop.DB;
using Online_Electronics_Shop.Models;
using Online_Electronics_Shop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Electronics_Shop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbOnlineElectronicsShopEntities ctx = new dbOnlineElectronicsShopEntities();


        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach (var item in cat)

            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });

            }
            return list;
        }



        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }

        public ActionResult AdminLogin()
        {

            return View();
        }

        public JsonResult CheckValidUser(SiteAdmin model)
        {
            string result = "Fail";
            var DataItem = ctx.SiteAdmins.Where(x => x.Username == model.Username && x.Password == model.Password).SingleOrDefault();

            // string Username = "Admin";
            // string Password= "1234";





            if (DataItem != null)
            {
                //Session["UserID"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.Username.ToString();
                //Session["UserName"] = "Admin";
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Logout()
        {

                Session.Clear();
                Session.Abandon();
                return RedirectToAction("AdminLogin");
           

           


        }
        public ActionResult Dashboard()
        {
            if (Session["UserName"] != null)
            {

                return View();
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }

               
        }

        public ActionResult Categories()
        {
            //List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
            //return View(allcategories);
            if (Session["UserName"] != null)
            {
                return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords());
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }

        }

        public ActionResult CategoryAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        [HttpPost]
        public ActionResult CategoryAdd(Tbl_Category tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Add(tbl);
            return RedirectToAction("Categories");
        }

        public ActionResult CategoryEdit(int catId)
        {

            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(catId));
        }

        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {

            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
            //return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords().Where(i => i.IsDelete == false));
        }

        public ActionResult CategoryDelete(int catId)
        {
            // ViewBag.CategoryList = GetCategory();
            //return View();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(catId));
        }
        [HttpPost]
        public ActionResult CategoryDelete(Tbl_Category tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
            // _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords().Where(i => i.IsDelete == false);
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Delete(tbl);
            return RedirectToAction("Categories");
           

        }


        public ActionResult Product()
        {
            if (Session["UserName"] != null)
            {
                return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct());
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult ProductDelete(int productId)
        {
            // ViewBag.CategoryList = GetCategory();
            //return View();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productId));
        }
        [HttpPost]
        public ActionResult ProductDelete(Tbl_Product tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
            // _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords().Where(i => i.IsDelete == false);
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Delete(tbl);
            return RedirectToAction("Product");

        }

        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productId));
        }

        [HttpPost]
        public ActionResult ProductEdit(Tbl_Product tbl, HttpPostedFileBase file)

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

        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        [HttpPost]
        public ActionResult ProductAdd(Tbl_Product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                //file is uploaded
                file.SaveAs(path);



            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Product");
        }


        public ActionResult Orders()
        {
            if (Session["UserName"] != null)
            {

                using (dbOnlineElectronicsShopEntities db = new dbOnlineElectronicsShopEntities())
                {

                    List<tbl_invoice> ivc = db.tbl_invoice.ToList();
                    List<tbl_order> ods = db.tbl_order.ToList();
                    List<Tbl_ShippingDetails> shd = db.Tbl_ShippingDetails.ToList();
                    List<SiteUser> su = db.SiteUsers.ToList();
                    List<Tbl_Product> tp = db.Tbl_Product.ToList();

                    var orderRecord = from o in ods
                                      join i in ivc on o.o_fk_invoice equals i.in_id into table1
                                      from i in table1.ToList()
                                      join sh in shd on i.in_id equals sh.InvoiceId into table2
                                      from sh in table2.ToList()
                                      join s in su on i.in_fk_user equals s.ID into table3
                                      from s in table3.ToList()
                                      join p in tp on o.o_fk_pro equals p.ProductId into table4
                                      from p in table4.ToList()

                                      select new ViewModel
                                      {
                                          ivc = i,
                                          ods = o,
                                          shd = sh,
                                          su = s,
                                          tp = p


                                      };







                    return View(orderRecord);
                }
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }









        public ActionResult Messages()
        {
            if (Session["UserName"] != null)
            {
                using (dbOnlineElectronicsShopEntities db = new dbOnlineElectronicsShopEntities())
                {


                    List<Tbl_Contact> conc = db.Tbl_Contact.ToList();
                    //List<tbl_invoice> ivc = db.tbl_invoice.ToList();
                    //List<tbl_order> ods = db.tbl_order.ToList();
                    //List<Tbl_ShippingDetails> shd = db.Tbl_ShippingDetails.ToList();
                    // List<SiteUser> su = db.SiteUsers.ToList();
                    //List<Tbl_Product> tp = db.Tbl_Product.ToList();

                    var messageRecord = from c in conc







                                        select new ViewModel1
                                        {

                                            conc = c



                                        };







                    return View(messageRecord);
                }
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }
        }



        public ActionResult Status()
        {
            //ViewBag.CategoryList = GetCategory();
            return View();
        }

        List<OStatus> li2 = new List<OStatus>();


        [HttpPost]
        public ActionResult Status(Tbl_OrderStatus tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
           
            _unitOfWork.GetRepositoryInstance<Tbl_OrderStatus>().Add(tbl);
            
            using (dbOnlineElectronicsShopEntities db = new dbOnlineElectronicsShopEntities())
            {
              
                List<Tbl_OrderStatus> li = db.Tbl_OrderStatus.ToList();
                List<tbl_order> li1 = db.tbl_order.ToList();


                foreach (var item in li)
                 {
                     
                    tbl_delivery dlv = new tbl_delivery();
                     //tbl_delivery dlv = ctx.tbl_delivery.FirstOrDefault(x => x.o_id == item.OrderId);
                     dlv.o_id = item.OrderId;
                     dlv.d_date = System.DateTime.Now;

                     ctx.tbl_delivery.Add(dlv);

                     ctx.SaveChanges();


                 }


               










                return RedirectToAction("Status");
            }
        }

        public ActionResult Delivery()
        {
            if (Session["UserName"] != null)
            {
                using (dbOnlineElectronicsShopEntities db = new dbOnlineElectronicsShopEntities())
                {

                    List<tbl_delivery> dv = db.tbl_delivery.ToList();
                    List<tbl_invoice> ivc = db.tbl_invoice.ToList();
                    List<tbl_order> ods = db.tbl_order.ToList();
                    List<Tbl_ShippingDetails> shd = db.Tbl_ShippingDetails.ToList();
                    List<SiteUser> su = db.SiteUsers.ToList();
                    List<Tbl_Product> tp = db.Tbl_Product.ToList();

                    var deliveryRecord = from d in dv
                                         join o in ods on d.o_id equals o.o_id into table1
                                         from o in table1.ToList()



                                         join i in ivc on o.o_fk_invoice equals i.in_id into table2
                                         from i in table2.ToList()
                                         join sh in shd on i.in_id equals sh.InvoiceId into table3
                                         from sh in table3.ToList()
                                         join s in su on i.in_fk_user equals s.ID into table4
                                         from s in table4.ToList()
                                         join p in tp on o.o_fk_pro equals p.ProductId into table5
                                         from p in table5.ToList()

                                         select new ViewModel
                                         {
                                             ivc = i,
                                             dv = d,
                                             ods = o,
                                             shd = sh,
                                             su = s,
                                             tp = p


                                         };



                    var deliveryRecord1 = from d in dv




                                          join i in ivc on d.o_fk_invoice equals i.in_id into table1
                                          from i in table1.ToList()
                                          join sh in shd on i.in_id equals sh.InvoiceId into table2
                                          from sh in table2.ToList()
                                          join s in su on i.in_fk_user equals s.ID into table3
                                          from s in table3.ToList()
                                          join p in tp on d.o_fk_pro equals p.ProductId into table4
                                          from p in table4.ToList()

                                          select new ViewModel
                                          {
                                              ivc = i,
                                              dv = d,

                                              shd = sh,
                                              su = s,
                                              tp = p


                                          };




                    return View(deliveryRecord1);
                }
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

       
        public ActionResult DeleteOrder(int o_id)

        {

            return View(_unitOfWork.GetRepositoryInstance<tbl_order>().GetFirstorDefault(o_id));



        }
        public ActionResult EditStatus(int o_id)

        {
           
            return View(_unitOfWork.GetRepositoryInstance<tbl_order>().GetFirstorDefault(o_id));



        }

        [HttpPost]
        public ActionResult EditStatus(tbl_order tbl, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                //file is uploaded
                file.SaveAs(path);



            }
            //tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            //tbl.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<tbl_order>().Update(tbl);
            return RedirectToAction("Order");
        }



        public ActionResult ConfirmDelete(int o_id)
        {
            // ViewBag.CategoryList = GetCategory();
            //return View();
            return View(_unitOfWork.GetRepositoryInstance<tbl_order>().GetFirstorDefault(o_id));
        }
        [HttpPost]
        public ActionResult ConfirmDelete(tbl_order tbl)
        {
            //tbl.CreatedDate = DateTime.Now;
            // _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords().Where(i => i.IsDelete == false);
            _unitOfWork.GetRepositoryInstance<tbl_order>().Delete(tbl);
            return RedirectToAction("Orders");


        }

        public ActionResult ConfirmDelivery(int? Id)

        {
            tbl_order o = ctx.tbl_order.Where(x => x.o_id == Id).SingleOrDefault();
            return View(o);



        }

        List<OStatus> li = new List<OStatus>();
        [HttpPost]
        public ActionResult ConfirmDelivery(tbl_order pi, int Id)
        {
           tbl_order p = ctx.tbl_order.Where(x => x.o_id == Id).SingleOrDefault();

            OStatus c = new OStatus();
            c.OrderId = p.o_id;
            c.ProductId = (int)p.o_fk_pro;
            c.Quantity = (int)p.o_qty;
            c.InvoiceId = (int)p.o_fk_invoice;
            c.Bill = (float)p.o_bill;
            c.UnitPrice = (float)p.o_unitprice;
            c.OrderDate = (DateTime)p.o_date;
            
            

            
            //c.qty = Convert.ToInt32(qty);
            //c.o_bill = p.o_bill;
            
            if (TempData["OStatus"] == null)
            {
                li.Add(c);
                TempData["OStatus"] = li;
                

            }
            

            TempData.Keep();




            return RedirectToAction("Index");
        }

        public ActionResult Check()
        {
            TempData.Keep();


            return View();
        }

        public ActionResult AddDeliveryDetails()
        {

            TempData.Keep();

            return View();

        }

        [HttpPost]

        //public ActionResult AddShippingDetails(tbl_order O)
        public ActionResult AddDeliveryDetails(tbl_delivery O)
        {
            List<OStatus> li = TempData["OStatus"] as List<OStatus>;
            tbl_invoice iv = new tbl_invoice();
            tbl_order od = new tbl_order();



            tbl_delivery sh = new tbl_delivery();


            foreach (var item in li)
            {
               

                //tbl_delivery sh = ctx.tbl_delivery.FirstOrDefault(x => x.o_id == item.OrderId);
                sh.o_id = item.OrderId;
                od.o_id = item.OrderId;
                sh.o_fk_pro = item.ProductId;
                sh.o_fk_invoice = item.InvoiceId;
                //sh.o_date = od.o_date;
                sh.o_date = item.OrderDate;
                sh.o_qty = item.Quantity;
                sh.o_unitprice = item.UnitPrice;
                sh.o_bill = item.Bill;
                sh.PaymentType = O.PaymentType;
                sh.d_date = System.DateTime.Now;
                //sh.OrderId = iv.in_id;
                //sh.InvoiceId = iv.in_id;
                ctx.tbl_delivery.Add(sh);
                ctx.SaveChanges();

               
               
                //ctx.tbl_order.Remove(p);
                //ctx.SaveChanges();
                //_unitOfWork.GetRepositoryInstance<tbl_order>().Remove(p);
               // _unitOfWork.GetRepositoryInstance<tbl_order>().GetFirstorDefault(p);

            }

            tbl_order p = ctx.tbl_order.Where(x => x.o_id == sh.o_id).SingleOrDefault();

            TempData.Remove("OStatus");

            TempData["msg"] = "Delivery Recorded....";
            TempData.Keep();

            ctx.tbl_order.Remove(p);
            ctx.SaveChanges();

            //return View(p);
            return RedirectToAction("Orders");
        }


        







    }
}