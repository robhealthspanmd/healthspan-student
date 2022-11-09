using healthspanmd.web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace healthspanmd.web.Helpers
{
    public static class ControllerExtensions
    {
        public static BreadCrumbViewModel AddBreadCrumb(this Controller controller, string caption)
        {
            BreadCrumbViewModel breadCrumbs = null;
            if (controller.ViewData.ContainsKey("Breadcrumbs"))
                breadCrumbs = (BreadCrumbViewModel)controller.ViewData["Breadcrumbs"];
            else 
                breadCrumbs = new BreadCrumbViewModel { Items = new List<BreadCrumbViewModel.BreadCrumb>() };

            if (breadCrumbs.Items == null)
                breadCrumbs.Items = new List<BreadCrumbViewModel.BreadCrumb>();

            breadCrumbs.Items.Add(new BreadCrumbViewModel.BreadCrumb
            {
                Caption = caption
            });
            controller.ViewData["Breadcrumbs"] = breadCrumbs;
            return breadCrumbs;
        }

        public static BreadCrumbViewModel AddBreadCrumb(this Controller controller, string caption, string url)
        {
            BreadCrumbViewModel breadCrumbs = null;
            if (controller.ViewData.ContainsKey("Breadcrumbs"))
                breadCrumbs = (BreadCrumbViewModel)controller.ViewData["Breadcrumbs"];
            else
                breadCrumbs = new BreadCrumbViewModel { Items = new List<BreadCrumbViewModel.BreadCrumb>() };

            if (breadCrumbs.Items == null)
                breadCrumbs.Items = new List<BreadCrumbViewModel.BreadCrumb>();

            breadCrumbs.Items.Add(new BreadCrumbViewModel.BreadCrumb
            {
                Caption = caption,
                Url = url
            });
            controller.ViewData["Breadcrumbs"] = breadCrumbs;
            return breadCrumbs;
        }

        public static DateTime JavascriptDateStringToDate(this string dateStr)
        {

            var parts = dateStr.Split('-');
            return new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));

        }

        //public static BreadCrumbViewModel CreateBreadCrumbs(this Controller controller)
        //{
        //    return new BreadCrumbViewModel
        //    {
        //        Items = new List<BreadCrumbViewModel.BreadCrumb>()
        //    };
        //}

        //public static BreadCrumbViewModel AddItem(this BreadCrumbViewModel breadCrumbs, string caption)
        //{
        //    if (breadCrumbs.Items == null)
        //        breadCrumbs.Items = new List<BreadCrumbViewModel.BreadCrumb>();
        //    breadCrumbs.Items.Add(new BreadCrumbViewModel.BreadCrumb
        //    {
        //        Caption = caption
        //    });
        //    return breadCrumbs;
        //}

        //public static BreadCrumbViewModel AddItem(this BreadCrumbViewModel breadCrumbs, string caption, string url)
        //{
        //    if (breadCrumbs.Items == null)
        //        breadCrumbs.Items = new List<BreadCrumbViewModel.BreadCrumb>();
        //    breadCrumbs.Items.Add(new BreadCrumbViewModel.BreadCrumb
        //    {
        //        Caption = caption,
        //        Url = url
        //    });
        //    return breadCrumbs;
        //}
    }
}
