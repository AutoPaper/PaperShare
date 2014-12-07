using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoPaper.Models
{
    public class users
    {
        public int ID { get; set; }
        public string keyhash { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public int role { get; set; }
        public string picAddr { get; set; }
    }
    public class notices
    {
        [Key]
        [Column(Order = 0)]
        public int userID { get; set; }
        public int noticeType { get; set; }
        [Key]
        [Column(Order = 1)]
        public string content { get; set; }
    }
    public class follow
    {
        [Key]
        [Column(Order = 0)]
        public int userID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int followID { get; set; }
    }
    public class errorHistory
    {
        [Key]
        [Column(Order = 0)]
        public int fromID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int toID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int questionID { get; set; }
        public string content { get; set; }
    }
    public class QU
    {
        [Key]
        [Column(Order = 0)]
        public int userID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int questionID { get; set; }
    }
    public class papers
    {
        public int ID { get; set; }
        public string name { get; set; }
        public float quality { get; set; }
        public int qualityVotes { get; set; }
        public float difficulty { get; set; }
        public int difficultyVotes { get; set; }
        public string SHACode { get; set; }
    }
    public class PU
    {
        [Key]
        [Column(Order = 0)]
        public int userID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int paperID { get; set; }
    }
    public class PU_Transcation
    {
        [Key]
        [Column(Order = 0)]
        public int userID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int paperID { get; set; }
        public bool hasSHA { get; set; }
        public string logAddr { get; set; }
        public DateTime logTime { get; set; }
        public string comment { get; set; }
        public DateTime commentTime { get; set; }
    }
    public class PT
    {
        [Key]
        public int paperID { get; set; }
        public int teacherID { get; set; }
        public DateTime createTime { get; set; }
        public int doneCount { get; set; }
    }
    public class PQ
    {
        [Key]
        [Column(Order = 0)]
        public int paperID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int questionID { get; set; }
    }
    public class tags
    {
        [Key]
        public int ID { get; set; }
        public string content { get; set; }
    }
    public class PTags
    {
        [Key]
        [Column(Order = 0)]
        public int tagID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int paperID { get; set; }
    }
    public class question
    {
        public int ID { get; set; }
        public int teacherID { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public string answer { get; set; }
        public int questionType { get; set; }
        public int difficulty { get; set; }
        public int citationCount { get; set; }
        public DateTime updateTime { get; set; }
    }
    public class QTags
    {
        [Key]
        [Column(Order = 0)]
        public int tagID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int questionID { get; set; }
    }
    public class PaperShareDBContext : DbContext
    {
        public DbSet<users> user_table { get; set; }
        public DbSet<notices> notice_table { get; set; }
        public DbSet<follow> follow_table { get; set; }
        public DbSet<errorHistory> error_table { get; set; }
        public DbSet<QU> QU_table { get; set; }
        public DbSet<papers> paper_table { get; set; }
        public DbSet<PU> PU_table { get; set; }
        public DbSet<PU_Transcation> PU_Transcation_table { get; set; }
        public DbSet<PT> PT_table { get; set; }
        public DbSet<PQ> PQ_table { get; set; }
        public DbSet<tags> tag_table { get; set; }
        public DbSet<PTags> PTags_table { get; set; }
        public DbSet<question> question_table { get; set; }
        public DbSet<QTags> QTags_table { get; set; }
    }
}