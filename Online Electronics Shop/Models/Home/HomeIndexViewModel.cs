﻿using Online_Electronics_Shop.DB;
using Online_Electronics_Shop.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Online_Electronics_Shop.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbOnlineElectronicsShopEntities context = new dbOnlineElectronicsShopEntities();
        public IPagedList<Tbl_Product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@search",search??(object)DBNull.Value)


                };

            IPagedList<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {

                // ListOfProducts = _unitOfWork.GetRepositoryInstance<Tbl_Product>().GetAllRecords()

                ListOfProducts = data

            };

        }
    }
}