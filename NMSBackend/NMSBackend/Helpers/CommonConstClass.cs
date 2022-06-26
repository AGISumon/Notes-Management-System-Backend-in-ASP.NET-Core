using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class MessageConst
    {
        public const string Insert = "Record has been saved.",
        Update = "Record has been updated.",
        Cancel = "Record has been canceled!",
        SystemError = "System error!",
        IsExist = "Already exist!",
        Failed = "Failed!",
        InvalidUser = "Invalid userId or password!",
        InvalidEmailId = "Invalid email!",
        InvalidPassword = "Wrong password!",
        SuccussLogin = "Login successful.",
        InvalidRefreshToken = "Invalid refresh token!",
        NotFound = "No record found!";
    }
    public static class RecordStatus
    {
        public const string Active = "A",
        Inactive = "I";
    }
    public static class NoteType
    {
        public const string RegularNote = "Regular Note",
        Reminder = "Reminder", Todo = "Todo", Bookmark = "Bookmark";
    }
    public static class FilterType
    {
        public const string Today = "1",
        ThisWeek = "2", ThisMonth = "3";
    }
}
