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
    public class NoteService : INoteService
    {
        XDocument noteXmldoc;
        string noteXmlFilePath = Path.Combine("XmlFiles", "Note.xml");
        private INoteRepository _noteRepository;
        public NoteService(INoteRepository noteRepository)
        {
            noteXmldoc = XDocument.Load(noteXmlFilePath);
            _noteRepository = noteRepository;
        }

        public bool Add(Note note)
        {
            int noteId = 0;
            int count = _noteRepository.AsQueryable(noteXmldoc, "Note").Count();
            if (count == 0)
            {
                noteId = 1;
            }
            else
            {
                noteId = _noteRepository.AsQueryable(noteXmldoc, "Note").Max(x => (int)x.Element("NoteId")) + 1;
            }
            XElement noteData = new XElement("Note",
                    new XElement("NoteId", noteId),
                    new XElement("NoteType", note.NoteType),
                    new XElement("NoteDetails", note.NoteDetails),
                    new XElement("Time", note.Time),
                    new XElement("BookmarkUrl", note.BookmarkUrl),
                    new XElement("Status", null)
                );
            _noteRepository.Add(noteXmldoc, noteData, noteXmlFilePath);
            return true;
        }

        public List<Note> GetAll(PagingParam pagingParam, out int total)
        {
            List<Note> data = new List<Note>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").OrderByDescending(p => (int)p.Element("NoteId")).Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                {
                    NoteId = (int)p.Element("NoteId"),
                    NoteType = p.Element("NoteType").Value,
                    NoteDetails = p.Element("NoteDetails").Value,
                    Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                    BookmarkUrl = p.Element("BookmarkUrl").Value,
                    Status = p.Element("Status").Value
                }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Count();
            }
            else
            {
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.Element("NoteDetails").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.Element("BookmarkUrl").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).OrderByDescending(p => (int)p.Element("NoteId")).Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                        {
                            NoteId = (int)p.Element("NoteId"),
                            NoteType = p.Element("NoteType").Value,
                            NoteDetails = p.Element("NoteDetails").Value,
                            Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                            BookmarkUrl = p.Element("BookmarkUrl").Value,
                            Status = p.Element("Status").Value,
                        }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.Element("NoteDetails").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) ||
                        x.Element("BookmarkUrl").Value.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())).Count();
            }
            return data;
        }

        public List<Note> GetRecentTodo(PagingParam pagingParam, out int total)
        {
            List<Note> data = new List<Note>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value == NoteType.Todo).OrderByDescending(p => (int)p.Element("NoteId")).Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                {
                    NoteId = (int)p.Element("NoteId"),
                    NoteType = p.Element("NoteType").Value,
                    NoteDetails = p.Element("NoteDetails").Value,
                    Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                    BookmarkUrl = p.Element("BookmarkUrl").Value,
                    Status = p.Element("Status").Value
                }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value == NoteType.Todo).Count();
            }
            else
            {
                DateTime dateFrom = new DateTime();
                DateTime dateTo = new DateTime();
                dateFrom = DateTime.Today;
                if (pagingParam.SearchString == FilterType.Today)
                {
                    dateTo = dateFrom.AddHours(23).AddMinutes(59);
                }
                else if(pagingParam.SearchString == FilterType.ThisWeek)
                {
                    dateTo = dateFrom.AddDays(6).AddHours(23).AddMinutes(59);
                }
                else if (pagingParam.SearchString == FilterType.ThisMonth)
                {
                    dateTo = dateFrom.AddDays(29).AddHours(23).AddMinutes(59);
                }
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => !string.IsNullOrEmpty(x.Element("Time").Value) && Convert.ToDateTime(x.Element("Time").Value) >= dateFrom && 
                        Convert.ToDateTime(x.Element("Time").Value) <= dateTo && x.Element("NoteType").Value == NoteType.Todo).OrderByDescending(p => (int)p.Element("NoteId"))
                        .Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                        {
                            NoteId = (int)p.Element("NoteId"),
                            NoteType = p.Element("NoteType").Value,
                            NoteDetails = p.Element("NoteDetails").Value,
                            Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                            BookmarkUrl = p.Element("BookmarkUrl").Value,
                            Status = p.Element("Status").Value
                        }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => !string.IsNullOrEmpty(x.Element("Time").Value) && Convert.ToDateTime(x.Element("Time").Value) >= dateFrom && 
                        Convert.ToDateTime(x.Element("Time").Value) <= dateTo && x.Element("NoteType").Value == NoteType.Todo).Count();
            }
            return data;
        }

        public List<Note> GetRecentReminder(PagingParam pagingParam, out int total)
        {
            List<Note> data = new List<Note>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value == NoteType.Reminder).OrderByDescending(p => (int)p.Element("NoteId")).Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                {
                    NoteId = (int)p.Element("NoteId"),
                    NoteType = p.Element("NoteType").Value,
                    NoteDetails = p.Element("NoteDetails").Value,
                    Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                    BookmarkUrl = p.Element("BookmarkUrl").Value
                }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => x.Element("NoteType").Value == NoteType.Reminder).AsQueryable().Count();
            }
            else
            {
                DateTime dateFrom = new DateTime();
                DateTime dateTo = new DateTime();
                dateFrom = DateTime.Today;
                if (pagingParam.SearchString == FilterType.Today)
                {
                    dateTo = dateFrom.AddHours(23).AddMinutes(59);
                }
                else if (pagingParam.SearchString == FilterType.ThisWeek)
                {
                    dateTo = dateFrom.AddDays(6).AddHours(23).AddMinutes(59);
                }
                else if (pagingParam.SearchString == FilterType.ThisMonth)
                {
                    dateTo = dateFrom.AddDays(29).AddHours(23).AddMinutes(59);
                }
                data = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => !string.IsNullOrEmpty(x.Element("Time").Value) && Convert.ToDateTime(x.Element("Time").Value) >= dateFrom &&
                        Convert.ToDateTime(x.Element("Time").Value) <= dateTo && x.Element("NoteType").Value == NoteType.Reminder).OrderByDescending(p => (int)p.Element("NoteId"))
                        .Skip(pagingParam.Skip).Take(pagingParam.PageSize).Select(p => new Note
                        {
                            NoteId = (int)p.Element("NoteId"),
                            NoteType = p.Element("NoteType").Value,
                            NoteDetails = p.Element("NoteDetails").Value,
                            Time = !string.IsNullOrEmpty(p.Element("Time").Value) ? Convert.ToDateTime(p.Element("Time").Value) : null,
                            BookmarkUrl = p.Element("BookmarkUrl").Value
                        }).ToList();
                total = _noteRepository.AsQueryable(noteXmldoc, "Note").Where(x => !string.IsNullOrEmpty(x.Element("Time").Value) && Convert.ToDateTime(x.Element("Time").Value) >= dateFrom &&
                        Convert.ToDateTime(x.Element("Time").Value) <= dateTo && x.Element("NoteType").Value == NoteType.Reminder).Count();
            }
            return data;
        }

        public bool ChangeTodoStatus(bool status, int noteId)
        {
            XElement note = _noteRepository.AsQueryable(noteXmldoc, "Note").FirstOrDefault(p => (int)p.Element("NoteId") == noteId);
            if (note != null)
            {
                note.Element("Status").Value = status ? "Complete" : "";
                _noteRepository.Update(noteXmldoc, noteXmlFilePath);
            }
            return true;
        }
    }
}
