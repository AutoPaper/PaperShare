using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AutoPaper.Models
{
    public class users
    {
        public int ID { get; set; }
        public string keyhash { get; set; }
        public string name { get; set; }
        public int authroity { get; set; }
        public string picAddr { get; set; }
    }
    public class follow
    {
        public int userID { get; set; }
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
        public int userID { get; set; }
        public int paperID { get; set; }
        public bool hasSHA { get; set; }
        public string logAddr { get; set; }
        public DateTime logTime { get; set; }
        public string commentAddr { get; set; }
        public DateTime commentTime { get; set; }
    }
    public class PT
    {
        public int teacherID { get; set; }
        public int paperID { get; set; }
        public DateTime createTime { get; set; }
        public int doneCount { get; set; }
    }
    public class knowledgeTree
    {
        public string knowledge { get; set; }
        public int treeIndex { get; set; }
        public int parent { get; set; }
    }
    public class PK
    {
        public string knowledge { get; set; }
        public int paperID { get; set; }
    }
    public class question
    {
        public int ID { get; set; }
        public string questionAddr { get; set; }
        public string answer { get; set; }
        public int questionType { get; set; }
        public int difficulty { get; set; }
    }
    public class QK
    {
        public string knowledge { get; set; }
        public int questionID { get; set; }
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
    }
}