using Common.Helpers;
using NMSBackend.Helpers;
using NMSBackend.Interface;
using NMSBackend.Model;
using NMSBackend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;

namespace NMSBackend.Service
{
    public class UserService : IUserService
    {
        XDocument userXmldoc;
        XDocument tokenXmldoc;
        string userXmlFilePath = Path.Combine("XmlFiles", "User.xml");
        string tokenXmlFilePath = Path.Combine("XmlFiles", "Token.xml");
        private IUserRepository _userRepository;
        private ITokenRepository _tokenRepository;
        public UserService(IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            userXmldoc = XDocument.Load(userXmlFilePath);
            tokenXmldoc = XDocument.Load(tokenXmlFilePath);
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }
        public LoginResponse Login(User user, out string message)
        {
            var saltTest = HashingHelper.GenerateSalt();
            var passwordHashTest = HashingHelper.HashUsingPbkdf2(user.Password, saltTest);
            XElement userData = _userRepository.AsQueryable(userXmldoc, "User").FirstOrDefault(p => p.Element("Email").Value == user.Email);
            if (userData == null)
            {
                message = MessageConst.InvalidEmailId;
                return null;
            }
            var salt = HashingHelper.GenerateSalt();
            var passwordHash = HashingHelper.HashUsingPbkdf2(user.Password, userData.Element("PasswordSalt").Value);
            if (userData.Element("Password").Value != passwordHash)
            {
                message = MessageConst.InvalidPassword;
                return null;
            }
            User obj = new User();
            obj.Email = userData.Element("Email").Value;
            var accessToken = TokenHelper.GenerateToken(obj);
            var refreshToken = TokenHelper.GenerateRefreshToken();
            var tokenDataList = _tokenRepository.AsQueryable(tokenXmldoc, "Token").Where(p => p.Element("Email").Value == user.Email && p.Element("Recstatus").Value == RecordStatus.Active);
            foreach(var tokenData in tokenDataList)
            {
                tokenData.Element("Recstatus").Value = RecordStatus.Inactive;
                _tokenRepository.Update(tokenXmldoc, tokenXmlFilePath);
            }
            XElement newTokenData = new XElement("Token",
                    new XElement("Email", user.Email),
                    new XElement("AccessToken", accessToken),
                    new XElement("RefreshToken", refreshToken),
                    new XElement("RefreshTokenExpiryTime", DateTime.Now.AddHours(4)),
                    new XElement("Recstatus", RecordStatus.Active)
                );
            _tokenRepository.Add(tokenXmldoc, newTokenData, tokenXmlFilePath);
            var response = new LoginResponse
            {
                User = new User {
                    Name = userData.Element("Name").Value,
                    Email = userData.Element("Email").Value,
                    DateOfBirth = !string.IsNullOrEmpty(userData.Element("DateOfBirth").Value) ? Convert.ToDateTime(userData.Element("DateOfBirth").Value) : null
                },
                Token = new Token {
                    AccessToken = newTokenData.Element("AccessToken").Value,
                    RefreshToken = newTokenData.Element("RefreshToken").Value
                }
            };
            message = MessageConst.SuccussLogin;
            return response;
        }

        //public LoginResponse RefreshToken(Token token, out string message)
        //{
        //    message = "";
        //    var principal = TokenHelper.GetPrincipalFromExpiredToken(token.AccessToken);
        //    var emailId = principal.Claims.FirstOrDefault().Value; //this is mapped to the Name claim by default
        //    var user = _userRepository.AsQueryable().FirstOrDefault(x => x.Email == emailId);
        //    if (user == null)
        //    {
        //        message = MessageConst.InvalidUserId;
        //        return null;
        //    }
        //    var tokenData = _tokenRepository.AsQueryable().FirstOrDefault(x => x.Email == user.Email);
        //    if (tokenData != null)
        //    {
        //        if (tokenData.RefreshToken != token.RefreshToken || tokenData.RefreshTokenExpiryTime <= DateTime.Now)
        //        {
        //            message = MessageConst.InvalidRefreshToken;
        //            return null;
        //        }
        //    }
        //    var newAccessToken = TokenHelper.GenerateToken(user);
        //    var newRefreshToken = TokenHelper.GenerateRefreshToken();
        //    tokenData.RefreshToken = newRefreshToken;
        //    _userRepository.Update(user);
        //    var obj = new LoginResponse
        //    {
        //        User = user,
        //        Token = tokenData
        //    };
        //    return obj;
        //}

        public bool Logout(string emailId)
        {
            var tokenData = _tokenRepository.AsQueryable(tokenXmldoc, "Token").FirstOrDefault(p => p.Element("Email").Value == emailId && p.Element("Recstatus").Value == RecordStatus.Active);
            if (tokenData == null)
                return false;
            tokenData.Element("Recstatus").Value = RecordStatus.Inactive;
            _tokenRepository.Update(tokenXmldoc, tokenXmlFilePath);
            return true;
        }

        public bool SignUp(User user)
        {
            XElement userExist = _userRepository.AsQueryable(userXmldoc, "User").FirstOrDefault(p => p.Element("Email").Value == user.Email);
            if (userExist != null)
            {
                return false;
            }
            else
            {
                var salt = HashingHelper.GenerateSalt();
                var passwordHash = HashingHelper.HashUsingPbkdf2(user.Password, salt);
                user.Password = passwordHash;
                user.PasswordSalt = salt;
                XElement userData = new XElement("User",
                    new XElement("Name", user.Name),
                    new XElement("Email", user.Email),
                    new XElement("DateOfBirth", user.DateOfBirth),
                    new XElement("Password", user.Password),
                    new XElement("PasswordSalt", user.PasswordSalt)
                );
                _userRepository.Add(userXmldoc, userData, userXmlFilePath);
                return true;
            }
        }
    }
}
