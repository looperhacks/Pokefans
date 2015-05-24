﻿// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// Discarded     = Will not be published, abandoned work
    /// WorkInProcess = Content unfinished, work in process.
    /// Ready         = Content finished, waiting for review and publish
    /// Published     = self-explanatory
    /// </summary>
    public enum ContentStatus { WorkInProcess, Ready, Published, Discarded }

    public enum HomePageOptions { Normal, NotOnHomePage, FixedOnHomePage }

    public enum ContentType { Article, News, Special, Boilerplate }

    [Table("content")]
    public class Content
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DefaultValue(ContentStatus.WorkInProcess)]
        public ContentStatus Status { get; set; }

        [DefaultValue(ContentType.Article)]
        public ContentType Type { get; set; }

        [Required]
        public int Version { get; set; }

        public int CategoryId { get; set; }

        [MaxLength(255, ErrorMessage="Der Titel darf maximal 255 Zeichen lang sein.")]
        public string Title { get; set; }

        public string UnparsedContent { get; set; }

        public string ParsedContent { get; set; }

        public string Description { get; set; }

        public string Teaser { get; set; }

        public string Notes { get; set; }

        public int EditPermissionId { get; set; }

        public HomePageOptions HomePageOptions { get; set; }

        [MaxLength(100)]
        public string StylesheetName { get; set; }

        public string StylesheetCode { get; set; }

        public string ViewCount { get; set; }

        public DateTime Published { get; set; }

        public int PublishedByUserId { get; set; }

        public DateTime Updated { get; set; }

        public int UpdatedByUserId { get; set; }

        public double UpdateMagnificance { get; set; }

        public int UpdateCharsChanged { get; set; }

        public int UpdateCharsDeleted { get; set; }

        public int DefaultUrlId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ContentCategory Category { get; set; }

        [ForeignKey("PublishedByUserId")]
        public virtual User PublishedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("DefaultUrlId")]
        public virtual ContentUrl DefaultUrl { get; set; }
    }
}