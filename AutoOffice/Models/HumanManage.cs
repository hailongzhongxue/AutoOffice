using System.ComponentModel.DataAnnotations;

namespace AutoOffice.Models
{
    public class HumanManage
    {
        public string ID { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string Job { get; set; }
    }
}
