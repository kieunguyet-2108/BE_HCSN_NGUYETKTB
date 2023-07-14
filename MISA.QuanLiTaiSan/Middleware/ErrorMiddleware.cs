using Microsoft.AspNetCore.Http;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Resources;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MISA.QuanLiTaiSan.Api.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ResponseModel responseModel = new ResponseModel();
            switch (ex)
            {
                case MISAException:
                    MISAException msEx = (MISAException)ex;
                    responseModel.DevMsg = msEx.ErrorMsg;
                    responseModel.UserMsg = ResourceVN.Msg_Exception;
                    responseModel.Error = msEx.Result;
                    responseModel.Data = msEx.Data;
                    context.Response.StatusCode = (int)MSCODE.BadRequest;
                    break;
                default:
                    responseModel.DevMsg = ex.Message;
                    responseModel.UserMsg = ex.Message;
                    responseModel.Error = ex.Data;
                    context.Response.StatusCode = (int)MSCODE.Error;
                    break;
            }
            var jsonResponse = JsonConvert.SerializeObject(responseModel);
            context.Response.ContentType = "application/json"; // Trả về kiểu dữ liệu json
            await context.Response.WriteAsync(jsonResponse); // Trả về kết quả
        }
    }
}
