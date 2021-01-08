using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HtmlAgilityPack;
using MarkdownDeep;

namespace BackOffice_ASP.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Display(Name = "Last edited")]
        public DateTime DateEdited { get; set; } = DateTime.Now;

        [Required]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public string BodyParsed { get; set; }

        [Display(Name="Published?")]
        public bool IsPublished { get; set; } = false;
        public bool IsEdited { get; set; } = false;

        public ApplicationUser Author { get; set; }
        public ApplicationUser Editor { get; set; }

        public void ParseMarkdown()
        {
            BodyParsed = _markdown.Transform(Body);
        }

        public string GetStrippedBody()
        {
            _htmlDoc.LoadHtml(BodyParsed);
            string result = _htmlDoc.DocumentNode.InnerText;

            return result;
        }

        static Announcement()
        {
            _htmlDoc = new HtmlDocument();

            _markdown = new Markdown();
            _markdown.ExtraMode = true;
            _markdown.SafeMode = true;
        }

        private static HtmlDocument _htmlDoc;
        private static Markdown _markdown;
    }
}
