using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Теннисный корт обязателен к заполнению")]
        [ForeignKey(nameof(TennisCourt))]
        [DisplayName("Теннисный корт")]
        public Guid TennisCourtId { get; set; }

        [Required(ErrorMessage = "Клиент обязателен к заполнению")]
        [ForeignKey(nameof(Client))]
        [DisplayName("Клиент")]
        public Guid ClientId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата и время начала игры")]
        public DateTime StartTime {  get;  set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата и время конца игры")]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Стоиомсть должна быть больше нуля")]
        [DisplayName("Стоимость игры")]
        public decimal TotalCost { get; set; }

        [Required]
        public Status Status { get;  set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public virtual TennisCourt TennisCourt { get; set; } = null!;

        [ValidateNever]
        public virtual Client Client { get; set; } = null!;

    }

    public enum Status
    {
        [Display(Name = "Забронировано")]
        Booked,

        [Display(Name = "Активно")]
        Active,

        [Display(Name = "Завершено")]
        Completed,

        [Display(Name = "Отменено")]
        Cancelled
    }
}