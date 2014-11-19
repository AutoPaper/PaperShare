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
        public string name { get; set; }
        public int authority { get; set; }
        public string picAddr { get; set; }
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
    public class papers
    {
        public int ID { get; set; }
        public string paperAddr { get; set; }
        public string name { get; set; }
        public bool isPublic { get; set; }
        public float quality { get; set; }
        public int qualityVotes { get; set; }
        public float difficulty { get; set; }
        public int difficultyVotes { get; set; }
        public string SHA { get; set; }
    }
    public class PU
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
    public class knowledgeTree
    {
        [Key]
        public int ID { get; set; }
        public string knowledge { get; set; }
        public int parentID { get; set; }
    }
    public class PK
    {
        [Key]
        [Column(Order = 0)]
        public string knowledge { get; set; }
        [Key]
        [Column(Order = 1)]
        public int paperID { get; set; }
    }
    public class question
    {
        public int ID { get; set; }
        public string content { get; set; }
        public string answer { get; set; }
        public int questionType { get; set; }
        public int difficulty { get; set; }
        public int citation { get; set; }
    }
    public class QK
    {
        [Key]
        [Column(Order = 0)]
        public string knowledge { get; set; }
        [Key]
        [Column(Order = 1)]
        public int questionID { get; set; }
    }
    public class selectHistory
    {
        [Key]
        [Column(Order = 0)]
        public int questionID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int teacherID { get; set; }
        public DateTime time { get; set; }
    }
    public class PaperShareDBContext : DbContext
    {
        public DbSet<users> user_table { get; set; }
        public DbSet<follow> follow_table { get; set; }
        public DbSet<papers> paper_table { get; set; }
        public DbSet<PU> PU_table { get; set; }
        public DbSet<PT> PT_table { get; set; }
        public DbSet<knowledgeTree> knowledgeTree_table { get; set; }
        public DbSet<PK> PK_table { get; set; }
        public DbSet<question> question_table { get; set; }
        public DbSet<QK> QK_table { get; set; }
        public DbSet<selectHistory> selectHistory_table { get; set; }
    }
}