using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно к заполнению")]
        [MaxLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна к заполнению")]
        [MaxLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Почта обязательна к заполнению")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес")]
        [MaxLength(255, ErrorMessage = "Почта не может превышать 255 символов")]
        [Display(Name = "Почта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Номер телефона обязателен к заполнению")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        [MaxLength(20, ErrorMessage = "Номер телефона не может превышать 20 символов")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
