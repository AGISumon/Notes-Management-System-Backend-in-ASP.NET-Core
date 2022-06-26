using Common.Helpers;
using NMSBackend.Helpers;
using NMSBackend.Interface;
using NMSBackend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace NMSBackend.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ApiReturnObj returnObj = new ApiReturnObj();
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User user)
        {
            try
            {
                var data = _userService.Login(user, out string message);
                if (data != null)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = message;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    returnObj.Message = message;
                    return Ok(returnObj);
                }
            }
            catch (Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.ApiData = null;
                if (ex.InnerException != null)
                {
                    returnObj.Message = ex.InnerException.Message;
                }
                else
                {
                    returnObj.Message = ex.Message;
                }
                return Ok(returnObj);
            }
        }

        //[HttpPost]
        //[Route("RefreshToken")]
        //public IActionResult RefreshToken(Token token)
        //{
        //    try
        //    {
        //        var data = _userService.RefreshToken(token, out string message);
        //        if (data != null)
        //        {
        //            returnObj.IsExecute = true;
        //            returnObj.ApiData = data;
        //            returnObj.Message = MessageConst.SuccussLogin;
        //            return Ok(returnObj);
        //        }
        //        else
        //        {
        //            returnObj.IsExecute = false;
        //            returnObj.ApiData = null;
        //            returnObj.Message = message;
        //            return Ok(returnObj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnObj.IsExecute = false;
        //        returnObj.ApiData = null;
        //        if (ex.InnerException != null)
        //        {
        //            returnObj.Message = ex.InnerException.Message;
        //        }
        //        else
        //        {
        //            returnObj.Message = ex.Message;
        //        }
        //        return Ok(returnObj);
        //    }
        //}

        [HttpGet]
        [Route("Logout/{emailId}")]
        public IActionResult Logout(string emailId)
        {
            try
            {
                var data = _userService.Logout(emailId);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    return Ok(returnObj);
                }
            }
            catch (Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.ApiData = null;
                if (ex.InnerException != null)
                {
                    returnObj.Message = ex.InnerException.Message;
                }
                else
                {
                    returnObj.Message = ex.Message;
                }
                return Ok(returnObj);
            }
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(User user)
        {
            try
            {
                var data = _userService.SignUp(user);
                if (data)
                {
                    returnObj.IsExecute = true;
                    returnObj.ApiData = data;
                    returnObj.Message = MessageConst.Insert;
                    return Ok(returnObj);
                }
                else
                {
                    returnObj.IsExecute = false;
                    returnObj.ApiData = null;
                    returnObj.Message = MessageConst.IsExist;
                    return Ok(returnObj);
                }
            }
            catch(Exception ex)
            {
                returnObj.IsExecute = false;
                returnObj.ApiData = null;
                if (ex.InnerException != null)
                {
                    returnObj.Message = ex.InnerException.Message;
                }
                else
                {
                    returnObj.Message = ex.Message;
                }
                return Ok(returnObj);
            }
        }
    }
}
