using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } //entity framework will recognise it correctly 
        public string UserName { get; set; }//for .netcore identity 
        public byte[] PasswordHash{get; set;}
        public byte[] PasswordSalt { get; set; }
    }
}