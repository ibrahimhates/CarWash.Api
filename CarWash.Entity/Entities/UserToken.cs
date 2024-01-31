using CarWash.Core.Entity;
using CarWash.Entity.Enums;

namespace CarWash.Entity.Entities
{
    public class UserToken : EntityBase
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public TokenTypes TokenType { get; set; }
        public WhosToken WhosToken { get; set; }
    }
}
