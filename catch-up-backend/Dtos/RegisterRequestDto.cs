﻿namespace catch_up_backend.Dtos
{
    public class RegisterRequestDto{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Position { get; set; }
    }
}
