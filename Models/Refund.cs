using BoaSaudeRefund.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace BoaSaudeRefund
{
    public class Refund
    {
        public Int64 Id { get; set; }
        public EnumStateRefund Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string NFeNumber { get; set; }
        public string NFeLink { get; set; }
        public double Price { get; set; }
        public string CNPJProvider { get; set; }
        public string UserId { get; set; }
        public byte[] File { get; set; }
        public DateTime CreatedAt { get; set; }
        public EnumStateRefund Status { get; set; } = EnumStateRefund.Novo;
        public string UserNameAnalyst { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


    public class RefundRegisterDto
    {
        [Required]
        public EnumStateRefund Type { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string NFeNumber { get; set; }
        [Required]
        public string NFeLink { get; set; }
        [Required]
        public string CNPJProvider { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class RefundEditDto
    {
        public Int64 Id { get; set; }
        [Required]
        public EnumStateRefund Status { get; set; }
    }

    public class RefundMessage
    {
        public long Id { get; set; }
        public string User { get; internal set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string NFeNumber { get; set; }
        public string NFeLink { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
