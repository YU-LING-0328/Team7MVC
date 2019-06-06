using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.Models
{
    public class Messages
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入Email")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{4}\-?\d{3}\-?\d{3}$", ErrorMessage = "需為09xx-xxx-xxx格式")]
        public string Phone { get; set; }

        [Required]
        public string QuestionCategory { get; set; }

        [Required(ErrorMessage = "請輸入您的意見")]
        public string Comments { get; set; }

        [Required]
        public DateTime Datetime { get; set; }
    }
}