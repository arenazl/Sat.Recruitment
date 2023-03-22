using System;
namespace Sat.Recruitment.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public void ApplyAmountBasedInUserType()
        {
            var typeFactors = new Dictionary<UserType, decimal>
        {
            { UserType.Normal, 0.8m },
            { UserType.SuperUser, 0.2m },
            { UserType.Premium, 2m }
        };

            if (typeFactors.ContainsKey(UserType))
            {
                var factor = typeFactors[UserType];
                Money = Money + (Money * factor);
            }

            if(UserType == UserType.Normal)
                Money = Money > 100 ? Money + (Money * 12m) : Money;
        }

        public void NormalizeMail()
        {
            string email = Email.Trim();
            if (!string.IsNullOrWhiteSpace(email))
            {
                string[] emailParts = email.Split('@');
                if (emailParts.Length == 2)
                {
                    string username = emailParts[0].Replace(".", string.Empty);
                    int plusIndex = username.IndexOf("+");
                    if (plusIndex >= 0)
                    {
                        username = username.Remove(plusIndex);
                    }
                    Email = $"{username}@{emailParts[1]}";
                }
            }
        }
    }

    public enum UserType
    {
        Normal,
        SuperUser,
        Premium
    }

}

