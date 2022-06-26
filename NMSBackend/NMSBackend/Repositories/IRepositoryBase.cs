using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NMSBackend.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(XDocument xmlDocument, XElement entity, string fileName);
        XElement Get(string xmlObjectName, XDocument xmlDocument);
        void Update(XDocument xmlDocument, string fileName);
        IEnumerable<XElement> AsQueryable(XDocument xmlDocument, string objectName);
    }
}
