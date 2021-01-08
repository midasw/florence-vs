using BackOffice_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice_ASP.ViewModels.api
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string BodyParsed { get; set; }
        public bool IsPublished { get; set; }
        public bool IsEdited { get; set; }
        public ApplicationUserViewModel Author { get; set; }
        public ApplicationUserViewModel Editor { get; set; }

        public AnnouncementViewModel() { }

        public AnnouncementViewModel(Announcement a)
        {
            Id = a.Id;
            DateCreated = a.DateCreated;
            DateEdited = a.DateEdited;
            Subject = a.Subject;
            Body = a.Body;
            BodyParsed = a.BodyParsed;
            IsPublished = a.IsPublished;
            IsEdited = a.IsEdited;

            if (a.Author != null)
            {
                Author = new ApplicationUserViewModel
                {
                    DateJoined = a.Author.DateJoined,
                    FirstName = a.Author.FirstName,
                    LastName = a.Author.LastName,
                    Country = a.Author.Country,
                    Avatar = a.Author.Avatar,
                    AvatarUrl = a.Author.AvatarUrl
                };
            }
            if (a.Editor != null)
            {
                Editor = new ApplicationUserViewModel
                {
                    DateJoined = a.Editor.DateJoined,
                    FirstName = a.Editor.FirstName,
                    LastName = a.Editor.LastName,
                    Country = a.Editor.Country,
                    Avatar = a.Editor.Avatar,
                    AvatarUrl = a.Editor.AvatarUrl
                };
            }
        }
    }
}
