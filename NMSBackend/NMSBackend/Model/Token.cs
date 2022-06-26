using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NMSBackend.Model
{
    public class Token
    {
        [XmlElement("Step")]
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
