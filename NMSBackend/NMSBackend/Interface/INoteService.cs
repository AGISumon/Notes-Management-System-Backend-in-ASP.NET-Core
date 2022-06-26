using NMSBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMSBackend.Interface
{
    public interface INoteService
    {
        bool Add(Note note);
        List<Note> GetAll(PagingParam param, out int total);
        List<Note> GetRecentTodo(PagingParam param, out int total);
        List<Note> GetRecentReminder(PagingParam param, out int total);
        bool ChangeTodoStatus(bool status, int noteId);
    }
}
