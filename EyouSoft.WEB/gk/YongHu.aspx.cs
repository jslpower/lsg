﻿//用户管理 汪奇志 2015-05-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Toolkit;

namespace EyouSoft.WEB.gk
{
    using System.Text;

    using EyouSoft.Model;

    public partial class YongHu : GkYeMian
    {
        #region attributes
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount = 0;
        #endregion

        protected YongHuLeiXing T = (YongHuLeiXing)Utils.GetInt(Utils.GetQueryStringValue("T"), 0);
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrivs();

            switch (Utils.GetQueryStringValue("dotype"))
            {
                case "shanchu": ShanChu(); break;
                case "qiyong": QiYong(); break;
                case "jinyong": JinYong(); break;
                default: break;
            }


            InitRpt();
        }

        #region private members
        /// <summary>
        /// get chaxun
        /// </summary>
        /// <returns></returns>
        EyouSoft.Model.MYongHuChaXunInfo GetChaXunInfo()
        {
            var info = new EyouSoft.Model.MYongHuChaXunInfo();

            info.GongSiName = Utils.GetQueryStringValue("txtGongSiName");
            info.LeiXing = T;
            info.Name = Utils.GetQueryStringValue("txtName");
            info.Username = Utils.GetQueryStringValue("txtUsername");
            info.Status = (EyouSoft.Model.YongHuStatus?)Utils.GetEnumValueNullable(typeof(EyouSoft.Model.YongHuStatus), Utils.GetQueryStringValue("txtStatus"));
            info.ShenHeStatus = EyouSoft.Model.ShenHeStatus.已审核;

            return info;
        }

        /// <summary>
        /// init repeater
        /// </summary>
        void InitRpt()
        {
            pageIndex = Utils.GetPadingIndex();
            var chaXun = GetChaXunInfo();

            var items = new EyouSoft.BLL.BYongHu().GetYongHus(pageSize, pageIndex, ref recordCount, chaXun);

            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                phEmpty.Visible = false;
            }
            else
            {
                phEmpty.Visible = true;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        void ShanChu()
        {
            string txtYongHuId = Utils.GetFormValue("txtYongHuId");

            int bllRetCode = new EyouSoft.BLL.BYongHu().YongHu_D(YongHuInfo.GongSiId, txtYongHuId);

            if (bllRetCode == 1) Utils.RCWE_AJAX("1", "操作成功");
            else Utils.RCWE_AJAX("0", "操作失败");
        }

        /// <summary>
        /// 启用
        /// </summary>
        void QiYong()
        {
            string txtYongHuId = Utils.GetFormValue("txtYongHuId");

            int bllRetCode = new EyouSoft.BLL.BYongHu().QiYong(YongHuInfo.GongSiId, txtYongHuId);

            if (bllRetCode == 1) Utils.RCWE_AJAX("1", "操作成功");
            else Utils.RCWE_AJAX("0", "操作失败");
        }

        /// <summary>
        /// 禁用
        /// </summary>
        void JinYong()
        {
            string txtYongHuId = Utils.GetFormValue("txtYongHuId");

            int bllRetCode = new EyouSoft.BLL.BYongHu().JinYong(YongHuInfo.GongSiId, txtYongHuId);

            if (bllRetCode == 1) Utils.RCWE_AJAX("1", "操作成功");
            else Utils.RCWE_AJAX("0", "操作失败");
        }

        /// <summary>
        /// init privs
        /// </summary>
        void InitPrivs()
        {
            if (T == YongHuLeiXing.采购商)
            {
                if (!YongHuInfo.IsPrivs(EyouSoft.Model.GK_Privs1.采购商管理))
                {
                    Response.Redirect("/gk/default1.aspx");
                }
            }
            else if (T == YongHuLeiXing.供应商)
            {
                if (!YongHuInfo.IsPrivs(EyouSoft.Model.GK_Privs1.供应商管理))
                {
                    Response.Redirect("/gk/default1.aspx");
                }
            }
            else
            {
                if (!YongHuInfo.IsPrivs(EyouSoft.Model.GK_Privs1.系统设置))
                {
                    Response.Redirect("/gk/default1.aspx");
                }
            }
        }
        #endregion

        #region protected members
        /// <summary>
        /// get caozuo
        /// </summary>
        /// <param name="status">用户状态</param>
        /// <returns></returns>
        protected string GetCaoZuo(object status)
        {
            string s = string.Empty;
            var _status = (EyouSoft.Model.YongHuStatus)status;

            if (_status == EyouSoft.Model.YongHuStatus.禁用)
            {
                s = "<a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"bianji\">编辑</a> <a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"qiyong\">启用</a> <a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"shanchu\">删除</a>";
            }

            if (_status == EyouSoft.Model.YongHuStatus.启用)
            {
                s = "<a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"bianji\">编辑</a> <a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"jinyong\">禁用</a> <a href=\"javascript:void(0)\" class=\"blue_btn\" data-class=\"shanchu\">删除</a>";
            }

            return s;
        }

        #endregion
    }
}
