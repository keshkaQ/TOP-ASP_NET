using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class TennisCourt
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название корта обязательно к заполнению")]
        [MaxLength(100, ErrorMessage = "Название корта не может превышать 100 символов")]
        [Display(Name = "Название корта")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Стоимость за час обязательна к заполнению")]
        [Range(1, 99999, ErrorMessage = "Стоимость за час должна быть в диапазоне от 1 до 99,999")]
        [Display(Name = "Стоимость корта за час")]
        public decimal HourlyRate { get; set; }

        [Required(ErrorMessage = "Описание обязательно к заполнению")]
        [MaxLength(500, ErrorMessage = "Описание не может превышать 500 символов")]
        [Display(Name="Описание")]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
