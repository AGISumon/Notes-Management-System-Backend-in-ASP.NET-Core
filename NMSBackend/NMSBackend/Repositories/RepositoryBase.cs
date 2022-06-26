using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace NMSBackend.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public virtual void Add(XDocument xmlDocument, XElement entity, string fileName)
        {
            xmlDocument.Root.Add(entity);
            xmlDocument.Save(fileName);
        }
        public virtual XElement Get(string xmlObjectName, XDocument xmlDocument)
        {
            return xmlDocument.Descendants(xmlObjectName).FirstOrDefault();
        }
        public virtual void Update(XDocument xmlDocument, string fileName)
        {
            xmlDocument.Save(fileName);
        }
        public virtual IEnumerable<XElement> AsQueryable(XDocument xmlDocument, string objectName)
        {
             return xmlDocument.Descendants(objectName);
        }
    }
}
